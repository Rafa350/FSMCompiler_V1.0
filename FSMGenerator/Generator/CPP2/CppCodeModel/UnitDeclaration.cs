namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class UnitDeclaration: IVisitable {

        private readonly List<IUnitMember> members = new List<IUnitMember>();

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

            members.Add(member);
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
        public IEnumerable<IUnitMember> Members => members;
    }
}
