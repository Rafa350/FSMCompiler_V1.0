namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Statements {

    public sealed class InlineStatement: StatementBase {

        private readonly string code;

        public InlineStatement(string code) {

            this.code = code;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Code => code;
    }
}
