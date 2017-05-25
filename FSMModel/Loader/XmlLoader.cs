namespace MikroPicDesigns.FSMCompiler.v1.Loader {

    using System;
    using System.Collections.Generic;
    using System.Xml;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Actions;

    public class XmlLoader {

        private Dictionary<string, State> stateDic = new Dictionary<string, State>();
        private Dictionary<string, Event> eventDic = new Dictionary<string, Event>();

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

        private void LoadEvents(Machine machine, XmlNode machineNode) {

            foreach (XmlNode eventNode in machineNode.SelectNodes("events/event")) {

                string eventName = GetAttribute(eventNode, "name");

                if (eventDic.ContainsKey(eventName))
                    throw new InvalidOperationException(String.Format("Ya se declaro el evento '{0}'.", eventName));

                Event ev = new Event(eventName);
                eventDic.Add(ev.Name, ev);
            
                machine.AddEvent(ev);
            }
        }

        private void LoadStates(Machine machine, XmlNode machineNode) {

            foreach (XmlNode stateNode in machineNode.SelectNodes("state"))
                LoadStates(machine, null, stateNode);
        }

        private void LoadStates(Machine machine, State parent, XmlNode stateNode) {

            string stateName = GetAttribute(stateNode, "name");
            State state = new State(parent, stateName);

            foreach (XmlNode childStateNode in stateNode.SelectNodes("state"))
                LoadStates(machine, state, childStateNode);

            if (stateDic.ContainsKey(state.FullName))
                throw new InvalidOperationException(
                    String.Format("Ya existe un estado con el nombre '{0}'.", state.FullName));
            stateDic.Add(state.FullName, state);
            
            machine.AddState(state);
        }

        private Machine ProcessMachineNode(XmlNode machineNode) {

            string machineName = GetAttribute(machineNode, "name");
            string startStateName = GetAttribute(machineNode, "initialState");

            Machine machine = new Machine(machineName);

            // Recolecta tots els estats i els crea
            //
            LoadStates(machine, machineNode);

            // Recolecta tots els events i els crea
            //
            LoadEvents(machine, machineNode);

            // Recolecta totes les condicions i les crea
            //

            // Procesa cada estat i asigna els parametres
            //
            foreach (XmlNode stateNode in machineNode.SelectNodes("state")) 
                ProcessStateNode(stateNode, machine);

            startStateName = startStateName.Replace(":", "");
            machine.StartState = stateDic[startStateName];

            return machine;
        }

        private void ProcessStateNode(XmlNode stateNode, Machine machine) {

            string stateName = null;
            if (stateNode.ParentNode.Name == "state")
                stateName = GetAttribute(stateNode.ParentNode, "name");
            stateName += GetAttribute(stateNode, "name");

            State state = machine.GetState(stateName);

            XmlNode onEnterNode = stateNode.SelectSingleNode("onEnter");
            if (onEnterNode != null)
                state.EnterActions = ProcessActionNode(onEnterNode);

            XmlNode onExitNode = stateNode.SelectSingleNode("onExit");
            if (onExitNode != null)
                state.ExitActions = ProcessActionNode(onExitNode);

            TransitionList transitions = new TransitionList();
            foreach (XmlNode onEventNode in stateNode.SelectNodes("onEvent")) {
                Transition transition = ProcessTransitionNode(onEventNode);
                transitions.Add(transition);
            }
            state.Transitions = transitions;

            foreach (XmlNode childStateNode in stateNode.SelectNodes("state"))
                ProcessStateNode(childStateNode, machine);
        }

        private ActionList ProcessActionNode(XmlNode actionNode) {

            ActionList actions = new ActionList();

            foreach (XmlNode node in actionNode.ChildNodes) {
                switch (node.Name) {
                    case "inline": 
                        actions.Add(ParseInlineActionNode(node));
                        break;

                    case "goto":
                        actions.Add(ParseGotoActionNode(node));
                        break;

                    case "raise":
                        actions.Add(ParseRaiseActionNode(node));
                        break;
                }
            }

            return actions;
        }

        private ActionBase ParseInlineActionNode(XmlNode inlineActionNode) {

            string condition = GetAttribute(inlineActionNode, "condition");

            InlineAction action = new InlineAction();
            action.Text = inlineActionNode.InnerText;
            action.Condition = condition;

            return action;
        }

        private ActionBase ParseGotoActionNode(XmlNode inlineActionNode) {

            string stateName = GetAttribute(inlineActionNode, "nextState");
            if (stateName.Contains(":"))
                stateName = stateName.Replace(":", "");

            if (!stateDic.ContainsKey(stateName))
                throw new InvalidOperationException(String.Format("No declaro el estado '{0}'.", stateName));

            GotoAction action = new GotoAction();
            action.Next = stateDic[stateName];

            return action;
        }

        private ActionBase ParseRaiseActionNode(XmlNode raiseActionNode) {

            string eventName = GetAttribute(raiseActionNode, "event");
            string delayText = GetAttribute(raiseActionNode, "delay");
            string condition = GetAttribute(raiseActionNode, "condition");

            if (!eventDic.ContainsKey(eventName))
                throw new InvalidOperationException(String.Format("No se declaro el evento '{0}'.", eventName));

            RaiseAction action = new RaiseAction();
            action.Event = eventDic[eventName];
            action.DelayText = delayText;
            action.Condition = condition;

            return action;
        }

        private Transition ProcessTransitionNode(XmlNode transitionNode) {

            string eventName = GetAttribute(transitionNode, "event");

            if (!eventDic.ContainsKey(eventName))
                throw new InvalidOperationException(String.Format("No se declaro el evento '{0}'.", eventName));

            Transition transition = new Transition();
            transition.Event = eventDic[eventName];
            transition.Condition = GetAttribute(transitionNode, "condition");

            string name = GetAttribute(transitionNode, "nextState");
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
                    
                    try {
                        transition.Next = stateDic[name];
                    }
                    catch (System.Exception ex) {
                        throw new System.InvalidOperationException(
                            System.String.Format("No se encontro el estado '{0}'.", name), ex);
                    }

                    transition.Mode = TransitionMode.JumpToState;
                }
            }

            transition.Actions = ProcessActionNode(transitionNode);

            return transition;
        }
    }
}
