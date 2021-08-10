namespace MicroCompiler.CodeGenerator.C {

    using System;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;

    public static class CodeGenerator {

        private class GeneratorVisitor : DefaultVisitor {

            private readonly CodeBuilder cb;

            public GeneratorVisitor(CodeBuilder cb) {

                this.cb = cb ?? throw new ArgumentNullException(nameof(cb));
            }

            public override void Visit(AssignStatement stmt) {

                cb.WriteIndent();
                cb.Write("{0} = ", stmt.Name);

                stmt.ValueExp.AcceptVisitor(this);

                cb.Write(";");
                cb.WriteLine();
            }

            public override void Visit(EnumerationDeclaration decl) {

                cb.WriteIndent();
                cb.Write("typedef enum {");
                cb.Indent();
                bool first = true;
                foreach (var element in decl.Elements) {
                    if (first)
                        first = false;
                    else
                        cb.Write(',');
                    cb.WriteLine();
                    cb.WriteIndent();
                    cb.Write(element);
                }
                cb.WriteLine();
                cb.Unindent();
                cb.Write("}} {0};", decl.Name);
                cb.WriteLine();
                cb.WriteLine();
            }

            public override void Visit(InvokeExpression exp) {

                exp.AddressEpr.AcceptVisitor(this);
                cb.Write('(');
                cb.Write(")");
            }

            public override void Visit(InvokeStatement stmt) {

                if (stmt.InvokeExp != null) {
                    cb.WriteIndent();
                    stmt.InvokeExp.AcceptVisitor(this);
                    cb.Write(";");
                    cb.WriteLine();
                }
            }

            public override void Visit(FunctionDeclaration decl) {

                cb.WriteIndent();
                if ((decl.Access != AccessSpecifier.Default) &&
                    (decl.Access != AccessSpecifier.Public))
                    cb.Write("static ");
                cb.Write("{0} {1}(", decl.ReturnType.Name, decl.Name);
                if (!decl.HasArguments) {
                    cb.WriteLine("void) {");
                    cb.Indent();
                }
                else {
                    cb.Indent();
                    bool first = true;
                    foreach (var argument in decl.Arguments) {
                        if (first) {
                            first = false;
                        }
                        else {
                            cb.Write(",");
                            cb.WriteLine();
                        }
                        cb.WriteIndent();
                        argument.AcceptVisitor(this);
                    }
                    cb.WriteLine(") {");
                }
                cb.WriteLine();

                if (decl.Body != null) 
                    decl.Body.AcceptVisitor(this);

                cb.Unindent();
                cb.WriteIndent();
                cb.WriteLine("}");
                cb.WriteLine();
            }

            public override void Visit(IfThenElseStatement stmt) {

                cb.WriteIndent();
                cb.Write("if (");
                stmt.ConditionExp.AcceptVisitor(this);
                cb.Write(") {");
                cb.WriteLine();

                cb.Indent();
                stmt.TrueStmt.AcceptVisitor(this);
                cb.Unindent();

                cb.WriteLine('}');


                if (stmt.FalseStmt != null) {
                    cb.WriteLine("else {");

                    cb.Indent();
                    stmt.FalseStmt.AcceptVisitor(this);
                    cb.Unindent();

                    cb.WriteLine("}");
                }
            }

            public override void Visit(IdentifierExpression exp) {

                cb.Write(exp.Name);
            }

            public override void Visit(InlineExpression exp) {

                cb.Write(exp.Code);
            }

            public override void Visit(InlineStatement stmt) {

                cb.WriteIndent();
                cb.Write(stmt.Code);
                cb.Write(";");
                cb.WriteLine();
            }

            public override void Visit(LiteralExpression exp) {

                if (exp.Value is Boolean) 
                    cb.Write((bool) exp.Value ? "true" : "false");

                else
                    cb.Write(exp.Value.ToString());
            }

            public override void Visit(CaseStatement stmt) {

                cb.WriteIndent();
                cb.Write("case ");
                stmt.Expression.AcceptVisitor(this);
                cb.Write(":");
                cb.WriteLine();

                cb.Indent();

                if (stmt.Stmt != null)
                    stmt.Stmt.AcceptVisitor(this);

                cb.WriteLine("break;");

                cb.Unindent();

                cb.WriteLine();
            }

            public override void Visit(ReturnStatement stmt) {

                cb.WriteIndent();
                cb.Write("return");
                if (stmt.ValueExp != null)
                    stmt.ValueExp.AcceptVisitor(this);
                cb.Write(';');
                cb.WriteLine();
            }

            public override void Visit(SwitchStatement stmt) {

                cb.WriteIndent();
                cb.Write("switch (");
                stmt.Expression.AcceptVisitor(this);
                cb.Write(") {");
                cb.WriteLine();

                cb.Indent();
                foreach (var switchCase in stmt.Cases)
                    switchCase.AcceptVisitor(this);

                if (stmt.DefaultCaseStmt != null) {
                    cb.WriteLine("default:");
                    cb.Indent();
                    stmt.DefaultCaseStmt.AcceptVisitor(this);
                    cb.WriteLine("break;");
                    cb.Unindent();
                }

                cb.Unindent();
                cb.WriteLine("}");
            }

            public override void Visit(VariableDeclaration decl) {

                cb.WriteIndent();
                if ((decl.Access != AccessSpecifier.Default) &&
                    (decl.Access != AccessSpecifier.Public))
                    cb.Write("static ");
                cb.Write("{0} {1}", decl.ValueType.Name, decl.Name);
                if (decl.Initializer != null) {
                    cb.Write(" = ");
                    decl.Initializer.AcceptVisitor(this);
                }
                cb.WriteLine(";");
                cb.WriteLine();
            }
        }

        public static string Generate(UnitDeclaration unitDecl) {

            if (unitDecl == null) 
                throw new ArgumentNullException(nameof(unitDecl));

            var cb = new CodeBuilder();
            var visitor = new GeneratorVisitor(cb);

            visitor.Visit(unitDecl);

            return cb.ToString();
        }
    }
}
