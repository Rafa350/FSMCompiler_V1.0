namespace MicroCompiler.CodeModel {

    using System;

    public sealed class ClassDeclaration : IVisitable, IDeclarationBlockMember {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public ClassDeclaration() {

            BaseAccess = AccessMode.Public;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="baseName">En nom de la clase base.</param>
        /// <param name="baseAccess">Modus d'access de la clase base.</param>
        /// <param name="variables">Llista de variables.</param>
        /// <param name="functions">Llista de funcions.</param>
        /// <param name="constructors">Llista de constructors.</param>
        /// <param name="destructor">Destructor.</param>
        /// 
        public ClassDeclaration(string name, string baseName, AccessMode baseAccess,
            MemberVariableDeclarationList variables,
            MemberFunctionDeclarationList functions, ConstructorDeclarationList constructors,
            DestructorDeclaration destructor) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            BaseName = baseName;
            BaseAccess = baseAccess;
            Variables = variables;
            Functions = functions;
            Constructors = constructors;
            Destructor = destructor;
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
        /// Obte o asigna el num de la clase.
        /// </summary>
        /// 
        public string Name { get; set; }

        /// <summary>
        /// Obte o asigna el nom de la clase base.
        /// </summary>
        /// 
        public string BaseName { get; set; }

        /// <summary>
        /// Obte o asigna el especificador d'acces de la clase base.
        /// </summary>
        /// 
        public AccessMode BaseAccess { get; set; }

        /// <summary>
        /// Obte o asigna la llista de functions membre.
        /// </summary>
        /// 
        public MemberFunctionDeclarationList Functions { get; set; }

        /// <summary>
        /// Obte o asigna le llista de variables membre.
        /// </summary>
        /// 
        public MemberVariableDeclarationList Variables { get; set; }

        /// <summary>
        /// Obte o asigna la llista de constructors.
        /// </summary>
        /// 
        public ConstructorDeclarationList Constructors { get; set; }

        /// <summary>
        /// Obte o asigna el destructor.
        /// </summary>
        /// 
        public DestructorDeclaration Destructor { get; set; }
    }
}
