using System;

namespace MikroPicDesigns.FSMCompiler.v1.Model {

    /// <summary>
    /// Clase que representa un event.
    /// </summary>
    /// 
    public sealed class Event: IVisitable {

        private readonly string _name;
        private readonly EventArguments _arguments;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="name">Nom.</param>
        /// <param name="arguments">Parametres.</param>
        /// 
        public Event(string name, EventArguments arguments) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _name = name;
            _arguments = arguments;
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
        public string Name => 
            _name;

        /// <summary>
        /// Obte els parametres.
        /// </summary>
        /// 
        public EventArguments Arguments => 
            _arguments;
    }
}
