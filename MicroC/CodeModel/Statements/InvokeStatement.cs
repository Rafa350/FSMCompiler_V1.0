namespace MicroCompiler.CodeModel.Statements {

    using System;
    using MicroCompiler.CodeModel.Expressions;

    public sealed class InvokeStatement : Statement {

        public InvokeStatement() {
        }

        public InvokeStatement(InvokeExpression invokeExp) {

            InvokeExp = invokeExp ?? throw new ArgumentNullException(nameof(invokeExp));
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
        /// Obte o asigna la expressio.
        /// </summary>
        /// 
        public InvokeExpression InvokeExp { get; set; }
    }
}
