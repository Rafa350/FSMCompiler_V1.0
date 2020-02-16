namespace MicroCompiler.CodeModel {

    using System;

    public abstract class DeclarationBlock : IVisitable {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        protected DeclarationBlock() {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="members">Llista de membres.</param>
        /// 
        protected DeclarationBlock(DeclarationBlockMemberList members) {

            Members = members ?? throw new ArgumentNullException(nameof(members));
        }

        public abstract void AcceptVisitor(IVisitor visitor);

        /// <summary>
        /// Obte o asigna la llista de declaracions.
        /// </summary>
        /// 
        public DeclarationBlockMemberList Members { get; set; }
    }
}
