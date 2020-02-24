namespace MicroCompiler.CodeModel.Statements {

    using System;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;

    public sealed class CaseStatement : IVisitable {

        private readonly LiteralExpression expression;
        private readonly Statement stmt;

        public CaseStatement() {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="stmt"></param>
        /// 
        public CaseStatement(LiteralExpression expression, Statement stmt) {

            this.expression = expression ?? throw new ArgumentNullException(nameof(expression));
            this.stmt = stmt ?? throw new ArgumentNullException(nameof(stmt));
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte o asigna l'expressio.
        /// </summary>
        /// 
        public LiteralExpression Expression => expression;

        /// <summary>
        /// Obte o asigna el cos.
        /// </summary>
        /// 
        public Statement Stmt => stmt;
    }
}
