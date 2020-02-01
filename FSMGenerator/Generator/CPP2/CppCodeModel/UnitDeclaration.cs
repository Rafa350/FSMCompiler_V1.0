namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class UnitDeclaration: IVisitable {

        private readonly List<IUnitMember> memberList = new List<IUnitMember>();

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        public UnitDeclaration() {
        }

        /// <summary>
        /// Afegeig un membre.
        /// </summary>
        /// <param name="member">El membre a afeigir.</param>
        /// 
        public void AddMember(IUnitMember member) {

            if (member == null)
                throw new ArgumentNullException(nameof(member));

            memberList.Add(member);
        }

        /// <summary>
        /// Afegeig un membre.
        /// </summary>
        /// <param name="member">El membre a afeigir.</param>
        /// 
        public void AddMembers(IEnumerable<IUnitMember> members) {

            if (members == null)
                throw new ArgumentNullException(nameof(members));

            memberList.AddRange(members);
        }

        /// <summary>
        /// Acepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Enumera els membres.
        /// </summary>
        /// 
        public IEnumerable<IUnitMember> Members => memberList;
    }
}
