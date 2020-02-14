namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;

    public sealed class Machine : IVisitable {

        private List<State> stateList;
        private readonly string name;
        private State start;
        private Action initializeAction;
        private Action terminateAction;

        /// <summary>
        /// Constructior.
        /// </summary>
        /// <param name="name">El nom de la maquina.</param>
        /// 
        public Machine(string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Afegeix un estat a la maquina.
        /// </summary>
        /// <param name="state">L'estat a afeigir.</param>
        /// 
        public void AddState(State state) {

            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (stateList == null)
                stateList = new List<State>();

            if (stateList.Contains(state))
                throw new InvalidOperationException(
                    String.Format("El estado '{0}' ya ha sido agregado.", state.FullName));

            stateList.Add(state);
        }

        /// <summary>
        /// Afegeix divesos estats a la maquina.
        /// </summary>
        /// <param name="states">Els estats a afeigir.</param>
        /// 
        public void AddStates(IEnumerable<State> states) {

            if (states == null)
                throw new ArgumentNullException(nameof(states));

            foreach (var state in states)
                AddState(state);
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
                throw new ArgumentNullException(nameof(name));

            if (stateList != null)
                foreach (State state in stateList)
                    if (state.Name == name)
                        return state;

            if (throwError)
                throw new InvalidOperationException(
                    String.Format("No se agrego ningun estado con el nombre '{0}'.", name));

            return null;
        }

        /// <summary>
        /// Obte el nom de la maquina.
        /// </summary>
        /// 
        public string Name => name;

        /// <summary>
        /// Obte l'estat inicial de la maquina.
        /// </summary>
        /// 
        public State Start {
            get {
                return start;
            }
            set {
                if ((stateList == null) || !stateList.Contains(value)) {
                    throw new InvalidOperationException(
                        String.Format("El estado '{0}', no esta declarado en esta maquina.", value.Name));
                }

                start = value;
            }
        }

        public Action InitializeAction {
            get {
                return initializeAction;
            }
            set {
                initializeAction = value;
            }
        }

        public Action TerminateAction {
            get {
                return terminateAction;
            }
            set {
                terminateAction = value;
            }
        }

        /// <summary>
        /// Enumera els noms dels estats de la maquina.
        /// </summary>
        /// 
        public IEnumerable<string> StateNames {
            get {
                foreach (State state in stateList) {
                    yield return state.Name;
                }
            }
        }

        /// <summary>
        /// Enumera els estats de la maquina.
        /// </summary>
        /// 
        public IEnumerable<State> States => stateList;

        public IEnumerable<State> FinalStates {
            get {
                foreach (State state in stateList) {
                    if (!state.HasChilds) {
                        yield return state;
                    }
                }
            }
        }
    }
}
