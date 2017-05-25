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
        private string condition;
        private ActionList actions;
        private State next;
        private TransitionMode mode = TransitionMode.Null;

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public Event Event {
            get {
                return ev;
            }
            set {
                ev = value;
            }
        }

        public string Condition {
            get {
                return condition;
            }
            set {
                condition = value;
            }
        }

        public ActionList Actions {
            get {
                return actions;
            }
            set {
                actions = value;
            }
        }

        public State Next {
            get {
                return next;
            }
            set {
                next = value;
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
