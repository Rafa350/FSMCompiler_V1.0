namespace MicroCompiler.CodeModel {

    using System;

    public abstract class VariableDeclarationBase : IVisitable {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        protected VariableDeclarationBase() {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="valueType">El tipus del valor.</param>
        /// <param name="initializer">Expressio d'inicialitzacio.</param>
        /// 
        protected VariableDeclarationBase(string name, TypeIdentifier valueType, Expression initializer) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            ValueType = valueType;
            Initializer = initializer;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public abstract void AcceptVisitor(IVisitor visitor);

        /// <summary>
        /// Obte o asigna el tipus del valor.
        /// </summary>
        /// 
        public TypeIdentifier ValueType { get; set; }

        /// <summary>
        /// Obte o asigna el nom.
        /// </summary>
        /// 
        public String Name { get; set; }

        /// <summary>
        /// Obte o asigna l'expressio d'inicialitzacio.
        /// </summary>
        /// 
        public Expression Initializer { get; set; }
    }
}
