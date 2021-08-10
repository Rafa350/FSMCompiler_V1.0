using System;

namespace MikroPicDesigns.FSMCompiler.v1.Model {

    public enum TransitionMode {
        InternalLoop,
        ExternalLoop,
        Jump,
        Push,
        Pop
    }

    public sealed class Transition : IVisitable {

        private readonly Event _transitionEvent;
        private readonly Guard _guard;
        private Action _action;
        private State _nextState;
        private TransitionMode _mode = TransitionMode.InternalLoop;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transitionEvent">L'event que activa la transicio.</param>
        /// <param name="guard">La guarda que autoritza la transicio.</param>
        /// 
        public Transition(Event transitionEvent, Guard guard) {

            _transitionEvent = transitionEvent ?? throw new ArgumentNullException(nameof(transitionEvent));
            _guard = guard;
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
        public Event TransitionEvent => 
            _transitionEvent;

        /// <summary>
        /// Obte la guarda que autoritza aquesta transicio.
        /// </summary>
        /// 
        public Guard Guard => 
            _guard;

        /// <summary>
        /// Obte o asigna l'accio.
        /// </summary>
        /// 
        public Action Action {
            get => _action;
            set => _action = value;
        }

        /// <summary>
        /// Obte o asigna el estat final de la transicio.
        /// </summary>
        /// 
        public State NextState {
            get => _nextState;
            set => _nextState = value;
        }

        public TransitionMode Mode {
            get => _mode;
            set => _mode = value;
        }
    }
}
