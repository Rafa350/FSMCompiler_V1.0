namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Expressions;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Statements;

    public interface IVisitor {

        void Visit(ArgumentDefinition obj);
        void Visit(AssignStatement obj);
        void Visit(BinaryExpression obj);
        void Visit(Block obj);
        void Visit(ClassDeclaration obj);
        void Visit(ConditionalExpression obj);
        void Visit(ConstructorDeclaration obj);
        void Visit(DestructorDeclaration obj);
        void Visit(FunctionCallExpression obj);
        void Visit(FunctionCallStatement obj);
        void Visit(IdentifierExpression obj);
        void Visit(IfThenElseStatement obj);
        void Visit(InlineExpression obj);
        void Visit(InlineStatement obj);
        void Visit(LiteralExpression obj);
        void Visit(LoopStatement obj);
        void Visit(MemberFunctionDeclaration obj);
        void Visit(MemberVariableDeclaration obj);
        void Visit(NamespaceDeclaration obj);
        void Visit(ReturnStatement obj);
        void Visit(SubscriptExpression obj);
        void Visit(UnaryExpression obj);
        void Visit(UnitDeclaration obj);
    }
}
