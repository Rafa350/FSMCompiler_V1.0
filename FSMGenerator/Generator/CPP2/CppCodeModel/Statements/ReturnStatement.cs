namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Statements {

    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel;

    public sealed class ReturnStatement: StatementBase {

        private readonly ExpressionBase expression;

        public ReturnStatement() {

            this.expression = null;
        }

        public ReturnStatement(ExpressionBase expression) {

            this.expression = expression;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public override string ToString() {

            return "return";
        }

        public ExpressionBase Expression {
            get {
                return expression;
            }
        }
    }
}
