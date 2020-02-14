namespace MicroCompiler.CodeModel {

    using System.Collections.Generic;

    public sealed class StatementList : List<Statement> {

        public StatementList() {
        }

        public StatementList(IEnumerable<Statement> statements) :
            base(statements) {
        }

        public StatementList(params Statement[] statements) :
            base(statements) {
        }
    }
}
