namespace MikroPicDesigns.FSMCompiler.v1.Model {

    public interface IVisitable {

        void AcceptVisitor(IVisitor visitor);
    }
}
