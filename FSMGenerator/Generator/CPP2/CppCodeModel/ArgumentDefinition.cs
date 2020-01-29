namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;

    public sealed class ArgumentDefinition: IVisitable {

        private readonly string name;
        private readonly string typeName;

        public ArgumentDefinition(string name, string typeName) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(typeName))
                throw new ArgumentNullException(nameof(typeName));

            this.name = name;
            this.typeName = typeName;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Name => name;
        public string TypeName => typeName;
    }
}
