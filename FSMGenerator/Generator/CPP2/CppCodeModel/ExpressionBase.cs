namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    public abstract class ExpressionBase: IVisitable {

        public abstract void AcceptVisitor(IVisitor visitor);
    }
}
