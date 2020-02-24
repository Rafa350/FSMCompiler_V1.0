namespace MicroCompiler.CodeModel.Expressions {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Clase que representa una indexacio.
    /// </summary>
    /// 
    public sealed class SubscriptExpression : Expression {

        private readonly Expression addressExp;
        private readonly List<Expression> indices;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="addressEpr">Expressio pel calcul de l'adressa.</param>
        /// <param name="indices">Llista de valors dels index.</param>
        /// 
        public SubscriptExpression(Expression addressEpr, List<Expression> indices) {

            this.addressExp = addressEpr ?? throw new ArgumentNullException(nameof(addressEpr));
            this.indices = indices ?? throw new ArgumentNullException(nameof(indices));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="addressExp">Expressio pel calcul de l'adressa.</param>
        /// <param name="indices">Llista de valors dels index.</param>
        /// 
        public SubscriptExpression(Expression addressExp, IEnumerable<Expression> indices) :
            this(addressExp, new List<Expression>(indices)) {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Exp">Expressio pel calcul de l'adressa..</param>
        /// <param name="indices">Llista de valors dels index.</param>
        /// 
        public SubscriptExpression(Expression Exp, params Expression[] indices) :
            this(Exp, new List<Expression>(indices)) {
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
        public Expression AddressExp => addressExp;

        /// <summary>
        /// Indica si conte index.
        /// </summary>
        /// 
        public bool HasIndices => (indices != null) && (indices.Count > 0);

        /// <summary>
        /// Enumera els d'arguments.
        /// </summary>
        /// 
        public IEnumerable<Expression> Indices => indices;
    }
}
