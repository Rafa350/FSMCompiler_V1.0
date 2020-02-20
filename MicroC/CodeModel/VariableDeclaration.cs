namespace MicroCompiler.CodeModel {

    using System;

    public sealed class VariableDeclaration : IClassMember, INamespaceMember {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public VariableDeclaration() {

            Access = AccessSpecifier.Default;
            Implementation = ImplementationSpecifier.Default;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="access">Especificador d'acces.</param>
        /// <param name="valueType">El tipus del valor.</param>
        /// <param name="initializer">Expressio d'inicialitzacio.</param>
        /// 
        public VariableDeclaration(string name, AccessSpecifier access, TypeIdentifier valueType, Expression initializer) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Access = access;
            ValueType = valueType;
            Initializer = initializer;
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

        public AccessSpecifier Access { get; set; }

        public ImplementationSpecifier Implementation { get; set; }

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
