namespace MicroCompiler.CodeModel {

    public abstract class Statement : IVisitable {

        public abstract void AcceptVisitor(IVisitor visitor);
    }
}
