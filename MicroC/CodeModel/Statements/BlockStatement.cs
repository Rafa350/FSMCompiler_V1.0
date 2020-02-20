namespace MicroCompiler.CodeModel.Statements {

    using System;

    /// <summary>
    /// Clase que representa un bloc d'instruccions.
    /// </summary>
    /// 
    public sealed class BlockStatement : Statement {

        private StatementList statements;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public BlockStatement() {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statements">Llista d'instruccions.</param>
        /// 
        public BlockStatement(StatementList statements) {

            this.statements = statements;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Indica si conte instruccions.
        /// </summary>
        /// 
        public bool HasStatements => statements != null;

        /// <summary>
        /// Obte o asigna la llista d'instruccions.
        /// </summary>
        /// 
        public StatementList Statements {
            get {
                if (statements == null)
                    statements = new StatementList();
                return statements;
            }
            set {
                statements = value;
            }
        }
    }
}
