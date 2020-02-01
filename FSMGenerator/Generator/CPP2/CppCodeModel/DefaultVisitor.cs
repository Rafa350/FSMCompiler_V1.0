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
        }

        public virtual void Visit(ConditionalExpression obj) {
        }

        public virtual void Visit(ConstructorDeclaration obj) {
        }

        public virtual void Visit(DestructorDeclaration obj) {
        }

        public virtual void Visit(FunctionCallExpression obj) {
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

        public virtual void Visit(UnaryExpression obj) {
        }

        public virtual void Visit(UnitDeclaration obj) {

            if (obj.Members != null)
                foreach (var member in obj.Members)
                    member.AcceptVisitor(this);
        }
    }
}
