namespace MicroCompiler.CodeModel {

    using System;
    using MicroCompiler.CodeModel.Statements;

    public sealed class ConstructorDeclaration : IClassMember {


        private ArgumentDeclarationList arguments;
        private ConstructorInitializerList initializers;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public ConstructorDeclaration() {

        }

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="access">Modus d'acces.</param>
        /// 
        public ConstructorDeclaration(AccessSpecifier access) {

            Access = access;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="access">Modus d'acces.</param>
        /// <param name="arguments">Llista d'argument.</param>
        /// <param name="initializers">llista d'inicialitzadors.</param>
        /// <param name="body">El bloc de codi.</param>
        /// 
        public ConstructorDeclaration(AccessSpecifier access, ArgumentDeclarationList arguments, ConstructorInitializerList initializers, BlockStatement body) {

            Access = access;
            this.arguments = arguments;
            this.initializers = initializers;
            Body = body;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="access">Modus d'acces.</param>
        /// <param name="arguments">Llista d'argument.</param>
        /// <param name="initializers">llista d'inicialitzadors.</param>
        /// <param name="statements">Llista d'instruccions.</param>
        /// 
        public ConstructorDeclaration(AccessSpecifier access, ArgumentDeclarationList arguments, ConstructorInitializerList initializers, StatementList statements) :
            this(access, arguments, initializers, new BlockStatement(statements)) {
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
        /// Obte o asigna el modus d'acces.
        /// </summary>
        /// 
        public AccessSpecifier Access { get; set; }

        /// <summary>
        /// Indica si conte arguments.
        /// </summary>
        /// 
        public bool HasArguments => arguments != null;

        /// <summary>
        /// Indica si conte inicialitzadors.
        /// </summary>
        /// 
        public bool HasInitializers => initializers != null;

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
        /// Obte o asigna la llista d'inicialitzadors.
        /// </summary>
        /// 
        public ConstructorInitializerList Initializers {
            get {
                if (initializers == null)
                    initializers = new ConstructorInitializerList();
                return initializers;
            }
            set {
                initializers = value;
            }
        }

        /// <summary>
        /// Obte o asigna el bloc de codi.
        /// </summary>
        /// 
        public BlockStatement Body { get; set; }
    }
}
