namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;

    public sealed class Machine: IVisitable {

        private readonly string name;
        private State startState;
        private Dictionary<string, State> states = new Dictionary<string, State>();
        private Dictionary<string, Event> events = new Dictionary<string, Event>();

        public Machine(string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.name = name;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public void AddState(State state) {

            if (state == null)
                throw new ArgumentNullException("state");

            if (states.ContainsKey(state.FullName))
                throw new InvalidOperationException(
                    String.Format("El estado '{0}' ya ha sido agregado.", state.FullName));

            states.Add(state.FullName, state);
        }

        public void AddEvent(Event ev) {

            if (ev == null)
                throw new ArgumentNullException("ev");

            if (events.ContainsKey(ev.Name))
                throw new InvalidOperationException(
                    String.Format("El evento '{0}' ya ha sido agregado.", ev.Name));

            events.Add(ev.Name, ev);
        }

        public State GetState(string fullName) {

            if (String.IsNullOrEmpty(fullName))
                throw new ArgumentNullException("fullName");

            if (!states.ContainsKey(fullName))
                throw new InvalidOperationException(
                    String.Format("No se agrego ningun estado con el nombre '{0}'.", fullName));

            return states[fullName];
        }

        public Event GetEvent(string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (!events.ContainsKey(name))
                throw new InvalidOperationException(
                    String.Format("No se agrego ningun evento con el nombre '{0}'.", name));

            return events[name];
        }

        public string Name {
            get { 
                return name; 
            }
        }

        public State StartState {
            get {
                return startState;
            }
            set {
                startState = value;
            }
        }

        public IEnumerable<string> StateNames {
            get {
                return states.Keys;
            }
        }

        public IEnumerable<string> EventNames {
            get {
                return events.Keys;
            }
        }

        public int StateCount {
            get {
                return states.Count;
            }
        }

        public int EventCount {
            get {
                return events.Count;
            }
        }

        public IEnumerable<State> States {
            get {
                return states.Values;
            }
        }

        public IEnumerable<State> FinalStates {
            get {
                List<State> finalStates = new List<State>();
                foreach (State state in states.Values)
                    if (!state.HasChilds)
                        finalStates.Add(state);
                return finalStates;
            }
        }

        public IEnumerable<Event> Events {
            get {
                return events.Values;
            }
        }
    }
}
