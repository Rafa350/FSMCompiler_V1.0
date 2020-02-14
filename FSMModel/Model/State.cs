namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;

    public sealed class State : IVisitable {

        private readonly List<State> childs = new List<State>();
        private List<Transition> transitionList;
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

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.parent = parent;
            this.name = name;

            if (parent != null)
                parent.childs.Add(this);
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
        /// Afegeix una transicio.
        /// </summary>
        /// <param name="transition">La transisio a afeigir.</param>
        /// 
        public void AddTransition(Transition transition) {

            if (transition == null)
                throw new ArgumentNullException(nameof(transition));

            if (transitionList == null)
                transitionList = new List<Transition>();

            transitionList.Add(transition);
        }

        /// <summary>
        /// Obte l'estat pare
        /// </summary>
        /// 
        public State Parent => parent;

        /// <summary>
        /// Obte el nom.
        /// </summary>
        /// 
        public string Name => name;

        /// <summary>
        /// Obte el nom complert, seguint la ruta pare-fill.
        /// </summary>
        /// 
        public string FullName => (parent == null) ? name : String.Format("{0}{1}", parent.FullName, name);

        /// <summary>
        /// Indica si hi han transisions.
        /// </summary>
        /// 
        public bool HasTransitions => transitionList != null;

        /// <summary>
        /// Obte un enumerador per les transicions.
        /// </summary>
        /// 
        public IEnumerable<Transition> Transitions {
            get {
                return transitionList;
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