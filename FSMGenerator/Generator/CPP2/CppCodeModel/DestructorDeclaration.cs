namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;

    public sealed class DestructorDeclaration: IVisitable {

        private readonly AccessSpecifier access;
        private readonly string body;

        public DestructorDeclaration(AccessSpecifier access, string body) {

            this.access = access;
            this.body = body;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public AccessSpecifier Access => access;
        public string Body => body;
    }
}
