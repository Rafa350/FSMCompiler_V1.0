namespace MicroCompiler.CodeModel {

    using System;

    public sealed class ConstructorDeclaration : IVisitable {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public ConstructorDeclaration() {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="access">Modus d'acces.</param>
        /// <param name="arguments">Llista d'argument.</param>
        /// <param name="body">El bloc de codi.</param>
        /// 
        public ConstructorDeclaration(AccessMode access, ArgumentDeclarationList arguments, Block body) {

            Access = access;
            Arguments = arguments;
            Body = body;
        }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="access">Modus d'acces.</param>
        /// <param name="arguments">Llista d'argument.</param>
        /// <param name="statements">Llista d'instruccions.</param>
        /// 
        public ConstructorDeclaration(AccessMode access, ArgumentDeclarationList arguments, StatementList statements):
            this(access, arguments, new Block(statements)) {
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
        /// Obte o asigna la llista d'arguments.
        /// </summary>
        /// 
        public ArgumentDeclarationList Arguments { get; set; }

        /// <summary>
        /// Obte o asigna el bloc de codi.
        /// </summary>
        /// 
        public Block Body { get; set; }
    }
}
