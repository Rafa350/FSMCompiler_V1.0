namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class ClassDeclaration: IVisitable, INamespaceMember, IUnitMember {

        private readonly string name;
        private readonly string baseName;
        private readonly AccessSpecifier baseAccess;
        private readonly IEnumerable<IClassMember> members;
        private readonly IEnumerable<ConstructorDeclaration> constructors;
        private readonly DestructorDeclaration destructor;

        public ClassDeclaration(string name, IEnumerable<IClassMember> members = null, IEnumerable<ConstructorDeclaration> constructors = null, DestructorDeclaration destructor = null) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            this.members = members;
            this.constructors = constructors;
            this.destructor = destructor;
        }

        public ClassDeclaration(string name, string baseName, AccessSpecifier baseAccess, IEnumerable<IClassMember> members = null, IEnumerable<ConstructorDeclaration> constructors = null, DestructorDeclaration destructor = null) {

            this.name = name;
            this.baseName = baseName;
            this.baseAccess = baseAccess;
            this.members = members;
            this.constructors = constructors;
            this.destructor = destructor;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Name => name;
        public string BaseName => baseName;
        public AccessSpecifier BaseAccess => baseAccess;
        public IEnumerable<IClassMember> Members => members;
        public IEnumerable<ConstructorDeclaration> Constructors => constructors;
        public DestructorDeclaration Destructor => destructor;
    }
}
