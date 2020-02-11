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

                if (sb == null) {
                    throw new ArgumentNullException(nameof(sb));
                }

                this.sb = sb;
                this.indent = indent;
            }

            public override void Visit(AssignStatement stmt) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} = ", stmt.Name);

                stmt.Expression.AcceptVisitor(this);

                sb.AppendLine(";");
            }

            public override void Visit(EnumeratorDeclaration decl) {

                sb.AppendIndent(indent);
                sb.Append("typedef enum {");
                indent++;
                bool first = true;
                foreach (var element in decl.Eements) {
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

            public override void Visit(FunctionCallExpression exp) {

                exp.Function.AcceptVisitor(this);
                sb.Append('(');
                sb.Append(")");
            }

            public override void Visit(FunctionCallStatement stmt) {

                if (stmt.Expression!= null) {
                    sb.AppendIndent(indent);
                    stmt.Expression.AcceptVisitor(this);
                    sb.AppendLine(";");
                }
            }

            public override void Visit(FunctionDeclaration dec) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} {1}(", dec.ReturnType.Name, dec.Name);
                if (dec.Arguments == null) {
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

                if (dec.Body != null) {
                    dec.Body.AcceptVisitor(this);
                }

                indent--;
                sb.AppendIndent(indent);
                sb.AppendLine("}");
                sb.AppendLine();
            }

            public override void Visit(IfThenElseStatement stmt) {

                sb.AppendIndent(indent);
                sb.Append("if (");
                stmt.ConditionExpression.AcceptVisitor(this);
                sb.AppendLine(") {");

                indent++;
                stmt.TrueBlock.AcceptVisitor(this);
                indent--;

                sb.AppendIndent(indent);
                sb.AppendLine("}");


                if (stmt.FalseBlock != null) {
                    sb.AppendIndent(indent);
                    sb.AppendLine("else {");

                    indent++;
                    stmt.FalseBlock.AcceptVisitor(this);
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

            public override void Visit(SwitchCaseStatement stmt) {

                sb.AppendIndent(indent);
                if (stmt.Expression == null) {
                    sb.AppendLine("default:");
                }
                else {
                    sb.Append("case ");
                    stmt.Expression.AcceptVisitor(this);
                    sb.AppendLine(":");
                }
                indent++;

                if (stmt.Body != null)
                        stmt.Body.AcceptVisitor(this);

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
                foreach (var switchCase in stmt.SwitchCases) {
                    switchCase.AcceptVisitor(this);
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
