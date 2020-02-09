namespace MicroCompiler.CodeModel {

    using System;

    public sealed class ArgumentDefinition : IVisitable {

        private readonly string name;
        private readonly TypeIdentifier valueType;

        public ArgumentDefinition(string name, TypeIdentifier valueType) {

            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentNullException(nameof(name));
            }

            this.name = name;
            this.valueType = valueType;
        }

        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public string Name => name;
        public TypeIdentifier ValueType => valueType;
    }
}
