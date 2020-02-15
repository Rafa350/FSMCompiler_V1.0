namespace MicroCompiler.CodeModel {

    using System;
    using MicroCompiler.CodeModel.Statements;

    public abstract class FunctionDeclarationBase : IVisitable {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        protected FunctionDeclarationBase() {

            ReturnType = TypeIdentifier.FromName("void");
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="returnType">El tipus de retorn.</param>
        /// <param name="body">Les instruccions.</param>
        /// 
        protected FunctionDeclarationBase(string name, TypeIdentifier returnType, Statement body) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            ReturnType = returnType;
            Body = body;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="returnType">El tipus de retorn.</param>
        /// <param name="statements">La llista d'instruccions.</param>
        /// 
        protected FunctionDeclarationBase(string name, TypeIdentifier returnType, StatementList statements) :
            this(name, returnType, new BlockStatement(statements)) {
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public abstract void AcceptVisitor(IVisitor visitor);

        /// <summary>
        /// Obte o asigna el nom.
        /// </summary>
        /// 
        public string Name { get; set; }

        /// <summary>
        /// Obte o asigna tipus retornat.
        /// </summary>
        /// 
        public TypeIdentifier ReturnType { get; set; }

        /// <summary>
        /// Obte o asigna la llista d'arguments.
        /// </summary>
        /// 
        public ArgumentDeclarationList Arguments { get; set; }

        /// <summary>
        /// Obte o asigna les instruccions.
        /// </summary>
        /// 
        public Statement Body { get; set; }
    }
}
