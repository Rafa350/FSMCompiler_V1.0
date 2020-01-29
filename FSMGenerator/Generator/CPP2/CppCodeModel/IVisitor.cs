namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    public interface IVisitor {

        void Visit(ArgumentDefinition obj);
        void Visit(ClassDeclaration obj);
        void Visit(ConstructorDeclaration obj);
        void Visit(DestructorDeclaration obj);
        void Visit(FieldDeclaration obj);
        void Visit(MethodDeclaration obj);
        void Visit(NamespaceDeclaration obj);
        void Visit(UnitDeclaration obj);
    }
}
