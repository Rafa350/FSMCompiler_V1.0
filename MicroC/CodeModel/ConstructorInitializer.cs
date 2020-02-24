namespace MicroCompiler.CodeModel {

    using System;

    public sealed class ConstructorInitializer : IVisitable {

        public ConstructorInitializer() {
        }

        public ConstructorInitializer(TypeIdentifier typeId, Expression expression) {

            TypeId = typeId;
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public TypeIdentifier TypeId { get; set; }

        public Expression Expression { get; set; }
    }
}
