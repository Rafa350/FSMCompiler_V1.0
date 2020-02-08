namespace MicroCompiler.CodeModel.Expressions {

    using System;

    /// <summary>
    /// Clase que representa un identificador.
    /// </summary>
    /// 
    public sealed class IdentifierExpression : ExpressionBase {

        private readonly string name;

        /// <summary>
        /// Construictor.
        /// </summary>
        /// <param name="name">El identificador.</param>
        /// 
        public IdentifierExpression(string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
        }

        /// <summary>
        /// Accepta un visitadir.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte el nom del identificador.
        /// </summary>
        /// 
        public string Name => name;
    }
}
