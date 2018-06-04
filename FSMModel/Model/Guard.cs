namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;

    public sealed class Guard {

        private readonly string expression;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="expression">Expresio per evaluar la condicio.</param>
        /// 
        public Guard(string expression) {

            if (String.IsNullOrEmpty(expression))
                throw new ArgumentNullException("condition");

            this.expression = expression;
        }

        /// <summary>
        /// Obte la condicio.
        /// </summary>
        /// 
        public string Expression {
            get {
                return expression;
            }
        }
    }
}
