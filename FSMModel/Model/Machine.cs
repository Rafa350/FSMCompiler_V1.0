namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;

    public sealed class Machine: IVisitable {

        private readonly List<State> states = new List<State>();
        private readonly List<Event> events = new List<Event>();
        private readonly string name;
        private State start;

        public Machine(string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.name = name;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
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

            if (states.Contains(state))
                throw new InvalidOperationException(
                    String.Format("El estado '{0}' ya ha sido agregado.", state.FullName));

            states.Add(state);
        }

        /// <summary>
        /// Afegeix un event a la maquina.
        /// </summary>
        /// <param name="ev">L'estat a afeigir.</param>
        /// 
        public void AddEvent(Event ev) {

            if (ev == null)
                throw new ArgumentNullException("ev");

            if (events.Contains(ev))
                throw new InvalidOperationException(
                    String.Format("El evento '{0}' ya ha sido agregado.", ev.Name));

            events.Add(ev);
        }

        /// <summary>
        /// Obte un estat afeigit previament a la maquina.
        /// </summary>
        /// <param name="name">El nom del estat.</param>
        /// <param name="throwError">True si cal generar una excepcio en cas d'error.</param>
        /// <returns>L'estat.</returns>
        /// 
        public State GetState(string name, bool throwError = true) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            foreach (State state in states) {
                if (state.Name == name)
                    return state;
            }

            if (throwError)
                throw new InvalidOperationException(
                    String.Format("No se agrego ningun estado con el nombre '{0}'.", name));

            return null;
        }

        /// <summary>
        /// Obte un event afeigit previament a la maquina.
        /// </summary>
        /// <param name="name">Nom de l'event.</param>
        /// <param name="throwError">True si cal generar una excepcio en cas d'error.</param>
        /// <returns>L'event.</returns>
        /// 
        public Event GetEvent(string name, bool throwError = true) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            foreach (Event ev in events)
                if (ev.Name == name)
                    return ev;

            if (throwError)
                throw new InvalidOperationException(
                    String.Format("No se agrego ningun evento con el nombre '{0}'.", name));

            return null;
        }

        /// <summary>
        /// Obte el nom de la maquina.
        /// </summary>
        /// 
        public string Name {
            get { 
                return name; 
            }
        }

        /// <summary>
        /// Obte l'estat inicial de la maquina.
        /// </summary>
        /// 
        public State Start {
            get {
                return start;
            }
            set {
                if (!states.Contains(value))
                    throw new InvalidOperationException(
                        String.Format("El estado '{0}', no esta declarado en esta maquina.", value.Name));
                start = value;
            }
        }

        /// <summary>
        /// Enumera els noms dels estats de la maquina.
        /// </summary>
        /// 
        public IEnumerable<string> StateNames {
            get {
                foreach (State state in states)
                    yield return state.Name;
            }
        }

        /// <summary>
        /// Enumera els noms dels events de la maquina.
        /// </summary>
        /// 
        public IEnumerable<string> EventNames {
            get {
                foreach (Event ev in events)
                    yield return ev.Name;
            }
        }

        /// <summary>
        /// Enumera els estats de la maquina.
        /// </summary>
        /// 
        public IEnumerable<State> States {
            get {
                return states;
            }
        }

        public IEnumerable<State> FinalStates {
            get {
                foreach (State state in states)
                    if (!state.HasChilds)
                        yield return state;
            }
        }

        /// <summary>
        /// Enumera els events de la maquina.
        /// </summary>
        /// 
        public IEnumerable<Event> Events {
            get {
                return events;
            }
        }
    }
}
