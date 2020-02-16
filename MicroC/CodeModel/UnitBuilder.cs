namespace MicroCompiler.CodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class UnitBuilder {

        private readonly Stack<DeclarationBlockMemberList> unitMemberStack = new Stack<DeclarationBlockMemberList>();
        private readonly Stack<ClassDeclaration> classDeclStack = new Stack<ClassDeclaration>();
        private readonly UnitDeclaration unitDecl;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        public UnitBuilder() {

            unitDecl = new UnitDeclaration(new DeclarationBlockMemberList());
            unitMemberStack.Push(unitDecl.Members);
        }

        /// <summary>
        /// Inicia la declaracio de'un espai de noms.
        /// </summary>
        /// <param name="name">El nom del espai.</param>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder BeginNamespace(string name) {

            NamespaceDeclaration namespaceDecl = new NamespaceDeclaration(name, new DeclarationBlockMemberList());
            unitMemberStack.Peek().Add(namespaceDecl);
            unitMemberStack.Push(namespaceDecl.Members);

            return this;
        }

        /// <summary>
        /// Finalitza la declaracio d'un espai de noms.
        /// </summary>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder EndNamespace() {

            if (unitMemberStack.Count < 2)
                throw new InvalidOperationException("No hay ningun espacio de nombres abierto.");

            unitMemberStack.Pop();

            return this;
        }

        /// <summary>
        /// Inicia la declaracio d'una clase.
        /// </summary>
        /// <param name="name">El nom de la clase.</param>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder BeginClass(string name) {

            ClassDeclaration classDecl = new ClassDeclaration {
                Name = name,
            };

            unitMemberStack.Peek().Add(classDecl);
            classDeclStack.Push(classDecl);

            return this;
        }

        /// <summary>
        /// Inicia la declaracio d'una clase.
        /// </summary>
        /// <param name="name">El nom de la clase.</param>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder BeginClass(string name, string baseName, AccessMode baseAccess) {

            ClassDeclaration classDecl = new ClassDeclaration {
                Name = name,
                BaseName = baseName,
                BaseAccess = baseAccess
            };

            unitMemberStack.Peek().Add(classDecl);
            classDeclStack.Push(classDecl);

            return this;
        }

        /// <summary>
        /// Finalitza la declaracio de la clase.
        /// </summary>
        /// 
        public UnitBuilder EndClass() {

            if (classDeclStack.Count == 0)
                throw new InvalidOperationException("No hay ninguna declaracion de clase abierta.");

            classDeclStack.Pop();

            return this;
        }

        public UnitBuilder AddConstructorDeclaration(ConstructorDeclaration constructorDecl) {

            if (classDeclStack.Count == 0)
                throw new InvalidOperationException("No hay ninguna declaracion de clase abierta.");

            ClassDeclaration classDecl = classDeclStack.Peek();
            if (classDecl.Constructors == null)
                classDecl.Constructors = new ConstructorDeclarationList();
            classDecl.Constructors.Add(constructorDecl);

            return this;
        }

        /// <summary>
        /// Afegeix una declaracio de funcio a la clase oberta.
        /// </summary>
        /// <param name="functionDecl">La declaracio de la funcio.</param>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder AddMemberFunctionDeclaration(MemberFunctionDeclaration functionDecl) {

            if (classDeclStack.Count == 0)
                throw new InvalidOperationException("No hay ninguna declaracion de clase abierta.");

            ClassDeclaration classDecl = classDeclStack.Peek();
            if (classDecl.Functions == null)
                classDecl.Functions = new MemberFunctionDeclarationList();
            classDecl.Functions.Add(functionDecl);

            return this;
        }

        /// <summary>
        /// Afegeix una declaracio de variable a la clase oberta.
        /// </summary>
        /// <param name="variableDecl">La declaracio de la variable.</param>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder AddMemberVariableDeclaration(MemberVariableDeclaration variableDecl) {

            if (classDeclStack.Count == 0)
                throw new InvalidOperationException("No hay ninguna declaracion de clase abierta.");

            ClassDeclaration classDecl = classDeclStack.Peek();
            if (classDecl.Variables == null)
                classDecl.Variables = new MemberVariableDeclarationList();
            classDecl.Variables.Add(variableDecl);

            return this;
        }

        /// <summary>
        /// Afegeix una declaracio de variable a la unit o espai de noms obert.
        /// </summary>
        /// <param name="variableDecl">La declaracio de la variable.</param>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder AddVariableDeclaration(VariableDeclaration variableDecl) {

            unitMemberStack.Peek().Add(variableDecl);

            return this;
        }

        /// <summary>
        /// Afegeix una declaracio de funcio a la unit o al espai de noms obert.
        /// </summary>
        /// <param name="functionDecl">La declaracio de la funcio.</param>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder AddFunctionDeclaration(FunctionDeclaration functionDecl) {

            unitMemberStack.Peek().Add(functionDecl);

            return this;
        }

        /// <summary>
        /// Obte la unitat que s'ha construit.
        /// </summary>
        /// <returns>La declaracio de la unit.</returns>
        /// 
        public UnitDeclaration ToUnit() {

            return unitDecl;
        }
    }
}
