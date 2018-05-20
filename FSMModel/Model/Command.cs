namespace MikroPicDesigns.FSMCompiler.v1.Model {
    
    public abstract class Command: IVisitable {

        public abstract void AcceptVisitor(IVisitor visitor);
    }
}
