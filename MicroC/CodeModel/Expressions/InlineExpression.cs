namespace MicroCompiler.CodeModel.Expressions {

    using System;

    /// <summary>
    /// Clase que representa codi inline.
    /// </summary>
    /// 
    public sealed class InlineExpression : ExpressionBase {

        private readonly string code;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="code">El codi.</param>
        /// 
        public InlineExpression(string code) {

            this.code = code;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null) {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte el codi.
        /// </summary>
        /// 
        public string Code => code;
    }
}
