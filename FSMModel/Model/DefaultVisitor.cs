namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    public abstract class DefaultVisitor: IVisitor {

        public virtual void Visit(Machine machine) {

            foreach (State state in machine.States)
                state.AcceptVisitor(this);
        }

        public virtual void Visit(State state) {

            if (state.EnterAction != null)
                state.EnterAction.AcceptVisitor(this);

            if (state.ExitAction != null)
                state.ExitAction.AcceptVisitor(this);

            foreach (Transition transition in state.Transitions)
                transition.AcceptVisitor(this);
        }

        public virtual void Visit(Transition transition) {

            if (transition.Action != null)
                transition.Action.AcceptVisitor(this);
        }

        public virtual void Visit(Guard guard) {
        }

        public virtual void Visit(Action action) {

            if (action.HasActivities)
                foreach (Activity command in action.Activities)
                    command.AcceptVisitor(this);
        }

        public virtual void Visit(CodeActity command) {
        }

        public virtual void Visit(CallActivity command) {
        }
    }
}
