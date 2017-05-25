namespace MikroPicDesigns.FSMCompiler.v1.Model.Actions {

    using System;

    public sealed class GotoAction: ActionBase {

        public State next;

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public State Next {
            get {
                return next;
            }
            set {
                next = value;
            }
        }
    }
}
