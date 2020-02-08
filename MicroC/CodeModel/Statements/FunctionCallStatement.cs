namespace MicroCompiler.CodeModel.Statements {

    using System;
    using MicroCompiler.CodeModel.Expressions;

    public sealed class FunctionCallStatement : StatementBase {

        private readonly FunctionCallExpression expression;

        public FunctionCallStatement(FunctionCallExpression expression) {

            if (expression == null)
                throw new ArgumentNullException("expression");

            this.expression = expression;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte o asigna la expressio.
        /// </summary>
        /// 
        public FunctionCallExpression Expression {
            get {
                return expression;
            }
        }
    }
}
