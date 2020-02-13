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
        /// <param name="address">Adressa de la funcio.</param>
        /// 
        public FunctionCallExpression(ExpressionBase address) {

            if (address == null)
                throw new ArgumentNullException(nameof(address));

            this.address = address;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Adressa de la funcio.</param>
        /// <param name="arguments">Llista d'arguments.</param>
        /// 
        public FunctionCallExpression(ExpressionBase address, List<ExpressionBase> arguments) {

            if (address == null)
                throw new ArgumentNullException(nameof(address));

            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            this.address = address;
            argumentList = arguments;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Adressa funcio.</param>
        /// <param name="arguments">Llista d'arguments de la funcio</param>
        /// 
        public FunctionCallExpression(ExpressionBase address, IEnumerable<ExpressionBase> arguments) :
            this(address, new List<ExpressionBase>(arguments)) { 

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Adressa de la funcio.</param>
        /// <param name="arguments">Llista d'arguments de la funcio.</param>
        /// 
        public FunctionCallExpression(ExpressionBase address, params ExpressionBase[] arguments) :
            this(address, new List<ExpressionBase>(arguments)) {
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
        public ExpressionBase Function => address;

        /// <summary>
        /// Indica si hi han arguments.
        /// </summary>
        /// 
        public bool HasArguments => argumentList != null;

        /// <summary>
        /// Enumera els d'arguments.
        /// </summary>
        /// 
        public IEnumerable<ExpressionBase> Arguments => argumentList;
    }
}
