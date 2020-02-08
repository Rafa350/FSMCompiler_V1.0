namespace MicroCompiler.CodeModel {

    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;

    public abstract class DefaultVisitor : IVisitor {

        public virtual void Visit(ArgumentDefinition obj) {
        }

        public virtual void Visit(AssignStatement stmt) {
        }

        public virtual void Visit(BinaryExpression exp) {
        }

        public virtual void Visit(Block block) {

            if (block.Statements != null)
                foreach (var statements in block.Statements)
                    statements.AcceptVisitor(this);
        }

        public virtual void Visit(ClassDeclaration decl) {

            if (decl.Variables != null)
                foreach (var variables in decl.Variables)
                    variables.AcceptVisitor(this);

            if (decl.Constructors != null)
                foreach (var constructor in decl.Constructors)
                    constructor.AcceptVisitor(this);

            if (decl.Destructor != null)
                decl.Destructor.AcceptVisitor(this);

            if (decl.Functions != null)
                foreach (var function in decl.Functions)
                    function.AcceptVisitor(this);
        }

        public virtual void Visit(ConditionalExpression exp) {
        }

        public virtual void Visit(ConstructorDeclaration decl) {
        }

        public virtual void Visit(DestructorDeclaration decl) {
        }

        public virtual void Visit(FunctionCallExpression exp) {

            exp.Function.AcceptVisitor(this);

            if (exp.Arguments != null)
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

        public virtual void Visit(MemberFunctionDeclaration decl) {
        }

        public virtual void Visit(MemberVariableDeclaration decl) {
        }

        public virtual void Visit(NamespaceDeclaration decl) {

            if (decl.Members != null)
                foreach (var member in decl.Members)
                    member.AcceptVisitor(this);
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
        }

        public virtual void Visit(UnaryExpression exp) {
        }

        public virtual void Visit(UnitDeclaration decl) {

            if (decl.Members != null)
                foreach (var member in decl.Members)
                    member.AcceptVisitor(this);
        }
    }
}
