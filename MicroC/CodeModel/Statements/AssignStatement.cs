namespace MicroCompiler.CodeModel.Statements {

    using System;

    public sealed class AssignStatement : StatementBase {

        private readonly string name;
        private readonly ExpressionBase expression;

        public AssignStatement(string name, ExpressionBase expression) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (expression == null)
                throw new ArgumentNullException("expression");

            this.name = name;
            this.expression = expression;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public override string ToString() {

            return String.Format("{0} = ", name);
        }

        public string Name {
            get {
                return name;
            }
        }

        public ExpressionBase Expression {
            get {
                return expression;
            }
        }
    }
}
