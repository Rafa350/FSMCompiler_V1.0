namespace MicroCompiler.CodeModel {

    using System;

    public sealed class NamespaceImport : IVisitable {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public NamespaceImport() {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom per importar.</param>
        /// 
        public NamespaceImport(string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
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
        /// Obte o asigna el nom.
        /// </summary>
        /// 
        public string Name { get; set; }
    }
}
