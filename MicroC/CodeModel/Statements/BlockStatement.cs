namespace MicroCompiler.CodeModel.Statements {

    using System;

    /// <summary>
    /// Clase que representa un bloc d'instruccions.
    /// </summary>
    /// 
    public sealed class BlockStatement : Statement {

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

            Statements = statements;
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
        /// Obte o asigna la llista d'instruccions.
        /// </summary>
        /// 
        public StatementList Statements { get; set; }
    }
}
