namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeGenerator {

    using System;
    using System.Text;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Statements;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Expressions;

    public sealed class CodeGenerator {

        private sealed class GeneratorVisitor : DefaultVisitor {

            private readonly StringBuilder sb;
            private int indent;
            private ClassDeclaration currentClass;

            public GeneratorVisitor(StringBuilder sb, int indent = 0) {

                if (sb == null)
                    throw new ArgumentNullException(nameof(sb));

                this.sb = sb;
                this.indent = indent;
            }

            public override void Visit(ArgumentDefinition obj) {

                sb.AppendLine();
                sb.AppendIndent(indent);
                sb.Append(obj.ValueType.Name);
                sb.Append(' ');
                sb.Append(obj.Name);
            }

            public override void Visit(Block obj) {

                if (obj.Statements != null)
                    foreach (var statement in obj.Statements)
                        statement.AcceptVisitor(this);
            }

            public override void Visit(ClassDeclaration obj) {

                ClassDeclaration oldClass = currentClass;
                currentClass = obj;

                if (obj.Constructors != null)
                    foreach (var constructor in obj.Constructors)
                        constructor.AcceptVisitor(this);

                if (obj.Functions != null)
                    foreach (var function in obj.Functions)
                        function.AcceptVisitor(this);

                currentClass = oldClass;
            }

            public override void Visit(ConstructorDeclaration obj) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0}::{0}(", currentClass.Name);

                if (obj.Arguments == null) {
                    sb.AppendLine(") {");
                    indent++;
                }
                else {
                    indent++;
                    bool first = true;
                    foreach (var argument in obj.Arguments) {
                        if (first)
                            first = false;
                        else
                            sb.Append(", ");
                        argument.AcceptVisitor(this);
                    }
                    sb.AppendLine(") {");
                }

                if (obj.Body != null)
                    obj.Body.AcceptVisitor(this);

                indent--;
                sb.AppendIndent(indent);
                sb.Append('}');
                sb.AppendLine();
                sb.AppendLine();
            }

            public override void Visit(FunctionCallExpression obj) {

                sb.Append(obj.Name);
                sb.Append("()");
            }

            public override void Visit(FunctionCallStatement obj) {

                sb.AppendIndent(indent);
                base.Visit(obj);
                sb.Append(';');
                sb.AppendLine();
            }

            public override void Visit(IdentifierExpression obj) {

                sb.Append(obj.Name);
            }

            public override void Visit(InlineExpression obj) {

                sb.Append(obj.Code);
            }

            public override void Visit(InlineStatement obj) {

                sb.AppendIndent(indent);
                sb.Append(obj.Code);
                sb.Append(';');
                sb.AppendLine();
            }

            public override void Visit(MemberVariableDeclaration obj) {

                sb.AppendIndent(indent);
                sb.AppendLine("{0} {1};", obj.ValueType.Name, obj.Name);
            }

            public override void Visit(MemberFunctionDeclaration obj) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} {1}::{2}(", obj.ReturnType.Name, currentClass.Name, obj.Name);

                if (obj.Arguments == null) {
                    sb.AppendLine(") {");
                    indent++;
                }
                else {
                    indent++;
                    bool first = true;
                    foreach (var argument in obj.Arguments) {
                        if (first)
                            first = false;
                        else
                            sb.Append(", ");
                        argument.AcceptVisitor(this);
                    }
                    sb.AppendLine(") {");
                }

                if (obj.Body != null) 
                    obj.Body.AcceptVisitor(this);

                indent--;
                sb.AppendIndent(indent);
                sb.AppendLine("}");
                sb.AppendLine();
            }

            public override void Visit(NamespaceDeclaration obj) {

                sb.AppendIndent(indent);
                sb.AppendLine("namespace {0} {{", obj.Name);
                sb.AppendLine();
                indent++;

                base.Visit(obj);

                indent--;
                sb.AppendIndent(indent);
                sb.Append('}');
                sb.AppendLine();
            }

            public override void Visit(ReturnStatement obj) {

                sb.AppendIndent(indent);
                sb.Append("return");
                if (obj.Expression != null) {
                    sb.Append(' ');
                    obj.Expression.AcceptVisitor(this);
                }
                sb.Append(';');
                sb.AppendLine();
            }
        }

        public static string Generate(UnitDeclaration unitDeclaration) {

            if (unitDeclaration == null)
                throw new ArgumentNullException(nameof(unitDeclaration));

            StringBuilder sb = new StringBuilder();
            var visitor = new GeneratorVisitor(sb);
            
            visitor.Visit(unitDeclaration);
            
            return sb.ToString();
        }
    }
}
