namespace MicroCompiler.CodeModel {

    using System;
    using System.Collections.Generic;

    public abstract class FunctionDeclarationBase : IVisitable {

        private string name;
        private TypeIdentifier returnType;
        private List<ArgumentDefinition> argumentList;
        private Block body;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public FunctionDeclarationBase() {
        }

        public abstract void AcceptVisitor(IVisitor visitor);

        /// <summary>
        /// Afegeig un argument.
        /// </summary>
        /// <param name="argument">L'argument.</param>
        /// 
        public void AddArgument(ArgumentDefinition argument) {

            if (argument == null) {
                throw new ArgumentNullException(nameof(argument));
            }

            if (argumentList == null) {
                argumentList = new List<ArgumentDefinition>();
            }

            argumentList.Add(argument);
        }

        /// <summary>
        /// Obte o asigna el nom de metode.
        /// </summary>
        /// 
        public string Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Obte o asigna tipus retornat.
        /// </summary>
        /// 
        public TypeIdentifier ReturnType {
            get { return returnType; }
            set { returnType = value; }
        }

        /// <summary>
        /// Enumera els arguments.
        /// </summary>
        /// 
        public IEnumerable<ArgumentDefinition> Arguments => argumentList;

        /// <summary>
        /// Obte o asigna el bloc.
        /// </summary>
        /// 
        public Block Body {
            get { return body; }
            set { body = value; }
        }
    }
}
