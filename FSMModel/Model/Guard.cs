namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;

    public sealed class Guard: IVisitable {

        private readonly string expression;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="expression">Expresio per evaluar la condicio.</param>
        /// 
        public Guard(string expression) {

            if (String.IsNullOrEmpty(expression))
                throw new ArgumentNullException(nameof(expression));

            this.expression = expression;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte la condicio.
        /// </summary>
        /// 
        public string Expression => expression;
    }
}
