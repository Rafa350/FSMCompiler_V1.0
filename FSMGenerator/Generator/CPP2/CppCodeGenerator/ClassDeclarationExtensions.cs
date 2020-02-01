namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeGenerator {

    using System.Collections.Generic;
    using System.Linq;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel;

    internal static class ClassDeclarationExtensions {

        public static IEnumerable<ConstructorDeclaration> GetConstructors(this ClassDeclaration classDeclaration, AccessMode access) {

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

        public static IEnumerable<MemberFunctionDeclaration> GetMemberFunctions(this ClassDeclaration classDeclaration) {

            List<MemberFunctionDeclaration> methods = null;

            if (classDeclaration.Functions != null)
                foreach (MemberFunctionDeclaration method in classDeclaration.Functions.OfType<MemberFunctionDeclaration>()) {
                    if (methods == null)
                        methods = new List<MemberFunctionDeclaration>();
                    methods.Add(method);
                }

            return methods;
        }

        public static IEnumerable<MemberFunctionDeclaration> GetMemberFunctions(this ClassDeclaration classDeclaration, AccessMode access) {

            List<MemberFunctionDeclaration> methods = null;

            if (classDeclaration.Functions != null)
                foreach (MemberFunctionDeclaration method in classDeclaration.Functions.OfType<MemberFunctionDeclaration>()) {
                    if (method.AccessMode == access) {
                        if (methods == null)
                            methods = new List<MemberFunctionDeclaration>();
                        methods.Add(method);
                    }
                }

            return methods;
        }
    }
}
