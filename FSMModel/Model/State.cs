namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;

    public sealed class State : IVisitable {

        private readonly List<State> childs = new List<State>();
        private readonly List<Transition> transitions = new List<Transition>();
        private readonly State parent;
        private readonly string name;
        private Action enterAction;
        private Action exitAction;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="name">Nom.</param>
        /// 
        public State(string name)
            : this(null, name) {
        }

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="parent">Estat pare.</param>
        /// <param name="name">Nom.</param>
        /// 
        public State(State parent, string name) {

            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            this.parent = parent;
            this.name = name;

            if (parent != null) {
                parent.childs.Add(this);
            }
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
        /// Afegeix una transicio.
        /// </summary>
        /// <param name="transition">La transisio a afeigir.</param>
        /// 
        public void AddTransition(Transition transition) {

            if (transition == null) {
                throw new ArgumentNullException("transition");
            }

            transitions.Add(transition);
        }

        /// <summary>
        /// Obte l'estat pare
        /// </summary>
        /// 
        public State Parent {
            get {
                return parent;
            }
        }

        /// <summary>
        /// Obte el nom.
        /// </summary>
        /// 
        public string Name {
            get {
                return name;
            }
        }

        /// <summary>
        /// Obte el nom complert, seguint la ruta pare-fill.
        /// </summary>
        /// 
        public string FullName {
            get {
                return (parent == null) ? name : String.Format("{0}{1}", parent.FullName, name);
            }
        }

        /// <summary>
        /// Obte un enumerador per les transicions.
        /// </summary>
        /// 
        public IEnumerable<Transition> Transitions {
            get {
                return transitions;
            }
        }

        /// <summary>
        /// Obte el nombre de transicions.
        /// </summary>
        /// 
        public int NumberOfTransitions {
            get {
                return transitions.Count;
            }
        }

        /// <summary>
        /// Indica si hi han transisions.
        /// </summary>
        /// 
        public bool HasTransitions {
            get {
                return transitions.Count > 0;
            }
        }

        /// <summary>
        /// Obte o asigna l'accio d'entrada.
        /// </summary>
        /// 
        public Action EnterAction {
            get {
                return enterAction;
            }
            set {
                enterAction = value;
            }
        }

        /// <summary>
        /// Obte o asigna l'accio de sortida.
        /// </summary>
        /// 
        public Action ExitAction {
            get {
                return exitAction;
            }
            set {
                exitAction = value;
            }
        }

        /// <summary>
        /// Obte un enumerador dels fills.
        /// </summary>
        public IEnumerable<State> Childs {
            get {
                return childs;
            }
        }

        /// <summary>
        /// Indica si conte fills.
        /// </summary>
        /// 
        public bool HasChilds {
            get {
                return childs.Count > 0;
            }
        }
    }
}