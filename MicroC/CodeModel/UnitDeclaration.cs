namespace MicroCompiler.CodeModel {

    using System;

    public sealed class UnitDeclaration : DeclarationBlock {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public UnitDeclaration() {
        }

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// <param name="members">Llista de membres.</param>
        /// 
        public UnitDeclaration(DeclarationBlockMemberList members) :
            base(members) {
        }

        /// <summary>
        /// Acepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }
    }
}
