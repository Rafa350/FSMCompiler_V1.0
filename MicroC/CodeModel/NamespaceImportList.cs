namespace MicroCompiler.CodeModel {

    using System.Collections.Generic;

    public sealed class NamespaceImportList : List<NamespaceImport> {

        public NamespaceImportList() {
        }

        public NamespaceImportList(IEnumerable<NamespaceImport> imports) :
            base(imports) {
        }

        public NamespaceImportList(params NamespaceImport[] imports) :
            base(imports) {
        }
    }
}
