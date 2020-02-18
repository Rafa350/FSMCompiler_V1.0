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

            public override void Visit(ArgumentDeclaration decl) {

                sb.AppendLine();
                sb.AppendIndent(indent);
                sb.Append(decl.ValueType.Name);
                sb.Append(' ');
                sb.Append(decl.Name);
            }

            public override void Visit(ClassDeclaration decl) {

                ClassDeclaration oldClass = currentClass;
                currentClass = decl;

                if (decl.Variables != null)
                    foreach (var variable in decl.Variables)
                        variable.AcceptVisitor(this);

                if (decl.Constructors != null)
                    foreach (var constructor in decl.Constructors)
                        constructor.AcceptVisitor(this);

                if (decl.Functions != null)
                    foreach (var function in decl.Functions)
                        function.AcceptVisitor(this);

                currentClass = oldClass;
            }

            public override void Visit(ConstructorDeclaration decl) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0}::{0}(", currentClass.Name);

                // Genera la llista d'arguments
                //
                if (decl.Arguments == null) {
                    sb.AppendLine(") {");
                    indent++;
                }
                else {
                    indent++;
                    bool first = true;
                    foreach (var argument in decl.Arguments) {
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

                // Genera la llista de contructors
                //

                // Genera el cos de la funcio
                //
                if (decl.Body != null)
                    decl.Body.AcceptVisitor(this);

                indent--;
                sb.AppendIndent(indent);
                sb.Append('}');
                sb.AppendLine();
                sb.AppendLine();
            }

            public override void Visit(ForwardClassDeclaration decl) {

                sb.AppendIndent(indent);
                sb.AppendFormat("class {0};", decl.Name);
                sb.AppendLine();
                sb.AppendLine();
            }

            public override void Visit(FunctionCallExpression exp) {

                exp.Function.AcceptVisitor(this);
                sb.Append('(');
                if (exp.Arguments != null) {
                    foreach (var argument in exp.Arguments) {
                        argument.AcceptVisitor(this);
                    }
                }

                sb.Append(')');
            }

            public override void Visit(FunctionCallStatement stmt) {

                sb.AppendIndent(indent);
                base.Visit(stmt);
                sb.Append(';');
                sb.AppendLine();
            }

            public override void Visit(IdentifierExpression obj) {

                sb.Append(obj.Name);
            }

            public override void Visit(IfThenElseStatement stmt) {

                sb.AppendIndent(indent);
                sb.Append("if (");
                if (stmt.ConditionExpression == null)
                    throw new InvalidOperationException("No se especifico la condicion.");

                stmt.ConditionExpression.AcceptVisitor(this);
                sb.AppendLine(") {");
                indent++;
                if (stmt.TrueStmt == null)
                    throw new InvalidOperationException("No se especificoel bloque true.");

                stmt.TrueStmt.AcceptVisitor(this);
                indent--;
                sb.AppendIndent(indent);
                sb.AppendLine("}");
                if (stmt.FalseStmt != null) {
                    sb.AppendIndent(indent);
                    sb.AppendLine("else {");
                    indent++;
                    stmt.FalseStmt.AcceptVisitor(this);
                    indent--;
                    sb.AppendIndent(indent);
                    sb.AppendLine("}");
                }
            }

            public override void Visit(InlineExpression obj) {

                sb.Append(obj.Code);
            }

            public override void Visit(InlineStatement stmt) {

                sb.AppendIndent(indent);
                sb.Append(stmt.Code);
                sb.Append(';');
                sb.AppendLine();
            }

            public override void Visit(LiteralExpression obj) {

                sb.Append(obj.Value);
            }

            public override void Visit(MemberVariableDeclaration decl) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} {1}::{2}", decl.ValueType.Name, currentClass.Name, decl.Name);
                if (decl.Initializer != null) {
                    sb.Append(" = ");
                    decl.Initializer.AcceptVisitor(this);
                }
                sb.Append(';');
                sb.AppendLine();
                sb.AppendLine();
            }

            public override void Visit(MemberFunctionDeclaration decl) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} {1}::{2}(", decl.ReturnType.Name, currentClass.Name, decl.Name);

                if (decl.Arguments == null) {
                    sb.AppendLine(") {");
                    indent++;
                }
                else {
                    indent++;
                    bool first = true;
                    foreach (var argument in decl.Arguments) {
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

                if (decl.Body != null) {
                    decl.Body.AcceptVisitor(this);
                }

                indent--;
                sb.AppendIndent(indent);
                sb.AppendLine("}");
                sb.AppendLine();
            }

            public override void Visit(NamespaceDeclaration decl) {

                sb.AppendIndent(indent);
                sb.AppendLine("namespace {0} {{", decl.Name);
                sb.AppendLine();
                indent++;

                base.Visit(decl);

                indent--;
                sb.AppendIndent(indent);
                sb.Append('}');
                sb.AppendLine();
            }

            public override void Visit(ReturnStatement stmt) {

                sb.AppendIndent(indent);
                sb.Append("return");
                if (stmt.Expression != null) {
                    sb.Append(' ');
                    stmt.Expression.AcceptVisitor(this);
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
