namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class Block: IVisitable {

        private List<VariableDeclarationBase> variableDeclarationList;
        private List<StatementBase> statmentList;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public Block() { 
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
        /// Afegeix una declaracio de variable.
        /// </summary>
        /// <param name="variableDeclaration">La declaracio.</param>
        /// 
        public void AddVariableDeclaration(VariableDeclarationBase variableDeclaration) {

            if (variableDeclaration == null)
                throw new ArgumentNullException(nameof(variableDeclaration));

            if (variableDeclarationList == null)
                variableDeclarationList = new List<VariableDeclarationBase>();

            variableDeclarationList.Add(variableDeclaration);
        }

        /// <summary>
        /// Afegeig una ordre.
        /// </summary>
        /// <param name="statement">La ordre.</param>
        /// 
        public void AddStatement(StatementBase statement) {

            if (statement == null)
                throw new ArgumentNullException(nameof(statement));

            if (statmentList == null)
                statmentList = new List<StatementBase>();

            statmentList.Add(statement);
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
                AddStatement(statement);
        }

        /// <summary>
        /// Enumera les declaracions de variables.
        /// </summary>
        /// 
        public IEnumerable<VariableDeclarationBase> VariableDeclarations => variableDeclarationList;

        /// <summary>
        /// Enumera les ordres.
        /// </summary>
        /// 
        public IEnumerable<StatementBase> Statements => statmentList;
    }
}
