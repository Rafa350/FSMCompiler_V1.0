namespace MikroPicDesigns.FSMCompiler.v1.Loader {

    using System;
    using System.Collections.Generic;
    using System.Xml;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    public class XmlLoader {

        private readonly IDictionary<string, State> stateDictionary = new Dictionary<string, State>();

        public Machine Load(string fileName) {

            XmlDocument document = ReadDocument(fileName);
            return ProcessMachineNode(document.SelectSingleNode("machine"));
        }

        private static XmlDocument ReadDocument(string fileName) {

            XmlDocument document = new XmlDocument();
            document.Load(fileName);
            return document;
        }

        private static string GetAttribute(XmlNode node, string name) {

            if (node.Attributes[name] == null) {
                return null;
            }
            else {
                return node.Attributes[name].Value;
            }
        }

        /// <summary>
        /// Procesa un node 'machine'
        /// </summary>
        /// <param name="machineNode">El node.</param>
        /// <returns>Un objecte 'Machine'.</returns>
        /// 
        private Machine ProcessMachineNode(XmlNode machineNode) {

            string machineName = GetAttribute(machineNode, "name");
            if (String.IsNullOrEmpty(machineName)) 
                throw new Exception("No se especifico el atributo 'name'");

            string startStateName = GetAttribute(machineNode, "start");
            if (String.IsNullOrEmpty(startStateName)) 
                throw new Exception("No se especifico el atributo 'start'");

            Machine machine = new Machine(machineName);

            XmlNode initializeNode = machineNode.SelectSingleNode("initialize");
            Model.Action initializeAction = initializeNode == null ? null : ProcessActionNode(initializeNode);

            XmlNode terminateNode = machineNode.SelectSingleNode("terminate");
            Model.Action terminateAction = terminateNode == null ? null : ProcessActionNode(terminateNode);

            // Procesa cada estat i asigna els parametres
            //
            foreach (XmlNode stateNode in machineNode.SelectNodes("state")) 
                ProcessStateNode(stateNode, machine);

            startStateName = startStateName.Replace(":", "");
            machine.Start = GetState(machine, startStateName);
            machine.InitializeAction = initializeAction;
            machine.TerminateAction = terminateAction;

            return machine;
        }

        /// <summary>
        /// Procesa un node 'state'
        /// </summary>
        /// <param name="stateNode">El node.</param>
        /// <param name="machine">El objecte 'Machine'</param>
        /// 
        private void ProcessStateNode(XmlNode stateNode, Machine machine) {

            string stateName = null;
            if (stateNode.ParentNode.Name == "state") 
                stateName = GetAttribute(stateNode.ParentNode, "name");

            stateName += GetAttribute(stateNode, "name");

            State state = GetState(machine, stateName);

            XmlNode onEnterNode = stateNode.SelectSingleNode("enter");
            if (onEnterNode != null) 
                state.EnterAction = ProcessActionNode(onEnterNode);

            XmlNode onExitNode = stateNode.SelectSingleNode("exit");
            if (onExitNode != null) 
                state.ExitAction = ProcessActionNode(onExitNode);

            foreach (XmlNode transitionNode in stateNode.SelectNodes("transition")) {
                Transition transition = ProcessTransitionNode(transitionNode, machine);
                state.AddTransition(transition);
            }

            foreach (XmlNode childStateNode in stateNode.SelectNodes("state")) 
                ProcessStateNode(childStateNode, machine);
        }

        /// <summary>
        /// Procesa el node 'action'
        /// </summary>
        /// <param name="actionNode">El node a procesar.</param>
        /// <returns>Un objecte 'Action'.</returns>
        /// 
        private Model.Action ProcessActionNode(XmlNode actionNode) {

            List<Activity> activityList = new List<Activity>();

            if (actionNode.HasChildNodes)
                foreach (XmlNode childNode in actionNode.ChildNodes) {
                    switch (childNode.Name) {
                        case "inline":
                            activityList.Add(ProcessInlineActivityNode(childNode));
                            break;

                        case "run":
                            activityList.Add(ProcessRunActivityNode(childNode));
                            break;

                        case "throw":
                            activityList.Add(ProcessThrowActivityNode(childNode));
                            break;
                    }
                }

            return new Model.Action(activityList);
        }

        /// <summary>
        /// Procesa un node 'inline'
        /// </summary>
        /// <param name="actionNode">El node.</param>
        /// <returns>'objecte 'Activity'</returns>
        /// 
        private Activity ProcessInlineActivityNode(XmlNode actionNode) {

            string text = actionNode.InnerText;
            return new InlineActity(text);
        }

        /// <summary>
        /// Procesa un node 'run'
        /// </summary>
        /// <param name="actionNode">El node.</param>
        /// <returns>'objecte 'Activity'</returns>
        /// 
        private Activity ProcessRunActivityNode(XmlNode actionNode) {

            string processName = GetAttribute(actionNode, "name");
            return new RunActivity(processName);
        }

        /// <summary>
        /// Procesa un node 'throw'
        /// </summary>
        /// <param name="actionNode">El node.</param>
        /// <returns>'objecte 'Activity'</returns>
        /// 
        private Activity ProcessThrowActivityNode(XmlNode actionNode) {

            string transitionName = GetAttribute(actionNode, "name");
            return new ThrowActivity(transitionName);
        }

        private Transition ProcessTransitionNode(XmlNode transitionNode, Machine machine) {

            // Obte el nom
            //
            string transitionName = GetAttribute(transitionNode, "name");
            if (String.IsNullOrEmpty(transitionName)) {
                throw new Exception("No se especifico el atributo 'name'");
            }

            Transition transition = new Transition(transitionName);

            // Obte la guarda
            //
            string condition = GetAttribute(transitionNode, "guard");
            if (!String.IsNullOrEmpty(condition)) {
                transition.Guard = new Guard(condition);
            }

            // Obte el nou estat
            //
            string name = GetAttribute(transitionNode, "state");
            if (System.String.IsNullOrEmpty(name)) {
                transition.Mode = TransitionMode.InternalLoop;
            }
            else {
                if (name == "*") {
                    transition.Mode = TransitionMode.Pop;
                }
                else {
                    if (name.Contains(":")) {
                        name = name.Replace(":", "");
                    }
                    else {
                        XmlNode node = transitionNode.ParentNode.ParentNode;
                        while (node.Name == "state") {
                            name = System.String.Format("{0}{1}", GetAttribute(node, "name"), name);
                            node = node.ParentNode;
                        }
                    }

                    transition.NextState = GetState(machine, name);
                    transition.Mode = TransitionMode.Jump;
                }
            }

            if (transitionNode.HasChildNodes) 
                transition.Action = ProcessActionNode(transitionNode);

            return transition;
        }

        /// <summary>
        /// Obte l'estat especificat. Si no existeix, el crea
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <param name="name">Nom de l'estat.</param>
        /// <returns>L'estat.</returns>
        /// 
        private static State GetState(Machine machine, string name) {

            State ev = machine.GetState(name, false);
            if (ev == null) {
                ev = new State(name);
                machine.AddState(ev);
            }
            return ev;
        }

    }
}
