namespace MicroCompiler.CodeModel.Statements {

    using System;
    using MicroCompiler.CodeModel.Expressions;

    public sealed class FunctionCallStatement : Statement {

        private readonly FunctionCallExpression expression;

        public FunctionCallStatement(FunctionCallExpression expression) {

            this.expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte o asigna la expressio.
        /// </summary>
        /// 
        public FunctionCallExpression Expression => expression;
    }
}
