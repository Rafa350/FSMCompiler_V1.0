namespace MicroCompiler.CodeModel.Expressions {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Clase que representa una crida a una funcio.
    /// </summary>
    /// 
    public sealed class FunctionCallExpression : Expression {

        private readonly Expression address;
        private readonly List<Expression> argumentList;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Adressa de la funcio.</param>
        /// 
        public FunctionCallExpression(Expression address) {

            this.address = address ?? throw new ArgumentNullException(nameof(address));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Adressa de la funcio.</param>
        /// <param name="arguments">Llista d'arguments.</param>
        /// 
        public FunctionCallExpression(Expression address, List<Expression> arguments) {

            this.address = address ?? throw new ArgumentNullException(nameof(address));
            argumentList = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Adressa funcio.</param>
        /// <param name="arguments">Llista d'arguments de la funcio</param>
        /// 
        public FunctionCallExpression(Expression address, IEnumerable<Expression> arguments) :
            this(address, new List<Expression>(arguments)) {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Adressa de la funcio.</param>
        /// <param name="arguments">Llista d'arguments de la funcio.</param>
        /// 
        public FunctionCallExpression(Expression address, params Expression[] arguments) :
            this(address, new List<Expression>(arguments)) {
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
        public Expression Function => address;

        /// <summary>
        /// Indica si hi han arguments.
        /// </summary>
        /// 
        public bool HasArguments => argumentList != null;

        /// <summary>
        /// Enumera els d'arguments.
        /// </summary>
        /// 
        public IEnumerable<Expression> Arguments => argumentList;
    }
}
