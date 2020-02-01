namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Expressions {

    using System;

    public sealed class IdentifierExpression: ExpressionBase {

        private readonly string name;

        public IdentifierExpression(string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            
            this.name = name;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Name {
            get {
                return name;
            }
        }
    }
}
