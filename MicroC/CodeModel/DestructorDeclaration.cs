namespace MicroCompiler.CodeModel {

    using System;
    
    public sealed class DestructorDeclaration : IVisitable {

        private readonly AccessMode access;
        private readonly string body;
        private readonly DestructorVirtualMode virtualMode = DestructorVirtualMode.None;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="access"></param>
        /// <param name="body"></param>
        /// <param name="isVirtual"></param>
        /// 
        public DestructorDeclaration(AccessMode access, string body, bool isVirtual = false) {

            this.access = access;
            this.body = body;
            this.virtualMode = isVirtual ? DestructorVirtualMode.Virtual : DestructorVirtualMode.None;
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
        
        public DestructorVirtualMode VirtualMode => virtualMode;
        
        public string Body => body;
    }
}
