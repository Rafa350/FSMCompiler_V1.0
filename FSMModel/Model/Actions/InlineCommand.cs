namespace MikroPicDesigns.FSMCompiler.v1.Model.Actions {
    
    public sealed class InlineCommand: Command {

        private string text;

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Text {
            get {
                return text;
            }
            set {
                text = value;
            }
        }
    }
}
