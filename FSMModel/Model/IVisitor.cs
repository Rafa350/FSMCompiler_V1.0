namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using MikroPicDesigns.FSMCompiler.v1.Model.Actions;

    public interface IVisitor {

        void Visit(Machine machine);
        void Visit(Transition transition);
        void Visit(TransitionList transitions);
        void Visit(RaiseAction action);
        void Visit(GotoAction action);
        void Visit(InlineAction action);
        void Visit(ActionList actions);
        void Visit(State state);
        void Visit(Event ev);
    }
}
