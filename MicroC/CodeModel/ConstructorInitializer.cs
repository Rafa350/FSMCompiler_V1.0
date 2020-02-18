namespace MicroCompiler.CodeModel {

    using System;

    public sealed class ConstructorInitializer: IVisitable {

        public ConstructorInitializer() {

        }

        public ConstructorInitializer(string name, Expression initializer) {

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
        }

        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public string Name { get; set; }

        public Expression Initializer { get; set; }
    }
}
