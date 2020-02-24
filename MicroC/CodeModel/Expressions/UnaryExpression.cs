namespace MicroCompiler.CodeModel.Expressions {

    using System;

    public enum UnaryOperation {
        Minus,
        Not,
        PostInc,
        PostDec,
        PreInc,
        PreDec
    }

    /// <summary>
    /// Clase que representa una operacio unitaria.
    /// </summary>
    /// 
    public class UnaryExpression : Expression {

        private readonly UnaryOperation operation;
        private readonly Expression expression;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="operation">El codi d'operacio.</param>
        /// <param name="expression">Expressio a la que s'aplica l'operacio.</param>
        /// 
        public UnaryExpression(UnaryOperation operation, Expression expression) {

            this.operation = operation;
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
        /// Obte el codi d'operacio.
        /// </summary>
        /// 
        public UnaryOperation Operation => operation;

        /// <summary>
        /// Obte l'expressio.
        /// </summary>
        /// 
        public Expression Expression => expression;
    }
}
