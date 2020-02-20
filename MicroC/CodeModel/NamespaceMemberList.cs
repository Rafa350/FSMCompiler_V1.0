namespace MicroCompiler.CodeModel {

    using System.Collections.Generic;

    public sealed class NamespaceMemberList : List<INamespaceMember> {

        public NamespaceMemberList() {
        }

        public NamespaceMemberList(IEnumerable<INamespaceMember> members) :
            base(members) {
        }

        public NamespaceMemberList(params INamespaceMember[] members) :
            base(members) {
        }
    }
}
