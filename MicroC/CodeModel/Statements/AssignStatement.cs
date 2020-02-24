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
        /// <param name="valueExp">L'expressio.</param>
        /// 
        public AssignStatement(string name, Expression valueExp) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.Name = name;
            this.ValueExp = valueExp ?? throw new ArgumentNullException(nameof(valueExp));
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
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
        public Expression ValueExp { get; set; }
    }
}
