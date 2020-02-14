namespace MicroCompiler.CodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class EnumeratorDeclaration : IUnitMember, IVisitable {

        private readonly string name;
        private readonly List<string> elementList;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Nom del enumerador.</param>
        /// <param name="elements">Els elements del enumerador.</param>
        /// 
        public EnumeratorDeclaration(string name, IEnumerable<string> elements) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            elementList = new List<string>(elements);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Nom del enumerador.</param>
        /// <param name="elements">Els elements del enumerador.</param>
        /// 
        public EnumeratorDeclaration(string name, params string[] elements) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            elementList = new List<string>(elements);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Nom del enumerador.</param>
        /// <param name="elements">Els elements del enumerador.</param>
        /// 
        public EnumeratorDeclaration(string name, List<string> elements) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            elementList = elements;
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
        /// Obte el nom.
        /// </summary>
        /// 
        public string Name => name;

        /// <summary>
        /// Enumera els elements.
        /// </summary>
        /// 
        public IEnumerable<string> Eements => elementList;
    }
}
