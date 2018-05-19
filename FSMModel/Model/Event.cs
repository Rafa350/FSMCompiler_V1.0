namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;

    public sealed class Event: IVisitable {

        private readonly string name;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// 
        public Event(string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.name = name;
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
        /// Obte el nom.
        /// </summary>
        /// 
        public string Name {
            get {
                return name;
            }
        }
    }
}
