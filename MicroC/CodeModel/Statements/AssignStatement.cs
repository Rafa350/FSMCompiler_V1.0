namespace MicroCompiler.CodeModel.Statements {

    using System;

    /// <summary>
    /// Clase que representa una instruccio d'asignacio.
    /// </summary>
    /// 
    public sealed class AssignStatement : Statement {

        /// <summary>
        /// Contructor per defecte.
        /// </summary>
        /// 
        public AssignStatement() {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="expression">L'expressio.</param>
        /// 
        public AssignStatement(string name, Expression expression) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.Name = name;
            this.Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte o asigna el nom de la variable
        /// </summary>
        /// 
        public string Name { get; set; }

        /// <summary>
        /// Obte o asigna l'expressio.
        /// </summary>
        /// 
        public Expression Expression { get; set; }
    }
}
