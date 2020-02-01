namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;

    public abstract class VariableDeclarationBase: IVisitable {

        private TypeIdentifier valueType;
        private string name;
        public ExpressionBase initializer;

        public VariableDeclarationBase() {
        }

        public abstract void AcceptVisitor(IVisitor visitor);

        public TypeIdentifier ValueType {
            get { return valueType; }
            set { valueType = value; }
        }

        public String Name {
            get { return name; }
            set { name = value; }
        }

        public ExpressionBase Initializer {
            get { return initializer; }
            set { initializer = value; }
        }
    }
}
