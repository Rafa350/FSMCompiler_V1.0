namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;

    public sealed class Guard {

        private readonly string condition;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="condition">Expresio per evaluar la condicio.</param>
        /// 
        public Guard(string condition) {

            if (String.IsNullOrEmpty(condition))
                throw new ArgumentNullException("condition");

            this.condition = condition;
        }

        /// <summary>
        /// Obte la condicio.
        /// </summary>
        /// 
        public string Condition {
            get {
                return condition;
            }
        }
    }
}
