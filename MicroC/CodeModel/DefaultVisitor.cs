namespace MicroCompiler.CodeModel {

    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;

    public abstract class DefaultVisitor : IVisitor {

        public virtual void Visit(ArgumentDeclaration obj) {
        }

        public virtual void Visit(AssignStatement stmt) {
        }

        public virtual void Visit(BinaryExpression exp) {
        }

        public virtual void Visit(BlockStatement block) {

            if (block.HasStatements)
                foreach (var statements in block.Statements)
                    statements.AcceptVisitor(this);
        }

        public virtual void Visit(ClassDeclaration decl) {

            if (decl.HasMembers)
                foreach (var member in decl.Members)
                    member.AcceptVisitor(this);
        }

        public virtual void Visit(ConditionalExpression exp) {
        }

        public virtual void Visit(ConstructorDeclaration decl) {
        }

        public virtual void Visit(ConstructorInitializer initializer) {
        }

        public virtual void Visit(DestructorDeclaration decl) {
        }

        public virtual void Visit(EnumerationDeclaration decl) {
        }

        public virtual void Visit(ForwardClassDeclaration decl) {
        }

        public virtual void Visit(FunctionCallExpression exp) {

            exp.Function.AcceptVisitor(this);

            if (exp.HasArguments)
                foreach (var argument in exp.Arguments)
                    argument.AcceptVisitor(this);
        }

        public virtual void Visit(FunctionCallStatement stmt) {

            if (stmt.Expression != null)
                stmt.Expression.AcceptVisitor(this);
        }

        public virtual void Visit(FunctionDeclaration decl) {
        }

        public virtual void Visit(IdentifierExpression decl) {
        }

        public virtual void Visit(IfThenElseStatement stmt) {
        }

        public virtual void Visit(InlineExpression exp) {
        }

        public virtual void Visit(InlineStatement stmt) {
        }

        public virtual void Visit(LiteralExpression exp) {
        }

        public virtual void Visit(LoopStatement stmt) {
        }

        public virtual void Visit(NamespaceDeclaration decl) {

            if (decl.HasImports)
                foreach (var import in decl.Imports)
                    import.AcceptVisitor(this);

            if (decl.HasNamespaces)
                foreach (var ns in decl.Namespaces)
                    ns.AcceptVisitor(this);

            if (decl.HasMembers)
                foreach (var member in decl.Members)
                    member.AcceptVisitor(this);
        }

        public virtual void Visit(NamespaceImport import) {
        }

        public virtual void Visit(ReturnStatement stmt) {

            if (stmt.Expression != null)
                stmt.Expression.AcceptVisitor(this);
        }

        public virtual void Visit(SubscriptExpression exp) {

            exp.Address.AcceptVisitor(this);

            if (exp.Indices != null)
                foreach (var index in exp.Indices)
                    index.AcceptVisitor(this);
        }

        public virtual void Visit(SwitchCaseStatement stmt) {

        }

        public virtual void Visit(SwitchStatement stmt) {

            stmt.Expression.AcceptVisitor(this);

            if (stmt.SwitchCases != null)
                foreach (var switchCase in stmt.SwitchCases)
                    switchCase.AcceptVisitor(this);

            if (stmt.DefaultBody != null)
                stmt.DefaultBody.AcceptVisitor(this);
        }

        public virtual void Visit(UnaryExpression exp) {
        }

        public virtual void Visit(UnitDeclaration decl) {

            if (decl.Namespace != null)
                decl.Namespace.AcceptVisitor(this);
        }

        public virtual void Visit(VariableDeclaration decl) {
        }
    }
}
