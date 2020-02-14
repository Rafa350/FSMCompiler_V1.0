namespace MicroCompiler.CodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class UnitDeclaration : IVisitable {

        private readonly List<IUnitMember> members;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public UnitDeclaration(List<IUnitMember> members) {

            this.members = members ?? throw new ArgumentNullException(nameof(members));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="members">Els memberes.</param>
        /// 
        public UnitDeclaration(IEnumerable<IUnitMember> members) :
            this(new List<IUnitMember>(members)) {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="members">Els membres.</param>
        /// 
        public UnitDeclaration(params IUnitMember[] members) :
            this(new List<IUnitMember>(members)) {

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
