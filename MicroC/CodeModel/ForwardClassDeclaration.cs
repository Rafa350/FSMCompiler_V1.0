namespace MicroCompiler.CodeModel {

    using System;

    public sealed class ForwardClassDeclaration : INamespaceMember {

        public ForwardClassDeclaration() {

        }

        public ForwardClassDeclaration(string name) {

            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public string Name { get; set; }
    }
}
