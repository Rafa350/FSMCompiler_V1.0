namespace MicroCompiler.CodeModel {

    using System;
    using MicroCompiler.CodeModel.Statements;

    public sealed class DestructorDeclaration : IVisitable {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public DestructorDeclaration() {

            Access = AccessMode.Public;
            VirtualMode = DestructorVirtualMode.None;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="access">Modus d'acces.</param>
        /// <param name="isVirtual">Indica si es virtual.</param>
        /// <param name="body">El bloc d'instruccions.</param>
        /// 
        public DestructorDeclaration(AccessMode access, bool isVirtual, BlockStatement body) {

            Access = access;
            VirtualMode = isVirtual ? DestructorVirtualMode.Virtual : DestructorVirtualMode.None;
            Body = body;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="access">El modus d'acces.</param>
        /// <param name="isVirtual">Indica si es virtual.</param>
        /// <param name="statements">La llista d'instruccions.</param>
        /// 
        public DestructorDeclaration(AccessMode access, bool isVirtual, StatementList statements) :
            this(access, isVirtual, new BlockStatement(statements)) {
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

        /// <summary>
        /// Obte o asigna el modus d'acces.
        /// </summary>
        /// 
        public AccessMode Access { get; set; }

        /// <summary>
        /// Obte o asigna el indicador de virtual.
        /// </summary>
        /// 
        public DestructorVirtualMode VirtualMode { get; }

        /// <summary>
        /// Obte asigna el bloc de codi.
        /// </summary>
        /// 
        public BlockStatement Body { get; set; }
    }
}
