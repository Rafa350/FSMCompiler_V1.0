namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;
    
    public sealed class ActionList: IVisitable {

        private List<ActionBase> actions;

        public ActionList Add(ActionBase action) {

            if (action == null)
                throw new ArgumentNullException("action");

            if (actions == null)
                actions = new List<ActionBase>();

            actions.Add(action);

            return this;
        }

        public ActionList Add(IEnumerable<ActionBase> actions) {

            if (actions == null)
                throw new ArgumentNullException("actions");

            foreach (ActionBase action in actions)
                Add(action);

            return this;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public bool HasActions {
            get {
                return actions != null;
            }
        }

        public IEnumerable<ActionBase> Actions {
            get {
                return actions;
            }
        }

    }
}
