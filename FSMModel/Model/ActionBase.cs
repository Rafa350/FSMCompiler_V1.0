namespace MikroPicDesigns.FSMCompiler.v1.Model {
    
    public abstract class ActionBase: IVisitable {

        private string condition;

        public abstract void AcceptVisitor(IVisitor visitor);

        public string Condition {
            get {
                return condition;
            }
            set {
                condition = value;
            }
        }
    }
}
