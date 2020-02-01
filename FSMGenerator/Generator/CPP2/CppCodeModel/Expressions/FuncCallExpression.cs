namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Expressions {

    using System;
    using System.Collections.Generic;

    public sealed class FunctionCallExpression: ExpressionBase {

        private readonly string name;
        private readonly List<ExpressionBase> argumentList;

        public FunctionCallExpression(string name, IEnumerable<ExpressionBase> arguments = null) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            if (arguments != null)
                argumentList = new List<ExpressionBase>(arguments);
        }

        public FunctionCallExpression(string name, params ExpressionBase[] arguments):
            this(name, (IEnumerable<ExpressionBase>)arguments) {
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
        public string Name => name;

        /// <summary>
        /// Enumera els d'arguments.
        /// </summary>
        /// 
        public IEnumerable<ExpressionBase> Arguments => Arguments;
    }
}
