using System;

namespace MicroCompiler.CodeModel.Expressions {

    /// <summary>
    /// Clase que representa un valor literal.
    /// </summary>
    public sealed class LiteralExpression : Expression {

        private readonly object _value;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">El valor literal.</param>
        /// 
        public LiteralExpression(object value) {

            _value = value;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor"></param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte el valor del literal.
        /// </summary>
        /// 
        public object Value => 
            _value;
    }
}
