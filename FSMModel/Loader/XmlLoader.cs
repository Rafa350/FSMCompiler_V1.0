namespace MikroPicDesigns.FSMCompiler.v1.Loader {

    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Actions;
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

        private Machine ProcessMachineNode(XmlNode machineNode) {

            string machineName = GetAttribute(machineNode, "name");
            string startStateName = GetAttribute(machineNode, "initialState");

            Machine machine = new Machine(machineName);

            // Procesa cada estat i asigna els parametres
            //
            foreach (XmlNode stateNode in machineNode.SelectNodes("state")) 
                ProcessStateNode(stateNode, machine);

            startStateName = startStateName.Replace(":", "");
            machine.InitialState = GetState(machine, startStateName);

            return machine;
        }

        private void ProcessStateNode(XmlNode stateNode, Machine machine) {

            string stateName = null;
            if (stateNode.ParentNode.Name == "state")
                stateName = GetAttribute(stateNode.ParentNode, "name");
            stateName += GetAttribute(stateNode, "name");

            State state = GetState(machine, stateName);

            XmlNode onEnterNode = stateNode.SelectSingleNode("onEnter");
            if (onEnterNode != null)
                state.EntryAction = ProcessActionNode(onEnterNode, machine);

            XmlNode onExitNode = stateNode.SelectSingleNode("onExit");
            if (onExitNode != null)
                state.ExitAction = ProcessActionNode(onExitNode, machine);

            foreach (XmlNode onEventNode in stateNode.SelectNodes("onEvent")) {
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

            Transition transition = new Transition();

            // Obte l'event
            //
            string eventName = GetAttribute(transitionNode, "event");
            if (!String.IsNullOrEmpty(eventName))
                transition.Event = GetEvent(machine, eventName);

            // Obte la guarda
            //
            string condition = GetAttribute(transitionNode, "condition");
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
                    
                    transition.Next = GetState(machine, name);
                    transition.Mode = TransitionMode.JumpToState;
                }
            }

            if (transitionNode.HasChildNodes)
                transition.Action = ProcessActionNode(transitionNode, machine);

            return transition;
        }

        private static State GetState(Machine machine, string name) {

            State ev = machine.GetState(name, false);
            if (ev == null) {
                ev = new State(name);
                machine.AddState(ev);
            }
            return ev;
        }

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
