namespace MicroCompiler.CodeModel {

    using System;

    public sealed class MemberVariableDeclaration : VariableDeclarationBase, IClassMember {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public MemberVariableDeclaration() {

            Access = AccessMode.Private;
            Mode = MemberVariableMode.Instance;
        }

        /// <summary>
        /// Conmstructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="access">Tipus d'acces.</param>
        /// <param name="mode">Tipus d'instanciacio.</param>
        /// <param name="valueType">Tipus del valor.</param>
        /// <param name="initializer">Expressio d'inicialitzacio.</param>
        /// 
        public MemberVariableDeclaration(string name, AccessMode access, MemberVariableMode mode, 
            TypeIdentifier valueType, Expression initializer) :
            base(name, valueType, initializer) {

            Access = access;
            Mode = mode;
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
        /// Obte o asigna el nivell d'acces
        /// </summary>
        /// 
        public AccessMode Access { get; set; }

        /// <summary>
        /// Obte o asigna el modus.
        /// </summary>
        /// 
        public MemberVariableMode Mode { get; set; }
    }
}
