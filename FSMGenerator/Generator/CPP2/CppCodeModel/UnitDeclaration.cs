namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class UnitDeclaration: IVisitable {

        private readonly IEnumerable<IUnitMember> members;

        public UnitDeclaration(IEnumerable<IUnitMember> members) {

            if (members == null)
                throw new ArgumentNullException(nameof(members));

            this.members = members;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public IEnumerable<IUnitMember> Members => members;
    }
}
