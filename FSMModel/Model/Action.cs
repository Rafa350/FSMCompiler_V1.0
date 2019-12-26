namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;

    public sealed class Action: IVisitable {

        private List<Command> commands = new List<Command>();

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// 
        public Action() {
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        /// <summary>
        /// Afegeix una comanda.
        /// </summary>
        /// <param name="command">La comanda.</param>
        /// 
        public void AddCommand(Command command) {

            if (command == null)
                throw new ArgumentNullException("command");

            commands.Add(command);
        }

        /// <summary>
        /// Enumera les comandes.
        /// </summary>
        /// 
        public IEnumerable<Command> Commands {
            get {
                return commands;
            }
        }

        /// <summary>
        /// Indica si hi han comandes.
        /// </summary>
        /// 
        public bool HasCommands {
            get {
                return (commands != null) && (commands.Count > 0);
            }
        }
    }
}
