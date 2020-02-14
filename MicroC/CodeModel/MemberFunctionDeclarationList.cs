namespace MicroCompiler.CodeModel {

    using System.Collections.Generic;

    public sealed class MemberFunctionDeclarationList : List<MemberFunctionDeclaration> {

        public MemberFunctionDeclarationList() {
        }

        public MemberFunctionDeclarationList(IEnumerable<MemberFunctionDeclaration> members) :
            base(members) {
        }

        public MemberFunctionDeclarationList(params MemberFunctionDeclaration[] members) :
            base(members) {
        }
    }
}
