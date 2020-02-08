namespace MikroPicDesigns.FSMCompiler.v1.Model.Activities {

    public sealed class CallActivity : Activity {

        private string methodName;

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string MethodName {
            get {
                return methodName;
            }
            set {
                methodName = value;
            }
        }
    }
}
