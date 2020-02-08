namespace MicroCompiler.CodeModel.Expressions {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Clase que representa una crida a una funcio.
    /// </summary>
    /// 
    public sealed class SubscriptExpression : ExpressionBase {

        private readonly ExpressionBase address;
        private readonly List<ExpressionBase> indexList;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Nom de la funcio a cridar.</param>
        /// <param name="indices">Llista d'arguments de la funcio</param>
        /// 
        public SubscriptExpression(ExpressionBase address, IEnumerable<ExpressionBase> indices = null) {

            if (address == null)
                throw new ArgumentNullException(nameof(address));

            this.address = address;
            if (indices != null)
                indexList = new List<ExpressionBase>(indices);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Nom de la cuncio a cridar.</param>
        /// <param name="indices">Llista d'arguments de la funcio.</param>
        /// 
        public SubscriptExpression(ExpressionBase address, params ExpressionBase[] indices) :
            this(address, (IEnumerable<ExpressionBase>)indices) {
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
        /// Obte el nom.
        /// </summary>
        /// 
        public ExpressionBase Address => address;

        /// <summary>
        /// Enumera els d'arguments.
        /// </summary>
        /// 
        public IEnumerable<ExpressionBase> Indices => indexList;
    }
}
