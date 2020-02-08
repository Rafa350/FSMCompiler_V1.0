namespace MicroCompiler.CodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class ClassDeclaration : IVisitable, IUnitMember {

        private string name;
        private string baseName;
        private AccessMode baseAccess;
        private List<MemberFunctionDeclaration> functionList;
        private List<MemberVariableDeclaration> variableList;
        private List<ConstructorDeclaration> constructorList;
        private DestructorDeclaration destructor;

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public ClassDeclaration() {

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
        /// Afegeix un constructor.
        /// </summary>
        /// <param name="constructor">El constructor.</param>
        /// 
        public void AddConstructor(ConstructorDeclaration constructor) {

            if (constructor == null)
                throw new ArgumentNullException(nameof(constructor));

            InternalAddContructor(constructor);
        }

        /// <summary>
        /// Afegeix diversos constructor.
        /// </summary>
        /// <param name="constructor">Els constructor.</param>
        /// 
        public void AddConstructors(IEnumerable<ConstructorDeclaration> constructors) {

            if (constructors == null)
                throw new ArgumentNullException(nameof(constructors));

            foreach (var constructor in constructors)
                InternalAddContructor(constructor);
        }

        /// <summary>
        /// Afegeix un constructor.
        /// </summary>
        /// <param name="constructor">El constructor.</param>
        /// 
        private void InternalAddContructor(ConstructorDeclaration constructor) {

            if (constructorList == null)
                constructorList = new List<ConstructorDeclaration>();

            constructorList.Add(constructor);
        }

        /// <summary>
        /// Afegeix una funcio membre.
        /// </summary>
        /// <param name="function">La funcio.</param>
        /// 
        public void AddMemberFunction(MemberFunctionDeclaration function) {

            if (function == null)
                throw new ArgumentNullException(nameof(function));

            InternalAddMemberFunction(function);
        }

        /// <summary>
        /// Afegeix diverses funcions membre..
        /// </summary>
        /// <param name="functions">Les funcions.</param>
        /// 
        public void AddMemberFunctions(IEnumerable<MemberFunctionDeclaration> functions) {

            if (functions == null)
                throw new ArgumentNullException(nameof(functions));

            foreach (var function in functions)
                InternalAddMemberFunction(function);
        }

        /// <summary>
        /// Afegeix una funcio membre.
        /// </summary>
        /// <param name="function">La funcio.</param>
        /// 
        private void InternalAddMemberFunction(MemberFunctionDeclaration function) {

            if (functionList == null)
                functionList = new List<MemberFunctionDeclaration>();

            functionList.Add(function);
        }

        /// <summary>
        /// Afegeig una variable membre.
        /// </summary>
        /// <param name="variable">La variable.</param>
        /// 
        public void AddMemberVariable(MemberVariableDeclaration variable) {

            if (variable == null)
                throw new ArgumentNullException(nameof(variable));

            InternalAddMemberVariable(variable);
        }

        /// <summary>
        /// Afegeix diverses variables membre.
        /// </summary>
        /// <param name="variables">Les variables.</param>
        /// 
        public void AddMemberVariable(IEnumerable<MemberVariableDeclaration> variables) {

            if (variables == null)
                throw new ArgumentNullException(nameof(variables));

            foreach (var variable in Variables)
                InternalAddMemberVariable(variable);
        }

        /// <summary>
        /// Afegeig una variable membre.
        /// </summary>
        /// <param name="variable">La variable.</param>
        /// 
        private void InternalAddMemberVariable(MemberVariableDeclaration variable) {

            if (variableList == null)
                variableList = new List<MemberVariableDeclaration>();

            variableList.Add(variable);
        }

        /// <summary>
        /// Obte o asigna el num de la clase.
        /// </summary>
        /// 
        public string Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Obte o asigna el nom de la clase base.
        /// </summary>
        /// 
        public string BaseName {
            get { return baseName; }
            set { baseName = value; }
        }

        /// <summary>
        /// Obte o asigna el especificador d'acces de la clase base.
        /// </summary>
        /// 
        public AccessMode BaseAccess {
            get { return baseAccess; }
            set { baseAccess = value; }
        }

        /// <summary>
        /// Enumera les funcions membre.
        /// </summary>
        /// 
        public IEnumerable<MemberFunctionDeclaration> Functions => functionList;

        /// <summary>
        /// Enumera les variables membre.
        /// </summary>
        /// 
        public IEnumerable<MemberVariableDeclaration> Variables => variableList;

        /// <summary>
        /// Enumera els constructors.
        /// </summary>
        /// 
        public IEnumerable<ConstructorDeclaration> Constructors => constructorList;

        /// <summary>
        /// Obte o asigna el destructor.
        /// </summary>
        /// 
        public DestructorDeclaration Destructor {
            get { return destructor; }
            set { destructor = value; }
        }
    }
}
