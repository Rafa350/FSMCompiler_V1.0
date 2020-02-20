namespace MicroCompiler.CodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class EnumerationDeclaration : INamespaceMember, IClassMember {

        private List<string> elements;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public EnumerationDeclaration() {

            Access = AccessSpecifier.Public;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Nom del enumerador.</param>
        /// <param name="access">Especificador d'acces.</param>
        /// <param name="elements">Els elements del enumerador.</param>
        /// 
        public EnumerationDeclaration(string name, AccessSpecifier access, List<string> elements) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Access = access;
            this.elements = elements;
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
        /// Obte o asigna els especificadors d'acces.
        /// </summary>
        /// 
        public AccessSpecifier Access { get; set; }

        /// <summary>
        /// Obte o asigna el nom.
        /// </summary>
        /// 
        public string Name { get; set; }

        /// <summary>
        /// Indica si conte elements.
        /// </summary>
        /// 
        public bool HasElements => elements != null;

        /// <summary>
        /// Obte o asigna els elements.
        /// </summary>
        /// 
        public List<string> Elements {
            get {
                if (elements == null)
                    elements = new List<string>();
                return elements;
            }
            set {
                elements = value;
            }
        }
    }
}
