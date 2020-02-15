namespace MicroCompiler.CodeModel {

    using System.Collections.Generic;

    public sealed class UnitMemberDeclarationList: List<IUnitMember> {

        public UnitMemberDeclarationList() {
        }

        public UnitMemberDeclarationList(IEnumerable<IUnitMember> members):
            base(members) {
        }

        public UnitMemberDeclarationList(params IUnitMember[] members) :
            base(members) {
        }
    }
}
