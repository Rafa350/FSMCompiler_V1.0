namespace MicroCompiler.CodeModel {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Clase que representa un bloc de codi.
    /// </summary>
    /// 
    public sealed class Block : IVisitable {

        private List<StatementBase> statementList;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public Block() {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statements">Llista de estaments.</param>
        /// 
        public Block(IEnumerable<StatementBase> statements) {

            if (statements == null) {
                throw new ArgumentNullException(nameof(statements));
            }

            statementList = new List<StatementBase>(statements);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statements">Llista de estaments.</param>
        /// 
        public Block(params StatementBase[] statements) {

            if (statements == null) {
                throw new ArgumentNullException(nameof(statements));
            }

            statementList = new List<StatementBase>(statements);
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null) {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }

        /// <summary>
        /// Afegeig una ordre.
        /// </summary>
        /// <param name="statement">La ordre.</param>
        /// 
        public void AddStatement(StatementBase statement) {

            if (statement == null) {
                throw new ArgumentNullException(nameof(statement));
            }

            if (statementList == null) {
                statementList = new List<StatementBase>();
            }

            statementList.Add(statement);
        }

        /// <summary>
        /// Afegeix una enumeracio d'ordres.
        /// </summary>
        /// <param name="statements">L'enumeracio d'ordres.</param>
        /// 
        public void AddStatements(IEnumerable<StatementBase> statements) {

            if (statements == null) {
                throw new ArgumentNullException(nameof(statements));
            }

            if (statementList == null) {
                statementList = new List<StatementBase>();
            }

            statementList.AddRange(statements);
        }

        /// <summary>
        /// Enumera d'estaments.
        /// </summary>
        /// 
        public IEnumerable<StatementBase> Statements => statementList;
    }
}
