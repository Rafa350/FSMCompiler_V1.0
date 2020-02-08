namespace MicroCompiler.CodeModel {

    public interface IVisitable {

        void AcceptVisitor(IVisitor visitor);
    }
}
