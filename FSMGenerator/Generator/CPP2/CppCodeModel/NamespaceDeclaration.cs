namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class NamespaceDeclaration: IVisitable, INamespaceMember, IUnitMember {

        private readonly string name;
        private readonly IEnumerable<INamespaceMember> members;

        public NamespaceDeclaration(string name, IEnumerable<INamespaceMember> members) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            this.members = members;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Name => name;
        public IEnumerable<INamespaceMember> Members => members;
    }
}
