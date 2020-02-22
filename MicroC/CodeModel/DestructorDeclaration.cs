namespace MicroCompiler.CodeModel {

    using System;
    using MicroCompiler.CodeModel.Statements;

    public sealed class DestructorDeclaration : IClassMember {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public DestructorDeclaration() {

            Access = AccessSpecifier.Default;
            Implemetation = ImplementationSpecifier.Default;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="access">Modus d'acces.</param>
        /// <param name="implementation">Tipus d'implementacio.</param>
        /// <param name="body">El bloc d'instruccions.</param>
        /// 
        public DestructorDeclaration(AccessSpecifier access, ImplementationSpecifier implementation, BlockStatement body) {

            Access = access;
            Implemetation = implementation;
            Body = body;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="access">El modus d'acces.</param>
        /// <param name="implementation">Tipus d'implementacio.</param>
        /// <param name="statements">La llista d'instruccions.</param>
        /// 
        public DestructorDeclaration(AccessSpecifier access, ImplementationSpecifier implementation, StatementList statements) :
            this(access, implementation, new BlockStatement(statements)) {
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
        public AccessSpecifier Access { get; set; }

        /// <summary>
        /// Obte o asigna el indicador de virtual.
        /// </summary>
        /// 
        public ImplementationSpecifier Implemetation { get; set; }

        /// <summary>
        /// Obte asigna el bloc de codi.
        /// </summary>
        /// 
        public BlockStatement Body { get; set; }
    }
}
