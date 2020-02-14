namespace MicroCompiler.CodeModel {

    public abstract class Expression : IVisitable {

        public abstract void AcceptVisitor(IVisitor visitor);
    }
}
