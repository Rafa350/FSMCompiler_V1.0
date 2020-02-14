namespace MicroCompiler.CodeModel {

    using System.Collections.Generic;

    public sealed class ConstructorDeclarationList : List<ConstructorDeclaration> {

        public ConstructorDeclarationList() {
        }

        public ConstructorDeclarationList(IEnumerable<ConstructorDeclaration> declarations) :
            base(declarations) {
        }

        public ConstructorDeclarationList(params ConstructorDeclaration[] declarations) :
            base(declarations) {
        }
    }
}
