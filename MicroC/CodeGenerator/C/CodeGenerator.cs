namespace MicroCompiler.CodeGenerator.C {

    using System;
    using System.Text;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;

    public static class CodeGenerator {

        private class GeneratorVisitor : DefaultVisitor {

            private readonly StringBuilder sb;
            private int indent;

            public GeneratorVisitor(StringBuilder sb, int indent = 0) {

                this.sb = sb ?? throw new ArgumentNullException(nameof(sb));
                this.indent = indent;
            }

            public override void Visit(AssignStatement stmt) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} = ", stmt.Name);

                stmt.ValueExp.AcceptVisitor(this);

                sb.AppendLine(";");
            }

            public override void Visit(EnumerationDeclaration decl) {

                sb.AppendIndent(indent);
                sb.Append("typedef enum {");
                indent++;
                bool first = true;
                foreach (var element in decl.Elements) {
                    if (first)
                        first = false;
                    else
                        sb.Append(',');
                    sb.AppendLine();
                    sb.AppendIndent(indent);
                    sb.Append(element);
                }
                sb.AppendLine();
                indent--;
                sb.AppendFormat("}} {0};", decl.Name);
                sb.AppendLine();
                sb.AppendLine();
            }

            public override void Visit(InvokeExpression exp) {

                exp.AddressEpr.AcceptVisitor(this);
                sb.Append('(');
                sb.Append(")");
            }

            public override void Visit(InvokeStatement stmt) {

                if (stmt.InvokeExp != null) {
                    sb.AppendIndent(indent);
                    stmt.InvokeExp.AcceptVisitor(this);
                    sb.AppendLine(";");
                }
            }

            public override void Visit(FunctionDeclaration dec) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} {1}(", dec.ReturnType.Name, dec.Name);
                if (!dec.HasArguments) {
                    sb.AppendLine("void) {");
                    indent++;
                }
                else {
                    indent++;
                    bool first = true;
                    foreach (var argument in dec.Arguments) {
                        if (first) {
                            first = false;
                        }
                        else {
                            sb.Append(",");
                            sb.AppendLine();
                        }
                        sb.AppendIndent(indent);
                        argument.AcceptVisitor(this);
                    }
                    sb.AppendLine(") {");
                }
                sb.AppendLine();

                if (dec.Body != null) 
                    dec.Body.AcceptVisitor(this);

                indent--;
                sb.AppendIndent(indent);
                sb.AppendLine("}");
                sb.AppendLine();
            }

            public override void Visit(IfThenElseStatement stmt) {

                sb.AppendIndent(indent);
                sb.Append("if (");
                stmt.ConditionExp.AcceptVisitor(this);
                sb.AppendLine(") {");

                indent++;
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

            public override void Visit(IdentifierExpression exp) {

                sb.Append(exp.Name);
            }

            public override void Visit(InlineExpression exp) {

                sb.Append(exp.Code);
            }

            public override void Visit(InlineStatement stmt) {

                sb.AppendIndent(indent);
                sb.Append(stmt.Code);
                sb.AppendLine(";");
            }

            public override void Visit(LiteralExpression exp) {

                sb.Append(exp.Value);
            }

            public override void Visit(CaseStatement stmt) {

                sb.AppendIndent(indent);

                sb.Append("case ");
                stmt.Expression.AcceptVisitor(this);
                sb.AppendLine(":");

                indent++;

                if (stmt.Stmt != null)
                    stmt.Stmt.AcceptVisitor(this);

                sb.AppendIndent(indent);
                sb.AppendLine("break;");

                indent--;

                sb.AppendLine();
            }

            public override void Visit(SwitchStatement stmt) {

                sb.AppendIndent(indent);
                sb.Append("switch (");
                stmt.Expression.AcceptVisitor(this);
                sb.AppendLine(") {");

                indent++;
                foreach (var switchCase in stmt.Cases)
                    switchCase.AcceptVisitor(this);

                if (stmt.DefaultCaseStmt != null) {
                    sb.AppendIndent(indent);
                    sb.AppendLine("default:");
                    indent++;
                    stmt.DefaultCaseStmt.AcceptVisitor(this);
                    sb.AppendIndent(indent);
                    sb.AppendLine("break;");
                    indent--;
                }

                indent--;

                sb.AppendIndent(indent);
                sb.AppendLine("}");
            }

            public override void Visit(VariableDeclaration decl) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} {1}", decl.ValueType.Name, decl.Name);
                if (decl.Initializer != null) {

                }
                sb.AppendLine(";");
                sb.AppendLine();
            }
        }

        public static string Generate(UnitDeclaration unitDeclaration) {

            if (unitDeclaration == null) {
                throw new ArgumentNullException(nameof(unitDeclaration));
            }

            StringBuilder sb = new StringBuilder();

            var visitor = new GeneratorVisitor(sb, 0);
            visitor.Visit(unitDeclaration);

            return sb.ToString();
        }
    }
}
