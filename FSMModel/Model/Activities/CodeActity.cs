namespace MikroPicDesigns.FSMCompiler.v1.Model.Activities {
    
    public sealed class CodeActity: Activity {

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
