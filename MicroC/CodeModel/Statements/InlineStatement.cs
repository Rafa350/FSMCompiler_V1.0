namespace MicroCompiler.CodeModel.Statements {

    using System;

    public sealed class InlineStatement : StatementBase {

        private readonly string code;

        public InlineStatement(string code) {

            if (String.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            this.code = code;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public string Code => code;
    }
}
