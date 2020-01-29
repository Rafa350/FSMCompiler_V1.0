namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class MethodDeclaration: IClassMember, IVisitable {

        private readonly string name;
        private readonly string typeName;
        private readonly AccessSpecifier access;
        private readonly IEnumerable<ArgumentDefinition> arguments;
        private readonly string body;

        public MethodDeclaration(string name, string typeName, AccessSpecifier access, IEnumerable<ArgumentDefinition> arguments, string body) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(typeName))
                throw new ArgumentNullException(nameof(typeName));

            this.name = name;
            this.typeName = typeName;
            this.access = access;
            this.arguments = arguments;
            this.body = body;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Name => name;
        public string TypeName => typeName;
        public AccessSpecifier Access => access;
        public IEnumerable<ArgumentDefinition> Arguments => arguments;
        public string Body => body;
    }
}
