namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;

    public sealed class DestructorDeclaration: IVisitable {

        private readonly AccessMode access;
        private readonly string body;
        private readonly DestructorVirtualMode virtualMode = DestructorVirtualMode.None;

        public DestructorDeclaration(AccessMode access, string body, bool isVirtual = false) {

            this.access = access;
            this.body = body;
            this.virtualMode = isVirtual ? DestructorVirtualMode.Virtual : DestructorVirtualMode.None;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public AccessMode Access => access;
        public DestructorVirtualMode VirtualMode => virtualMode;
        public string Body => body;
    }
}
