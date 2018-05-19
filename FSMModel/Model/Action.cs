namespace MikroPicDesigns.FSMCompiler.v1.Model {

    public sealed class Action: IVisitable {

        private CommandList commands;

        public Action(CommandList commands) {

            this.commands = commands;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public CommandList Commands {
            get {
                return commands;
            }
        }
    }
}
