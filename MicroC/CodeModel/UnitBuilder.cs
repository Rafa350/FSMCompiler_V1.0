namespace MicroCompiler.CodeModel {

    using System;
    using System.Collections.Generic;

    public sealed class UnitBuilder {

        private readonly Stack<NamespaceDeclaration> namespaceDeclStack = new Stack<NamespaceDeclaration>();
        private readonly Stack<ClassDeclaration> classDeclStack = new Stack<ClassDeclaration>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        public UnitBuilder() {

            // Crea l'espai de noms global.
            //
            namespaceDeclStack.Push(new NamespaceDeclaration("::", null, null, new NamespaceMemberList()));
        }

        /// <summary>
        /// Inicia la declaracio de'un espai de noms.
        /// </summary>
        /// <param name="name">El nom del espai.</param>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder BeginNamespace(string name) {

            NamespaceDeclaration namespaceDecl = new NamespaceDeclaration(name, null, null, new NamespaceMemberList());
            namespaceDeclStack.Peek().Namespaces.Add(namespaceDecl);
            namespaceDeclStack.Push(namespaceDecl);

            return this;
        }

        /// <summary>
        /// Finalitza la declaracio d'un espai de noms.
        /// </summary>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder EndNamespace() {

            // No permet tancar l'espai de noms global.
            //
            if (namespaceDeclStack.Count < 2)
                throw new InvalidOperationException("No hay ningun espacio de nombres abierto.");

            namespaceDeclStack.Pop();

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

            namespaceDeclStack.Peek().Members.Add(classDecl);
            classDeclStack.Push(classDecl);

            return this;
        }

        /// <summary>
        /// Inicia la declaracio d'una clase.
        /// </summary>
        /// <param name="name">El nom de la clase.</param>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder BeginClass(string name, string baseName, AccessSpecifier baseAccess) {

            ClassDeclaration classDecl = new ClassDeclaration {
                Name = name,
                BaseName = baseName,
                BaseAccess = baseAccess
            };

            namespaceDeclStack.Peek().Members.Add(classDecl);
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

        public UnitBuilder AddForwardClassDeclaration(string name) {

            namespaceDeclStack.Peek().Members.Add(new ForwardClassDeclaration(name));

            return this;
        }

        public UnitBuilder AddMemberDeclaration(IClassMember memberDecl) {

            if (classDeclStack.Count == 0)
                throw new InvalidOperationException("No hay ninguna declaracion de clase abierta.");

            ClassDeclaration classDecl = classDeclStack.Peek();
            classDecl.Members.Add(memberDecl);

            return this;
        }

        /// <summary>
        /// Afegeix una declaracio de variable a la unit o espai de noms obert.
        /// </summary>
        /// <param name="variableDecl">La declaracio de la variable.</param>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder AddVariableDeclaration(VariableDeclaration variableDecl) {

            namespaceDeclStack.Peek().Members.Add(variableDecl);

            return this;
        }

        /// <summary>
        /// Afegeix una declaracio de funcio a la unit o al espai de noms obert.
        /// </summary>
        /// <param name="functionDecl">La declaracio de la funcio.</param>
        /// <returns>L'objecte this.</returns>
        /// 
        public UnitBuilder AddFunctionDeclaration(FunctionDeclaration functionDecl) {

            namespaceDeclStack.Peek().Members.Add(functionDecl);

            return this;
        }

        /// <summary>
        /// Obte la unitat que s'ha construit.
        /// </summary>
        /// <returns>La declaracio de la unit.</returns>
        /// 
        public UnitDeclaration ToUnit() {

            return new UnitDeclaration(namespaceDeclStack.Peek());
        }
    }
}
