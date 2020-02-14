namespace MicroCompiler.CodeModel {

    using System;

    public sealed class ClassDeclaration : IVisitable, IUnitMember {

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
