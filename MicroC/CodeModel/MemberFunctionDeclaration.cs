namespace MicroCompiler.CodeModel {

    using System;
    using MicroCompiler.CodeModel.Statements;

    public sealed class MemberFunctionDeclaration : FunctionDeclarationBase, IClassMember {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public MemberFunctionDeclaration() {

            Access = AccessMode.Private;
            Mode = MemberFunctionMode.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="access">El modus d'acces.</param>
        /// <param name="mode">El tipus d'instancia.</param>
        /// <param name="returnType">El tipus de retorn.</param>
        /// <param name="arguments">Llista d'arguments.</param>
        /// <param name="body">El bloc d'instruccions.</param>
        /// 
        public MemberFunctionDeclaration(string name, AccessMode access, MemberFunctionMode mode,
            TypeIdentifier returnType, ArgumentDeclarationList arguments, BlockStatement body) :
            base(name, returnType, arguments, body) {

            Access = access;
            Mode = mode;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="access">El modus d'acces.</param>
        /// <param name="mode">El tipus d'instancia.</param>
        /// <param name="returnType">El tipus de retorn.</param>
        /// <param name="arguments">Llista d'arguments.</param>
        /// <param name="statements">La llista d'instruccions.</param>
        /// 
        public MemberFunctionDeclaration(string name, AccessMode access, MemberFunctionMode mode,
            TypeIdentifier returnType, ArgumentDeclarationList arguments, StatementList statements) :
            base(name, returnType, arguments, statements) {

        }

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

        /// <summary>
        /// Obte o asigna el especificador d'acces.
        /// </summary>
        /// 
        public AccessMode Access { get; set; }

        /// <summary>
        /// Obte o asigna el especificador de virtualizacio.
        /// </summary>
        /// 
        public MemberFunctionMode Mode { get; set; }
    }
}
