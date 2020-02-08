namespace MicroCompiler.CodeModel {

    public abstract class ExpressionBase : IVisitable {

        public abstract void AcceptVisitor(IVisitor visitor);
    }
}
