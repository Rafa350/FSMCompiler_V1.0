namespace MicroCompiler.CodeModel {

    using System.Collections.Generic;

    public sealed class MemberVariableDeclarationList : List<MemberVariableDeclaration> {

        public MemberVariableDeclarationList() {
        }

        public MemberVariableDeclarationList(IEnumerable<MemberVariableDeclaration> variables) :
            base(variables) {
        }

        public MemberVariableDeclarationList(params MemberVariableDeclaration[] variables) :
            base(variables) {
        }
    }
}
