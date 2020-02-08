namespace MicroCompiler.CodeModel.Expressions {

    using System;

    public enum UnaryOpCode {
        Minus,
        Not,
        PostInc,
        PostDec,
        PreInc,
        PreDec
    }

    /// <summary>
    /// Clase que represemta una operacio unitaria.
    /// </summary>
    /// 
    public class UnaryExpression : ExpressionBase {

        private readonly UnaryOpCode opCode;
        private readonly ExpressionBase expression;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="opCode">El codi d'operacio.</param>
        /// <param name="expression">Expressio a la que s'aplica l'operacio.</param>
        /// 
        public UnaryExpression(UnaryOpCode opCode, ExpressionBase expression) {

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            this.opCode = opCode;
            this.expression = expression;
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
        public UnaryOpCode OpCode => opCode;

        /// <summary>
        /// Obte l'expressio.
        /// </summary>
        /// 
        public ExpressionBase Expression => expression;
    }
}
