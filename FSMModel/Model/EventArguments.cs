namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    
    public sealed class EventArguments {

        private readonly string _expression;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="expression">Expresio per definir els parametres.</param>
        /// 
        public EventArguments(string expression) {

            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        /// <summary>
        /// Obte l'expressio dels parametres.
        /// </summary>
        /// 
        public string Expression => 
            _expression;
    }
}
