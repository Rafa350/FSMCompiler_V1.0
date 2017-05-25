namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;

    public sealed class TransitionList: IVisitable {

        private List<Transition> transitions;

        public void Add(Transition transition) {

            if (transition == null)
                throw new ArgumentNullException("transition");

            if (transitions == null)
                transitions = new List<Transition>();

            transitions.Add(transition);
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public bool HasTransitions {
            get {
                return transitions != null;
            }
        }

        public IEnumerable<Transition> Transitions {
            get {
                return transitions;
            }
        }
    }
}
