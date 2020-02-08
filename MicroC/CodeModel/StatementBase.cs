namespace MicroCompiler.CodeModel {

    public abstract class StatementBase : IVisitable {

        public abstract void AcceptVisitor(IVisitor visitor);
    }
}
