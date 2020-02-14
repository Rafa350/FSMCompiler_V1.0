namespace MicroCompiler.CodeModel {

    using System;

    public sealed class FunctionDeclaration : FunctionDeclarationBase, IUnitMember {

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
    }
}
