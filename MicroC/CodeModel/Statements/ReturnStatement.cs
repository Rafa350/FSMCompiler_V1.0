namespace MicroCompiler.CodeModel.Statements {

    using System;
    using MicroCompiler.CodeModel;

    public sealed class ReturnStatement : Statement {

        public ReturnStatement() {
        }

        public ReturnStatement(Expression expression) {

            this.Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public Expression Expression { get; set; }
    }
}
