namespace MicroCompiler.CodeModel.Statements {

    using System;
    using MicroCompiler.CodeModel;

    public sealed class ReturnStatement : Statement {

        private readonly Expression expression;

        public ReturnStatement() {
        }

        public ReturnStatement(Expression expression) {

            this.expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public Expression Expression => expression;
    }
}
