namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;

    /// <summary>
    /// Clase que representa un event.
    /// </summary>
    /// 
    public sealed class Event: IVisitable {

        private readonly string name;
        private readonly EventArguments arguments;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="name">Nom.</param>
        /// <param name="arguments">Parametres.</param>
        /// 
        public Event(string name, EventArguments arguments) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            this.arguments = arguments;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visaitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte el nom.
        /// </summary>
        /// 
        public string Name => name;

        /// <summary>
        /// Obte els parametres.
        /// </summary>
        /// 
        public EventArguments Arguments => arguments;
    }
}
