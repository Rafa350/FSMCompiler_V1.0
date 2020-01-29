namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;

    public sealed class FieldDeclaration: IClassMember, IVisitable {

        private readonly string name;
        private readonly string typeName;
        private readonly AccessSpecifier access;

        public FieldDeclaration(string name, string typeName, AccessSpecifier access) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(typeName))
                throw new ArgumentNullException(nameof(typeName));

            this.name = name;
            this.typeName = typeName;
            this.access = access;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Name => name;
        public string TypeName => typeName;
        public AccessSpecifier Access => access;
    }
}
