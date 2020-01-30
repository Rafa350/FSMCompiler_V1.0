namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class NamespaceDeclaration: IVisitable, INamespaceMember, IUnitMember {

        private string name;
        private readonly List<INamespaceMember> members = new List<INamespaceMember>();

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

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Afegeig un membre.
        /// </summary>
        /// <param name="member">El membre a afeigir.</param>
        /// 
        public void AddMember(INamespaceMember member) {

            if (member == null)
                throw new ArgumentNullException(nameof(member));

            members.Add(member);
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
        public IEnumerable<INamespaceMember> Members => members;
    }
}
