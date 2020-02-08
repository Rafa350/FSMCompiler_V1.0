namespace MikroPicDesigns.FSMCompiler.v1.Model {

    public abstract class Activity : IVisitable {

        public abstract void AcceptVisitor(IVisitor visitor);
    }
}
