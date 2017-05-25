namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using MikroPicDesigns.FSMCompiler.v1.Model.Actions;

    public abstract class DefaultVisitor: IVisitor {

        public virtual void Visit(Machine machine) {

            foreach (State state in machine.States)
                state.AcceptVisitor(this);
        }

        public virtual void Visit(Transition transition) {

            if (transition.Actions != null)
                transition.Actions.AcceptVisitor(this);
        }

        public virtual void Visit(TransitionList transitions) {

            if (transitions.HasTransitions)
                foreach (Transition transition in transitions.Transitions)
                    transition.AcceptVisitor(this);
        }

        public virtual void Visit(InlineAction action) {
        }

        public virtual void Visit(GotoAction action) {
        }

        public virtual void Visit(RaiseAction action) {
        }

        public virtual void Visit(ActionList actions) {

            if (actions.HasActions)
                foreach (ActionBase action in actions.Actions)
                    action.AcceptVisitor(this);
        }

        public virtual void Visit(State state) {

            if (state.EnterActions != null)
                state.EnterActions.AcceptVisitor(this);

            if (state.ExitActions != null)
                state.ExitActions.AcceptVisitor(this);

            if (state.Transitions != null)
                state.Transitions.AcceptVisitor(this);
        }

        public virtual void Visit(Event ev) {
        }
    }
}
