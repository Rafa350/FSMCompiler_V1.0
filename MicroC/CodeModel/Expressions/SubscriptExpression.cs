namespace MicroCompiler.CodeModel.Expressions {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Clase que representa una crida a una funcio.
    /// </summary>
    /// 
    public sealed class SubscriptExpression : Expression {

        private readonly Expression address;
        private readonly List<Expression> indexList;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Expressio pel calcul de l'adressa.</param>
        /// <param name="indices">Llista de valors del s index.</param>
        /// 
        public SubscriptExpression(Expression address, List<Expression> indices) {

            this.address = address ?? throw new ArgumentNullException(nameof(address));
            indexList = indices ?? throw new ArgumentNullException(nameof(indices));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Expressio pel calcul de l'adressa.</param>
        /// <param name="indices">Llista de valors dels index.</param>
        /// 
        public SubscriptExpression(Expression address, IEnumerable<Expression> indices) :
            this(address, new List<Expression>(indices)) {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Expressio pel calcul de l'adressa..</param>
        /// <param name="indices">Llista de valors dels index.</param>
        /// 
        public SubscriptExpression(Expression address, params Expression[] indices) :
            this(address, new List<Expression>(indices)) {
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
        public Expression Address => address;

        /// <summary>
        /// Enumera els d'arguments.
        /// </summary>
        /// 
        public IEnumerable<Expression> Indices => indexList;
    }
}
