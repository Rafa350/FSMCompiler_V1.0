namespace MicroCompiler.CodeModel.Statements {

    using System;
    using MicroCompiler.CodeModel;

    public sealed class ReturnStatement : Statement {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        public ReturnStatement() {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="valueExp">Expressio pel valor de retorn.</param>
        /// 
        public ReturnStatement(Expression valueExp) {

            ValueExp = valueExp ?? throw new ArgumentNullException(nameof(valueExp));
        }

        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public Expression ValueExp { get; set; }
    }
}
