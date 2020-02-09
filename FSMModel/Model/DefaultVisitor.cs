namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    public abstract class DefaultVisitor : IVisitor {

        public virtual void Visit(Machine machine) {

            foreach (var state in machine.States) {
                state.AcceptVisitor(this);
            }
        }

        public virtual void Visit(State state) {

            if (state.EnterAction != null) {
                state.EnterAction.AcceptVisitor(this);
            }

            if (state.ExitAction != null) {
                state.ExitAction.AcceptVisitor(this);
            }

            foreach (Transition transition in state.Transitions) {
                transition.AcceptVisitor(this);
            }
        }

        public virtual void Visit(Transition transition) {

            if (transition.Action != null) {
                transition.Action.AcceptVisitor(this);
            }
        }

        public virtual void Visit(Guard guard) {
        }

        public virtual void Visit(Action action) {

            if (action.Activities != null) {
                foreach (var activity in action.Activities) {
                    activity.AcceptVisitor(this);
                }
            }
        }

        public virtual void Visit(CodeActity activity) {
        }

        public virtual void Visit(CallActivity activity) {
        }
    }
}
