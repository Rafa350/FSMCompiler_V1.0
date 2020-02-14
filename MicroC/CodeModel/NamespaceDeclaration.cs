namespace MicroCompiler.CodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class NamespaceDeclaration : IVisitable, IUnitMember {

        private string name;
        private List<IUnitMember> members;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public NamespaceDeclaration() {
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null) {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }

        /// <summary>
        /// Afegeig un membre.
        /// </summary>
        /// <param name="member">El membre a afeigir.</param>
        /// 
        public void AddMember(IUnitMember member) {

            if (member == null) {
                throw new ArgumentNullException(nameof(member));
            }

            if (members == null) {
                members = new List<IUnitMember>();
            }

            members.Add(member);
        }

        public void AddMembers(IEnumerable<IUnitMember> members) {

            if (members == null) {
                throw new ArgumentNullException(nameof(members));
            }

            foreach (var member in members) {
                AddMember(member);
            }
        }

        /// <summary>
        /// Obte o asigna el nom del espai de noms.
        /// </summary>
        /// 
        public string Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Enumera els membres.
        /// </summary>
        /// 
        public IEnumerable<IUnitMember> Members => members;
    }
}
