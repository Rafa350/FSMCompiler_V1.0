namespace MicroCompiler.CodeModel {

    using System.Collections.Generic;

    public sealed class NamespaceDeclarationList : List<NamespaceDeclaration> {

        public NamespaceDeclarationList() {
        }

        public NamespaceDeclarationList(IEnumerable<NamespaceDeclaration> namespaces) :
            base(namespaces) {
        }

        public NamespaceDeclarationList(params NamespaceDeclaration[] namespaces) :
            base(namespaces) {
        }
    }
}
