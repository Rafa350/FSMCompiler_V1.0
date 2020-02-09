namespace MicroCompiler.CodeModel.Statements {

    using System;

    public sealed class AssignStatement : StatementBase {

        private readonly string name;
        private readonly ExpressionBase expression;

        public AssignStatement(string name, ExpressionBase expression) {

            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentNullException(nameof(name));
            }

            if (expression == null) {
                throw new ArgumentNullException(nameof(expression));
            }

            this.name = name;
            this.expression = expression;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Name => name;

        public ExpressionBase Expression => expression;
    }
}
