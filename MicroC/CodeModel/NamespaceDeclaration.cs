namespace MicroCompiler.CodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class NamespaceDeclaration : IVisitable, IUnitMember {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public NamespaceDeclaration() {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="members">La llista de membres.</param>
        /// 
        public NamespaceDeclaration(string name, UnitMemberDeclarationList members) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Members = members ?? throw new ArgumentNullException(nameof(members));
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
        /// Obte o asigna la llista de membres.
        /// </summary>
        /// 
        public UnitMemberDeclarationList Members { get; set; }
    }
}
