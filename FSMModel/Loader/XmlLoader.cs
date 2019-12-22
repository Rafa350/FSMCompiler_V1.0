namespace MikroPicDesigns.FSMCompiler.v1.Loader {

    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Commands;
    using System;
    using System.Xml;

    public class XmlLoader {

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

            if (node.Attributes[name] == null)
                return null;
            else
                return node.Attributes[name].Value;
        }

        /// <summary
        /// Crea un objecte Machine a partir d'un node XML
        /// </summary>
        /// <param name="machineNode">El node a procesar.</param>
        /// <returns>L'objecte 'machine' creat.</returns>
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
            if (initializeNode != null)
                machine.InitializeAction = ProcessActionNode(initializeNode, machine);

            XmlNode terminateNode = machineNode.SelectSingleNode("terminate");
            if (terminateNode != null)
                machine.TerminateAction = ProcessActionNode(terminateNode, machine);

            // Procesa cada estat i asigna els parametres
            //
            foreach (XmlNode stateNode in machineNode.SelectNodes("state")) {
                ProcessStateNode(stateNode, machine);
            }

            startStateName = startStateName.Replace(":", "");
            machine.Start = GetState(machine, startStateName);

            return machine;
        }

        private void ProcessStateNode(XmlNode stateNode, Machine machine) {

            string stateName = null;
            if (stateNode.ParentNode.Name == "state")
                stateName = GetAttribute(stateNode.ParentNode, "name");
            stateName += GetAttribute(stateNode, "name");

            State state = GetState(machine, stateName);

            XmlNode onEnterNode = stateNode.SelectSingleNode("enter");
            if (onEnterNode != null)
                state.EnterAction = ProcessActionNode(onEnterNode, machine);

            XmlNode onExitNode = stateNode.SelectSingleNode("exit");
            if (onExitNode != null)
                state.ExitAction = ProcessActionNode(onExitNode, machine);

            foreach (XmlNode onEventNode in stateNode.SelectNodes("transition")) {
                Transition transition = ProcessTransitionNode(onEventNode, machine);
                state.AddTransition(transition);
            }

            foreach (XmlNode childStateNode in stateNode.SelectNodes("state"))
                ProcessStateNode(childStateNode, machine);
        }

        private Model.Action ProcessActionNode(XmlNode actionNode, Machine machine) {

            Model.Action action = new Model.Action();

            foreach (XmlNode node in actionNode.ChildNodes) {
                switch (node.Name) {
                    case "inline": 
                        action.AddCommand(ProcessInlineActionNode(node, machine));
                        break;

                    case "raise":
                        action.AddCommand(ProcessRaiseActionNode(node, machine));
                        break;
                }
            }

            return action;
        }

        private Command ProcessInlineActionNode(XmlNode inlineActionNode, Machine machine) {

            InlineCommand command = new InlineCommand();
            command.Text = inlineActionNode.InnerText;

            return command;
        }

        private Command ProcessRaiseActionNode(XmlNode raiseActionNode, Machine machine) {

            string eventName = GetAttribute(raiseActionNode, "event");
            string delayText = GetAttribute(raiseActionNode, "delay");

            if (String.IsNullOrEmpty(eventName))
                throw new InvalidOperationException(String.Format("No se declaro el evento '{0}'.", eventName));

            RaiseCommand command = new RaiseCommand();
            command.Event = GetEvent(machine, eventName);
            command.DelayText = delayText;

            return command;
        }

        private Transition ProcessTransitionNode(XmlNode transitionNode, Machine machine) {

            string transitionName = GetAttribute(transitionNode, "name");
            if (String.IsNullOrEmpty(transitionName))
                throw new Exception("No se especifico el atributo 'name'");

            Transition transition = new Transition(transitionName);

            // Obte l'event
            //
            string eventName = GetAttribute(transitionNode, "event");
            if (!String.IsNullOrEmpty(eventName))
                transition.Event = GetEvent(machine, eventName);

            // Obte la guarda
            //
            string condition = GetAttribute(transitionNode, "guard");
            if (!String.IsNullOrEmpty(condition))
                transition.Guard = new Guard(condition);

            string name = GetAttribute(transitionNode, "state");
            if (System.String.IsNullOrEmpty(name))
                transition.Mode = TransitionMode.Null;

            else {
                if (name == "*")
                    transition.Mode = TransitionMode.ReturnFromState;

                else {
                    if (name.Contains(":"))
                        name = name.Replace(":", "");
                    else {
                        XmlNode node = transitionNode.ParentNode.ParentNode;
                        while (node.Name == "state") {
                            name = System.String.Format("{0}{1}", GetAttribute(node, "name"), name);
                            node = node.ParentNode;
                        }
                    }
                    
                    transition.NextState = GetState(machine, name);
                    transition.Mode = TransitionMode.JumpToState;
                }
            }

            if (transitionNode.HasChildNodes)
                transition.Action = ProcessActionNode(transitionNode, machine);

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

        /// <summary>
        /// Obte l'event especificat. Si no existeix, el crea.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <param name="name">Nom de l'event.</param>
        /// <returns>L'event.</returns>
        /// 
        private static Event GetEvent(Machine machine, string name) {

            Event ev = machine.GetEvent(name, false);
            if (ev == null) {
                ev = new Event(name);
                machine.AddEvent(ev);
            }
            return ev;
        }
    }
}
