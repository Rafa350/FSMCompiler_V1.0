namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class MemberFunctionDeclaration: FunctionDeclarationBase, IClassMember {

        private AccessMode accessMode = AccessMode.Private;
        private MemberFunctionMode mode = MemberFunctionMode.Instance;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public MemberFunctionDeclaration() { 
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
        /// Obte o asigna el especificador d'acces.
        /// </summary>
        /// 
        public AccessMode AccessMode {
            get { return accessMode; }
            set { accessMode = value; }
        }

        /// <summary>
        /// Obte o asigna el especificador de virtualizacio.
        /// </summary>
        /// 
        public MemberFunctionMode Mode {
            get { return mode; }
            set { mode = value; }
        }
    }
}
