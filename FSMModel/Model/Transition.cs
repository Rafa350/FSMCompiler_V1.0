namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;

    public enum TransitionMode {
        Null,
        JumpToState,
        CallToState,
        ReturnFromState
    }

    public sealed class Transition: IVisitable {

        private Event ev;
        private Guard guard;
        private Action action;
        private State nextState;
        private TransitionMode mode = TransitionMode.Null;

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte o asigna l'event associat a aquesta transicio.
        /// </summary>
        /// 
        public Event Event {
            get {
                return ev;
            }
            set {
                ev = value;
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
