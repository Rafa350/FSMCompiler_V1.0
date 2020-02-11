namespace MicroCompiler.CodeModel.Statements {

    using System;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;

    public sealed class SwitchCaseStatement : IVisitable {

        private LiteralExpression expression;
        private Block body;

        public SwitchCaseStatement() {

        }

        public SwitchCaseStatement(LiteralExpression expression, Block body) {

            this.expression = expression;
            this.body = body;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null) {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte o asigna l'expressio.
        /// </summary>
        /// 
        public LiteralExpression Expression {
            get { return expression; }
            set { expression = value; }
        }

        /// <summary>
        /// Obte o asigna el cos.
        /// </summary>
        /// 
        public Block Body {
            get { return body; }
            set { body = value; }
        }
    }
}
