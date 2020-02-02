namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Expressions;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Statements;

    public abstract class DefaultVisitor : IVisitor {

        public virtual void Visit(ArgumentDefinition obj) {
        }

        public virtual void Visit(AssignStatement obj) {
        }

        public virtual void Visit(BinaryExpression obj) {
        }

        public virtual void Visit(Block obj) {

            if (obj.Statements != null)
                foreach (var statements in obj.Statements)
                    statements.AcceptVisitor(this);
        }

        public virtual void Visit(ClassDeclaration obj) {

            if (obj.Variables != null)
                foreach (var variables in obj.Variables)
                    variables.AcceptVisitor(this);

            if (obj.Constructors != null)
                foreach (var constructor in obj.Constructors)
                    constructor.AcceptVisitor(this);

            if (obj.Destructor != null)
                obj.Destructor.AcceptVisitor(this);

            if (obj.Functions != null)
                foreach (var function in obj.Functions)
                    function.AcceptVisitor(this);
        }

        public virtual void Visit(ConditionalExpression obj) {
        }

        public virtual void Visit(ConstructorDeclaration obj) {
        }

        public virtual void Visit(DestructorDeclaration obj) {
        }

        public virtual void Visit(FunctionCallExpression obj) {

            obj.Function.AcceptVisitor(this);

            if (obj.Arguments != null)
                foreach (var argument in obj.Arguments)
                    argument.AcceptVisitor(this);
        }

        public virtual void Visit(FunctionCallStatement obj) {

            if (obj.Expression != null)
                obj.Expression.AcceptVisitor(this);
        }

        public virtual void Visit(IdentifierExpression obj) {
        }

        public virtual void Visit(IfThenElseStatement obj) {
        }

        public virtual void Visit(InlineExpression obj) {
        }

        public virtual void Visit(InlineStatement obj) {
        }

        public virtual void Visit(LiteralExpression obj) {
        }

        public virtual void Visit(LoopStatement obj) {
        }

        public virtual void Visit(MemberFunctionDeclaration obj) {
        }

        public virtual void Visit(MemberVariableDeclaration obj) {
        }

        public virtual void Visit(NamespaceDeclaration obj) {

            if (obj.Members != null)
                foreach (var member in obj.Members)
                    member.AcceptVisitor(this);
        }

        public virtual void Visit(ReturnStatement obj) {

            if (obj.Expression != null)
                obj.Expression.AcceptVisitor(this);
        }

        public virtual void Visit(SubscriptExpression obj) {

            obj.Address.AcceptVisitor(this);

            if (obj.Indices != null)
                foreach (var index in obj.Indices)
                    index.AcceptVisitor(this);
        }

        public virtual void Visit(UnaryExpression obj) {
        }

        public virtual void Visit(UnitDeclaration obj) {

            if (obj.Members != null)
                foreach (var member in obj.Members)
                    member.AcceptVisitor(this);
        }
    }
}
