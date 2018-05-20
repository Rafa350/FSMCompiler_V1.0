namespace MikroPicDesigns.FSMCompiler.v1.Model.Actions {

    public sealed class RaiseCommand: Command {

        private Event ev;
        private string delayText;

        public override void AcceptVisitor(IVisitor visitor) {

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

        public string DelayText {
            get {
                return delayText;
            }
            set {
                delayText = value;
            }
        }
    }
}
