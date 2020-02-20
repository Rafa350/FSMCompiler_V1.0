namespace MicroCompiler.CodeModel {

    using System;

    public sealed class UnitDeclaration : IVisitable {

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
        public UnitDeclaration(NamespaceDeclaration ns) {

            Namespace = ns;
        }

        /// <summary>
        /// Acepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public NamespaceDeclaration Namespace { get; set; }
    }
}
