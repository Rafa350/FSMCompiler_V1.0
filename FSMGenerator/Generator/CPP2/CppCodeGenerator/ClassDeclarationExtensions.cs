namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeGenerator {

    using System.Collections.Generic;
    using System.Linq;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel;

    internal static class ClassDeclarationExtensions {

        public static IEnumerable<ConstructorDeclaration> GetConstructors(this ClassDeclaration classDeclaration, AccessSpecifier access) {

            List<ConstructorDeclaration> constructors = null;

            if (classDeclaration.Constructors != null)
                foreach (var constructor in classDeclaration.Constructors) {
                    if (constructor.Access == access) {
                        if (constructors == null)
                            constructors = new List<ConstructorDeclaration>();
                        constructors.Add(constructor);
                    }
                }

            return constructors;
        }

        public static IEnumerable<MethodDeclaration> GetMethods(this ClassDeclaration classDeclaration) {

            List<MethodDeclaration> methods = null;

            if (classDeclaration.Methods != null)
                foreach (MethodDeclaration method in classDeclaration.Methods.OfType<MethodDeclaration>()) {
                    if (methods == null)
                        methods = new List<MethodDeclaration>();
                    methods.Add(method);
                }

            return methods;
        }

        public static IEnumerable<MethodDeclaration> GetMethods(this ClassDeclaration classDeclaration, AccessSpecifier access) {

            List<MethodDeclaration> methods = null;

            if (classDeclaration.Methods != null)
                foreach (MethodDeclaration method in classDeclaration.Methods.OfType<MethodDeclaration>()) {
                    if (method.Access == access) {
                        if (methods == null)
                            methods = new List<MethodDeclaration>();
                        methods.Add(method);
                    }
                }

            return methods;
        }
    }
}
