namespace MicroCompiler.CodeModel {

    using System;

    public sealed class ConstructorInitializer : IVisitable {

        public ConstructorInitializer() {

        }

        public ConstructorInitializer(string name, Expression expression) {

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public string Name { get; set; }

        public Expression Expression { get; set; }
    }
}
