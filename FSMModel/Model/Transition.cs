namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;

    public enum TransitionMode {
        Null,
        JumpToState,
        CallToState,
        ReturnFromState
    }

    public sealed class Transition: IVisitable {

        private readonly string name;
        private Guard guard;
        private Action action;
        private State nextState;
        private TransitionMode mode = TransitionMode.Null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom de la transicio.</param>
        /// 
        public Transition(string name) {

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
        /// Obte el nom de la transicio.
        /// </summary>
        /// 
        public string Name {
            get {
                return name;
            }
        }

        /// <summary>
        /// Obte o asigna la guarda d'aquesta transicio.
        /// </summary>
        /// 
        public Guard Guard {
            get {
                return guard;
            }
            set {
                guard = value;
            }
        }

        /// <summary>
        /// Obte o asigna l'accio.
        /// </summary>
        /// 
        public Action Action {
            get {
                return action;
            }
            set {
                action = value;
            }
        }

        /// <summary>
        /// Obte o asigna el estat final de la transicio.
        /// </summary>
        /// 
        public State NextState {
            get {
                return nextState;
            }
            set {
                nextState = value;
            }
        }

        public TransitionMode Mode {
            get {
                return mode;
            }
            set {
                mode = value;
            }
        }
    }
}
