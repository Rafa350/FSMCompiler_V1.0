namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Expressions {

    public sealed class LiteralExpression: ExpressionBase {

        private readonly object value;

        public LiteralExpression(object value) {

            this.value = value;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public object Value {
            get {
                return value;
            }
        }
    }
}
