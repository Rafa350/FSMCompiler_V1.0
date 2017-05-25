namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;
    
    public sealed class State: IVisitable {

        private readonly State parent;
        private readonly string name;
        private List<State> childs;
        private TransitionList transitions;
        private ActionList enterActions;
        private ActionList exitActions;

        public State(string name)
            : this(null, name) {
        }

        public State(State parent, string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.parent = parent;
            this.name = name;

            if (parent != null) {
                if (parent.childs == null)
                    parent.childs = new List<State>();
                parent.childs.Add(this);
            }
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public State Parent {
            get {
                return parent;
            }
        }

        public string Name {
            get {
                return name;
            }
        }

        public string FullName {
            get {
                if (parent == null)
                    return name;
                else
                    return String.Format("{0}{1}", parent.FullName, name);
            }
        }

        public TransitionList Transitions {
            get {
                return transitions;
            }
            set {
                transitions = value;
            }
        }

        public bool HasEnterActions {
            get {
                State s = this;
                while (s != null) {
                    if (s.enterActions != null)
                        return true;
                    s = s.parent;
                }
                return false;
            }
        }

        public ActionList EnterActions {
            get {
                return enterActions;
            }
            set {
                enterActions = value;
            }
        }

        public bool HasExitActions {
            get {
                State s = this;
                while (s != null) {
                    if (s.exitActions != null)
                        return true;
                    s = s.parent;
                }
                return false;
            }
        }

        public ActionList ExitActions {
            get {
                return exitActions;
            }
            set {
                exitActions = value;
            }
        }

        public bool HasChilds {
            get {
                return childs != null;
            }
        }

        public IEnumerable<State> Childs {
            get {
                return childs;
            }
        }
    }
}
