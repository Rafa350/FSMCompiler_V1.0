namespace MicroCompiler.CodeModel {

    using System;

    /// <summary>
    /// Clase que representa un espai de noms.
    /// </summary>
    /// 
    public sealed class NamespaceDeclaration : INamespaceMember, IVisitable {

        private NamespaceImportList imports;
        private NamespaceMemberList members;
        private NamespaceDeclarationList namespaces;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public NamespaceDeclaration() {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom de l'espai de noms.</param>
        /// <param name="imports">La llista d'importacio.</param>
        /// <param name="namespaces">La llista de espais de noms.</param>
        /// <param name="members">La llista de membres.</param>
        /// 
        public NamespaceDeclaration(string name, NamespaceImportList imports, NamespaceDeclarationList namespaces, NamespaceMemberList members) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            this.imports = imports;
            this.namespaces = namespaces;
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
        /// Obte o asigna el nom del espai de noms.
        /// </summary>
        /// 
        public string Name { get; set; }

        /// <summary>
        /// Comprova si te noms per importar.
        /// </summary>
        /// 
        public bool HasImports => (imports != null) && (imports.Count > 0);

        /// <summary>
        /// Comprova si te membres.
        /// </summary>
        /// 
        public bool HasMembers => (members != null) && (members.Count > 0);

        /// <summary>
        /// Comprova si te espais de noms.
        /// </summary>
        /// 
        public bool HasNamespaces => (namespaces != null) && (namespaces.Count > 0);

        /// <summary>
        /// Obte o asigna la llista no noms per importar.
        /// </summary>
        /// 
        public NamespaceImportList Imports {
            get {
                if (imports == null)
                    imports = new NamespaceImportList();
                return imports;
            }
            set {
                imports = value;
            }
        }

        /// <summary>
        /// Obte o asigna la llista de membres
        /// </summary>
        /// 
        public NamespaceMemberList Members {
            get {
                if (members == null)
                    members = new NamespaceMemberList();
                return members;
            }
            set {
                members = value;
            }
        }

        /// <summary>
        /// Obte o asigna la llista d'espais de noms.
        /// </summary>
        /// 
        public NamespaceDeclarationList Namespaces {
            get {
                if (namespaces == null)
                    namespaces = new NamespaceDeclarationList();
                return namespaces;
            }
            set {
                namespaces = value;
            }
        }
    }
}
