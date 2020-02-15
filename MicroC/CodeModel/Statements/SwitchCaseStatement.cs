namespace MicroCompiler.CodeModel.Statements {

    using System;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;

    public sealed class SwitchCaseStatement : IVisitable {

        private readonly LiteralExpression expression;
        private readonly BlockStatement body;

        /// <summary>
        /// Constructor (El cas de 'default'
        /// </summary>
        /// <param name="body"></param>
        /// 
        public SwitchCaseStatement(BlockStatement body) {

            this.body = body ?? throw new ArgumentNullException(nameof(body));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="body"></param>
        /// 
        public SwitchCaseStatement(LiteralExpression expression, BlockStatement body) {

            this.expression = expression ?? throw new ArgumentNullException(nameof(expression));
            this.body = body ?? throw new ArgumentNullException(nameof(body));
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte o asigna l'expressio.
        /// </summary>
        /// 
        public LiteralExpression Expression => expression;

        /// <summary>
        /// Obte o asigna el cos.
        /// </summary>
        /// 
        public BlockStatement Body => body;
    }
}
