namespace MicroCompiler.CodeModel {

    public sealed class FunctionDeclaration : FunctionDeclarationBase, IUnitMember {

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }
    }
}
