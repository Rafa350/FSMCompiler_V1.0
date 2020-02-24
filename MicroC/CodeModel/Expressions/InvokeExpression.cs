namespace MicroCompiler.CodeModel.Expressions {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Clase que representa una crida a una funcio.
    /// </summary>
    /// 
    public sealed class InvokeExpression : Expression {

        private readonly Expression addressExp;
        private readonly List<Expression> arguments;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="addressEpr">Adressa de la funcio.</param>
        /// 
        public InvokeExpression(Expression addressEpr) {

            this.addressExp = addressEpr ?? throw new ArgumentNullException(nameof(addressEpr));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="addressEpr">Adressa de la funcio.</param>
        /// <param name="arguments">Llista d'arguments.</param>
        /// 
        public InvokeExpression(Expression addressEpr, List<Expression> arguments) {

            this.addressExp = addressEpr ?? throw new ArgumentNullException(nameof(addressEpr));
            this.arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="addressEpr">Adressa funcio.</param>
        /// <param name="arguments">Llista d'arguments de la funcio</param>
        /// 
        public InvokeExpression(Expression addressEpr, IEnumerable<Expression> arguments) :
            this(addressEpr, new List<Expression>(arguments)) {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="addressExp">Adressa de la funcio.</param>
        /// <param name="arguments">Llista d'arguments de la funcio.</param>
        /// 
        public InvokeExpression(Expression addressExp, params Expression[] arguments) :
            this(addressExp, new List<Expression>(arguments)) {
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
        public Expression AddressEpr => addressExp;

        /// <summary>
        /// Indica si hi han arguments.
        /// </summary>
        /// 
        public bool HasArguments => (arguments != null) && (arguments.Count > 0);

        /// <summary>
        /// Enumera els d'arguments.
        /// </summary>
        /// 
        public IEnumerable<Expression> Arguments => arguments;
    }
}
