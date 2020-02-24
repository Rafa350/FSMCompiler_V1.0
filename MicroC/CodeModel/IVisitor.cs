namespace MicroCompiler.CodeModel {

    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;

    public interface IVisitor {

        void Visit(ArgumentDeclaration decl);
        void Visit(AssignStatement stmt);
        void Visit(BinaryExpression exp);
        void Visit(BlockStatement block);
        void Visit(CastExpression exp);
        void Visit(ClassDeclaration decl);
        void Visit(ConditionalExpression exp);
        void Visit(ConstructorDeclaration decl);
        void Visit(ConstructorInitializer initializer);
        void Visit(DestructorDeclaration decl);
        void Visit(EnumerationDeclaration decl);
        void Visit(ForwardClassDeclaration decl);
        void Visit(InvokeExpression exp);
        void Visit(InvokeStatement stmt);
        void Visit(FunctionDeclaration dell);
        void Visit(IdentifierExpression exp);
        void Visit(IfThenElseStatement stmt);
        void Visit(InlineExpression exp);
        void Visit(InlineStatement stmt);
        void Visit(LiteralExpression exp);
        void Visit(LoopStatement stmt);
        void Visit(NamespaceDeclaration decl);
        void Visit(NamespaceImport import);
        void Visit(ReturnStatement stmt);
        void Visit(SubscriptExpression exp);
        void Visit(CaseStatement stmt);
        void Visit(SwitchStatement stmt);
        void Visit(ThisReferenceExpression exp);
        void Visit(UnaryExpression exp);
        void Visit(UnitDeclaration exp);
        void Visit(VariableDeclaration decl);
    }
}
