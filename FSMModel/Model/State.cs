using System;
using System.Collections.Generic;

namespace MikroPicDesigns.FSMCompiler.v1.Model {

    public sealed class State : IVisitable {

        private readonly List<State> _childs = new List<State>();
        private List<Transition> _transitionList;
        private readonly State _parent;
        private readonly string _name;
        private Action _enterAction;
        private Action _exitAction;

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

            _parent = parent;
            _name = name;

            if (parent != null)
                parent._childs.Add(this);
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

            if (_transitionList == null)
                _transitionList = new List<Transition>();

            _transitionList.Add(transition);
        }

        /// <summary>
        /// Obte l'estat pare
        /// </summary>
        /// 
        public State Parent => 
            _parent;

        /// <summary>
        /// Obte el nom.
        /// </summary>
        /// 
        public string Name => 
            _name;

        /// <summary>
        /// Obte el nom complert, seguint la ruta pare-fill.
        /// </summary>
        /// 
        public string FullName => 
            (_parent == null) ? _name : String.Format("{0}{1}", _parent.FullName, _name);

        /// <summary>
        /// Indica si hi han transisions.
        /// </summary>
        /// 
        public bool HasTransitions => 
            _transitionList != null;

        /// <summary>
        /// Obte un enumerador per les transicions.
        /// </summary>
        /// 
        public IEnumerable<Transition> Transitions =>
            _transitionList;

        /// <summary>
        /// Obte o asigna l'accio d'entrada.
        /// </summary>
        /// 
        public Action EnterAction {
            get => _enterAction;
            set => _enterAction = value;
        }

        /// <summary>
        /// Obte o asigna l'accio de sortida.
        /// </summary>
        /// 
        public Action ExitAction {
            get => _exitAction;
            set => _exitAction = value;
        }

        /// <summary>
        /// Obte un enumerador dels fills.
        /// </summary>
        public IEnumerable<State> Childs =>
            _childs;

        /// <summary>
        /// Indica si conte fills.
        /// </summary>
        /// 
        public bool HasChilds =>
            _childs.Count > 0;
    }
}