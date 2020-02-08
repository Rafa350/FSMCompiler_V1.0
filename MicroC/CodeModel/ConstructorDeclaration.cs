namespace MicroCompiler.CodeModel {

    using System.Collections.Generic;

    public sealed class ConstructorDeclaration : IVisitable {

        private readonly AccessMode access;
        private readonly IEnumerable<ArgumentDefinition> arguments;
        private readonly Block body;

        public ConstructorDeclaration(AccessMode access, IEnumerable<ArgumentDefinition> arguments = null, Block body = null) {

            this.access = access;
            this.arguments = arguments;
            this.body = body;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public AccessMode Access => access;
        public IEnumerable<ArgumentDefinition> Arguments => arguments;
        public Block Body => body;
    }
}
