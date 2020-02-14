namespace MicroCompiler.CodeModel {

    using System;

    public sealed class ArgumentDeclaration : IVisitable {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public ArgumentDeclaration() {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="valueType">El tipus del valor.</param>
        /// 
        public ArgumentDeclaration(string name, TypeIdentifier valueType) {

            Name = name;
            ValueType = valueType;
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
        /// Obte o asigna el nom.
        /// </summary>
        /// 
        public string Name { get; set; }

        /// <summary>
        /// Obte o asigna el tipus del valor.
        /// </summary>
        /// 
        public TypeIdentifier ValueType { get; set; }
    }
}
