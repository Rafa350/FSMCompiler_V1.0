namespace MicroCompiler.CodeModel {

    using System;

    public sealed class ClassDeclaration : IVisitable, INamespaceMember {

        private ClassMemberList members;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public ClassDeclaration() {

            BaseAccess = AccessSpecifier.Public;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="baseName">En nom de la clase base.</param>
        /// <param name="baseAccess">Modus d'access de la clase base.</param>
        /// 
        public ClassDeclaration(string name, string baseName, AccessSpecifier baseAccess, ClassMemberList members) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            BaseName = baseName;
            BaseAccess = baseAccess;
            this.members = members;
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
        public AccessSpecifier BaseAccess { get; set; }

        /// <summary>
        /// Indica si conmte membres.
        /// </summary>
        /// 
        public bool HasMembers => members != null;

        /// <summary>
        /// Obte o asigna la llista de membres.
        /// </summary>
        /// 
        public ClassMemberList Members {
            get {
                if (members == null)
                    members = new ClassMemberList();
                return members;
            }
            set {
                members = value;
            }
        }
    }
}
