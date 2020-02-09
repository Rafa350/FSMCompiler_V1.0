namespace MicroCompiler.CodeModel {

    using System;

    public sealed class MemberVariableDeclaration : VariableDeclarationBase, IClassMember {

        private AccessMode access;
        private MemberVariableMode mode = MemberVariableMode.Instance;

        /// <summary>
        /// Constructpor per defecte.
        /// </summary>
        /// 
        public MemberVariableDeclaration() {
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null) {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte o asigna el nivell d'acces
        /// </summary>
        /// 
        public AccessMode Access {
            get { return access; }
            set { access = value; }
        }

        /// <summary>
        /// Obte o asigna el modus.
        /// </summary>
        /// 
        public MemberVariableMode Mode {
            get { return mode; }
            set { mode = value; }
        }
    }
}
