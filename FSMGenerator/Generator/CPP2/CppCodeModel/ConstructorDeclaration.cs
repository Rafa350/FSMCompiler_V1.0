namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System.Collections.Generic;

    public sealed class ConstructorDeclaration: IVisitable {

        private readonly AccessSpecifier access;
        private readonly IEnumerable<ArgumentDefinition> arguments;
        private readonly string body;

        public ConstructorDeclaration(AccessSpecifier access, IEnumerable<ArgumentDefinition> arguments, string body) {

            this.access = access;
            this.arguments = arguments;
            this.body = body;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public AccessSpecifier Access => access;
        public IEnumerable<ArgumentDefinition> Arguments => arguments;
        public string Body => body;
    }
}
