namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class ClassDeclaration: IVisitable, INamespaceMember, IUnitMember {

        private string name;
        private string baseName;
        private AccessSpecifier baseAccess;
        private readonly List<MethodDeclaration> methods = new List<MethodDeclaration>();
        private readonly List<ConstructorDeclaration> constructors = new List<ConstructorDeclaration>();
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

            constructors.Add(constructor);
        }

        /// <summary>
        /// Afegeix un metode.
        /// </summary>
        /// <param name="method">El metode.</param>
        /// 
        public void AddMethod(MethodDeclaration method) {

            if (method == null)
                throw new ArgumentNullException(nameof(method));

            methods.Add(method);
        }

        /// <summary>
        /// Afegeix diversos metodes.
        /// </summary>
        /// <param name="methods">Enumeracio dels metodes a afeigir.</param>
        /// 
        public void AddMethods(IEnumerable<MethodDeclaration> methods) {

            if (methods == null)
                throw new ArgumentNullException(nameof(methods));

            this.methods.AddRange(methods);
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
        public AccessSpecifier BaseAccess {
            get { return baseAccess; }
            set { baseAccess = value; }
        }
        
        /// <summary>
        /// Enumera els membres.
        /// </summary>
        /// 
        public IEnumerable<MethodDeclaration> Methods => methods;

        /// <summary>
        /// Enumera els constructors.
        /// </summary>
        /// 
        public IEnumerable<ConstructorDeclaration> Constructors => constructors;
        
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
