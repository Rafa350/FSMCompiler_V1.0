namespace MicroCompiler.CodeModel.Statements {

    using System;

    public sealed class AssignStatement : Statement {

        private readonly string name;
        private readonly Expression expression;

        public AssignStatement(string name, Expression expression) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            this.expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Name => name;

        public Expression Expression => expression;
    }
}
