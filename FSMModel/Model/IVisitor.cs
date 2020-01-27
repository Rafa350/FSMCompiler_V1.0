namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using MikroPicDesigns.FSMCompiler.v1.Model.Commands;

    public interface IVisitor {

        void Visit(Machine machine);
        void Visit(State state);
        void Visit(Transition transition);
        void Visit(Guard guard);
        void Visit(Action action);

        void Visit(InlineCommand action);
        void Visit(MachineCommand action);
    }
}
