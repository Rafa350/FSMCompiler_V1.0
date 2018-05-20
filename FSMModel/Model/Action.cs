namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;

    public sealed class Action: IVisitable {

        private List<Command> commands = new List<Command>();

        public Action() {
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public void AddCommand(Command command) {

            if (command == null)
                throw new ArgumentNullException("command");

            commands.Add(command);
        }

        public IEnumerable<Command> Commands {
            get {
                return commands;
            }
        }

        public bool HasCommands {
            get {
                return commands.Count > 0;
            }
        }
    }
}
