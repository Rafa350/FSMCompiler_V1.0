namespace MicroCompiler.CodeModel {

    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;

    public interface IVisitor {

        void Visit(ArgumentDeclaration decl);
        void Visit(AssignStatement stmt);
        void Visit(BinaryExpression exp);
        void Visit(BlockStatement block);
        void Visit(ClassDeclaration decl);
        void Visit(ConditionalExpression exp);
        void Visit(ConstructorDeclaration decl);
        void Visit(DestructorDeclaration decl);
        void Visit(EnumeratorDeclaration decl);
        void Visit(FunctionCallExpression exp);
        void Visit(FunctionCallStatement stmt);
        void Visit(FunctionDeclaration dell);
        void Visit(IdentifierExpression exp);
        void Visit(IfThenElseStatement stmt);
        void Visit(InlineExpression exp);
        void Visit(InlineStatement stmt);
        void Visit(LiteralExpression exp);
        void Visit(LoopStatement stmt);
        void Visit(MemberFunctionDeclaration decl);
        void Visit(MemberVariableDeclaration decl);
        void Visit(NamespaceDeclaration decl);
        void Visit(ReturnStatement stmt);
        void Visit(SubscriptExpression exp);
        void Visit(SwitchCaseStatement stmt);
        void Visit(SwitchStatement stmt);
        void Visit(UnaryExpression exp);
        void Visit(UnitDeclaration exp);
        void Visit(VariableDeclaration decl);
    }
}
