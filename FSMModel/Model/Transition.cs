namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;

    public enum TransitionMode {
        InternalLoop,
        ExternalLoop,
        Jump,
        Push,
        Pop
    }

    public sealed class Transition : IVisitable {

        private readonly Event transitionEvent;
        private readonly Guard guard;
        private Action action;
        private State nextState;
        private TransitionMode mode = TransitionMode.InternalLoop;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transitionEvent">L'event que activa la transicio.</param>
        /// <param name="guard">La guarda que autoritza la transicio.</param>
        /// 
        public Transition(Event transitionEvent, Guard guard) {

            this.transitionEvent = transitionEvent ?? throw new ArgumentNullException(nameof(transitionEvent));
            this.guard = guard;
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
        /// Obte l'event que activa aquesta transicio.
        /// </summary>
        /// 
        public Event TransitionEvent => transitionEvent;

        /// <summary>
        /// Obte la guarda que autoritza aquesta transicio.
        /// </summary>
        /// 
        public Guard Guard => guard;

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
