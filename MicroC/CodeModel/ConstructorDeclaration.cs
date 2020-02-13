namespace MicroCompiler.CodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class ConstructorDeclaration : IVisitable {

        private readonly AccessMode access;
        private readonly IEnumerable<ArgumentDefinition> arguments;
        private readonly Block body;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="access">Mode d'acces.</param>
        /// <param name="arguments">La llista d'arguments.</param>
        /// <param name="body">El cos del constructor.</param>
        /// 
        public ConstructorDeclaration(AccessMode access, IEnumerable<ArgumentDefinition> arguments = null, Block body = null) {

            this.access = access;
            this.arguments = arguments;
            this.body = body;
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

        public AccessMode Access => access;

        public IEnumerable<ArgumentDefinition> Arguments => arguments;

        public Block Body => body;
    }
}
