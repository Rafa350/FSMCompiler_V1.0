namespace MicroCompiler.CodeModel {

    using System.Collections.Generic;

    public sealed class DeclarationBlockMemberList : List<IDeclarationBlockMember> {

        public DeclarationBlockMemberList() {
        }

        public DeclarationBlockMemberList(IEnumerable<IDeclarationBlockMember> members) :
            base(members) {
        }

        public DeclarationBlockMemberList(params IDeclarationBlockMember[] members) :
            base(members) {
        }
    }
}
