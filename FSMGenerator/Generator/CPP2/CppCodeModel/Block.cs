namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Clase que representa un bloc de codi.
    /// </summary>
    /// 
    public sealed class Block: IVisitable {

        private List<StatementBase> statmentList;

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

            if (statements == null)
                throw new ArgumentNullException(nameof(statements));

            foreach (var statement in statements)
                InternalAddStatement(statement);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statements">Llista de estaments.</param>
        /// 
        public Block(params StatementBase[] statements):
            this((IEnumerable<StatementBase>) statements) {

        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        /// <summary>
        /// Afegeig una ordre.
        /// </summary>
        /// <param name="statement">La ordre.</param>
        /// 
        public void AddStatement(StatementBase statement) {

            if (statement == null)
                throw new ArgumentNullException(nameof(statement));

            InternalAddStatement(statement);
        }

        /// <summary>
        /// Afegeix una enumeracio d'ordres.
        /// </summary>
        /// <param name="statements">L'enumeracio d'ordres.</param>
        /// 
        public void AddStatements(IEnumerable<StatementBase> statements) {

            if (statements == null)
                throw new ArgumentNullException(nameof(statements));

            foreach (var statement in statements)
                InternalAddStatement(statement);
        }

        private void InternalAddStatement(StatementBase statement) {

            if (statmentList == null)
                statmentList = new List<StatementBase>();

            statmentList.Add(statement);
        }

        /// <summary>
        /// Enumera d'estaments.
        /// </summary>
        /// 
        public IEnumerable<StatementBase> Statements => statmentList;
    }
}
