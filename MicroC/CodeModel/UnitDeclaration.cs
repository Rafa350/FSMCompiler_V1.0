namespace MicroCompiler.CodeModel {

    using System;
    using System.Collections.Generic;

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
        /// 
        public UnitDeclaration(UnitMemberDeclarationList members) {

            Members = members ?? throw new ArgumentNullException(nameof(members));
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

        /// <summary>
        /// Obte o asigna la llista de membres
        /// </summary>
        /// 
        public UnitMemberDeclarationList Members { get; set; }
    }
}
