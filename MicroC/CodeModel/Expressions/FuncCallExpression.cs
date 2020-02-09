namespace MicroCompiler.CodeModel.Expressions {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Clase que representa una crida a una funcio.
    /// </summary>
    /// 
    public sealed class FunctionCallExpression : ExpressionBase {

        private readonly ExpressionBase address;
        private readonly List<ExpressionBase> argumentList;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Nom de la funcio a cridar.</param>
        /// <param name="arguments">Llista d'arguments de la funcio</param>
        /// 
        public FunctionCallExpression(ExpressionBase address, IEnumerable<ExpressionBase> arguments = null) {

            if (address == null) {
                throw new ArgumentNullException(nameof(address));
            }

            this.address = address;
            if (arguments != null) {
                argumentList = new List<ExpressionBase>(arguments);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Nom de la cuncio a cridar.</param>
        /// <param name="arguments">Llista d'arguments de la funcio.</param>
        /// 
        public FunctionCallExpression(ExpressionBase address, params ExpressionBase[] arguments) :
            this(address, (IEnumerable<ExpressionBase>)arguments) {
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
        /// Obte el nom.
        /// </summary>
        /// 
        public ExpressionBase Function => address;

        /// <summary>
        /// Enumera els d'arguments.
        /// </summary>
        /// 
        public IEnumerable<ExpressionBase> Arguments => argumentList;
    }
}
