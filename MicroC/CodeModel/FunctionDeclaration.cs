namespace MicroCompiler.CodeModel {

    using System;
    using MicroCompiler.CodeModel.Statements;

    public sealed class FunctionDeclaration : INamespaceMember, IClassMember {

        private ArgumentDeclarationList arguments;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public FunctionDeclaration() {

            Access = AccessSpecifier.Default;
            Implementation = ImplementationSpecifier.Default;
            ReturnType = TypeIdentifier.FromName("void");
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="access">Especificador d'acces.</param>
        /// <param name="returnType">El tipus de retorn.</param>
        /// <param name="arguments">Llista d'arguments.</param>
        /// <param name="body">Les instruccions.</param>
        /// 
        public FunctionDeclaration(string name, AccessSpecifier access, TypeIdentifier returnType, ArgumentDeclarationList arguments,
            Statement body) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Access = access;
            ReturnType = returnType;
            this.arguments = arguments;
            Body = body;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="access">Especificador d'acces.</param>
        /// <param name="returnType">El tipus de retorn.</param>
        /// <param name="arguments">Llista d'arguments.</param>
        /// <param name="statements">La llista d'instruccions.</param>
        /// 
        public FunctionDeclaration(string name, AccessSpecifier access, TypeIdentifier returnType, ArgumentDeclarationList arguments,
            StatementList statements) :
            this(name, access, returnType, arguments, new BlockStatement(statements)) {
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }


        /// <summary>
        /// Obte o asigna el nom.
        /// </summary>
        /// 
        public string Name { get; set; }

        /// <summary>
        /// Obte o asigna el especificador d'acces.
        /// </summary>
        /// 
        public AccessSpecifier Access { get; set; }

        /// <summary>
        /// Obte o asigna l'especicficador d'implementacio.
        /// </summary>
        /// 
        public ImplementationSpecifier Implementation { get; set; }

        /// <summary>
        /// Obte o asigna tipus retornat.
        /// </summary>
        /// 
        public TypeIdentifier ReturnType { get; set; }

        /// <summary>
        /// Indica si te arguments.
        /// </summary>
        /// 
        public bool HasArguments => (arguments != null) && (arguments.Count > 0);

        /// <summary>
        /// Obte o asigna la llista d'arguments.
        /// </summary>
        /// 
        public ArgumentDeclarationList Arguments { 
            get {
                if (arguments == null)
                    arguments = new ArgumentDeclarationList();
                return arguments;
            }
            set {
                arguments = value;
            }
        }

        /// <summary>
        /// Obte o asigna les instruccions.
        /// </summary>
        /// 
        public Statement Body { get; set; }
    }
}
