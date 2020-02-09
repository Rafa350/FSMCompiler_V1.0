namespace MicroCompiler.CodeModel {

    using System;

    public sealed class VariableDeclaration: VariableDeclarationBase, IUnitMember {

        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }
    }
}
