namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;
    
    public sealed class CommandList: IVisitable {

        private List<CommandBase> commands;

        public CommandList Add(CommandBase command) {

            if (command == null)
                throw new ArgumentNullException("command");

            if (commands == null)
                commands = new List<CommandBase>();

            commands.Add(command);

            return this;
        }

        public CommandList Add(IEnumerable<CommandBase> commands) {

            if (commands == null)
                throw new ArgumentNullException("commands");

            foreach (CommandBase command in commands)
                Add(command);

            return this;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public bool HasCommands {
            get {
                return commands != null;
            }
        }

        public IEnumerable<CommandBase> Commands {
            get {
                return commands;
            }
        }

    }
}
