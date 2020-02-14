namespace MicroCompiler.CodeGenerator.Cpp {

    using System;
    using System.Text;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;

    public sealed class CodeGenerator {

        private sealed class GeneratorVisitor : DefaultVisitor {

            private readonly StringBuilder sb;
            private int indent;
            private ClassDeclaration currentClass;

            public GeneratorVisitor(StringBuilder sb, int indent = 0) {
                this.sb = sb ?? throw new ArgumentNullException(nameof(sb));
                this.indent = indent;
            }

            public override void Visit(ArgumentDeclaration obj) {

                sb.AppendLine();
                sb.AppendIndent(indent);
                sb.Append(obj.ValueType.Name);
                sb.Append(' ');
                sb.Append(obj.Name);
            }

            public override void Visit(ClassDeclaration obj) {

                ClassDeclaration oldClass = currentClass;
                currentClass = obj;

                if (obj.Variables != null) {
                    foreach (var variable in obj.Variables) {
                        variable.AcceptVisitor(this);
                    }
                }

                if (obj.Constructors != null) {
                    foreach (var constructor in obj.Constructors) {
                        constructor.AcceptVisitor(this);
                    }
                }

                if (obj.Functions != null) {
                    foreach (var function in obj.Functions) {
                        function.AcceptVisitor(this);
                    }
                }

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
                        if (first) {
                            first = false;
                        }
                        else {
                            sb.Append(", ");
                        }

                        argument.AcceptVisitor(this);
                    }
                    sb.AppendLine(") {");
                }

                if (obj.Body != null) {
                    obj.Body.AcceptVisitor(this);
                }

                indent--;
                sb.AppendIndent(indent);
                sb.Append('}');
                sb.AppendLine();
                sb.AppendLine();
            }

            public override void Visit(FunctionCallExpression obj) {

                obj.Function.AcceptVisitor(this);
                sb.Append('(');
                if (obj.Arguments != null) {
                    foreach (var argument in obj.Arguments) {
                        argument.AcceptVisitor(this);
                    }
                }

                sb.Append(')');
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

            public override void Visit(IfThenElseStatement obj) {

                sb.AppendIndent(indent);
                sb.Append("if (");
                if (obj.ConditionExpression == null) {
                    throw new InvalidOperationException("No se especifico la condicion.");
                }

                obj.ConditionExpression.AcceptVisitor(this);
                sb.AppendLine(") {");
                indent++;
                if (obj.TrueBlock == null) {
                    throw new InvalidOperationException("No se especificoel bloque true.");
                }

                obj.TrueBlock.AcceptVisitor(this);
                indent--;
                sb.AppendIndent(indent);
                sb.AppendLine("}");
                if (obj.FalseBlock != null) {
                    sb.AppendIndent(indent);
                    sb.AppendLine("else {");
                    indent++;
                    obj.FalseBlock.AcceptVisitor(this);
                    indent--;
                    sb.AppendIndent(indent);
                    sb.AppendLine("}");
                }
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

            public override void Visit(LiteralExpression obj) {

                sb.Append(obj.Value);
            }

            public override void Visit(MemberVariableDeclaration obj) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} {1}::{2}", obj.ValueType.Name, currentClass.Name, obj.Name);
                if (obj.Initializer != null) {
                    sb.Append(" = ");
                    obj.Initializer.AcceptVisitor(this);
                }
                sb.Append(';');
                sb.AppendLine();
                sb.AppendLine();
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
                        if (first) {
                            first = false;
                        }
                        else {
                            sb.Append(", ");
                        }

                        argument.AcceptVisitor(this);
                    }
                    sb.AppendLine(") {");
                }

                if (obj.Body != null) {
                    obj.Body.AcceptVisitor(this);
                }

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

            if (unitDeclaration == null) {
                throw new ArgumentNullException(nameof(unitDeclaration));
            }

            StringBuilder sb = new StringBuilder();
            var visitor = new GeneratorVisitor(sb);

            visitor.Visit(unitDeclaration);

            return sb.ToString();
        }
    }
}
