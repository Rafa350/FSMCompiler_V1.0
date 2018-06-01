namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;

    public sealed class Machine: IVisitable {

        private readonly Dictionary<string, State> states = new Dictionary<string, State>();
        private readonly Dictionary<string, Event> events = new Dictionary<string, Event>();
        private readonly string name;
        private State start;

        public Machine(string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.name = name;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        /// <summary>
        /// Afegeix un estat a la maquina.
        /// </summary>
        /// <param name="state">L'estat a afeigir.</param>
        /// 
        public void AddState(State state) {

            if (state == null)
                throw new ArgumentNullException("state");

            if (states.ContainsKey(state.FullName))
                throw new InvalidOperationException(
                    String.Format("El estado '{0}' ya ha sido agregado.", state.FullName));

            states.Add(state.FullName, state);
        }

        /// <summary>
        /// Afegeix un event a la maquina.
        /// </summary>
        /// <param name="ev">L'estat a afeigir.</param>
        /// 
        public void AddEvent(Event ev) {

            if (ev == null)
                throw new ArgumentNullException("ev");

            if (events.ContainsKey(ev.Name))
                throw new InvalidOperationException(
                    String.Format("El evento '{0}' ya ha sido agregado.", ev.Name));

            events.Add(ev.Name, ev);
        }

        public State GetState(string name, bool throwError = true) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("fullName");

            if (!states.ContainsKey(name)) {
                if (throwError)
                    throw new InvalidOperationException(
                        String.Format("No se agrego ningun estado con el nombre '{0}'.", name));
                else
                    return null;
            }

            return states[name];
        }

        public Event GetEvent(string name, bool throwError = true) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (!events.ContainsKey(name)) {
                if (throwError)
                    throw new InvalidOperationException(
                        String.Format("No se agrego ningun evento con el nombre '{0}'.", name));
                else
                    return null;
            }

            return events[name];
        }

        public string Name {
            get { 
                return name; 
            }
        }

        public State Start {
            get {
                return start;
            }
            set {
                start = value;
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
