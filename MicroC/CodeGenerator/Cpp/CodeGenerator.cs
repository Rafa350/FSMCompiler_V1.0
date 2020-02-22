namespace MicroCompiler.CodeGenerator.Cpp {

    using System;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;

    public sealed class CodeGenerator {

        private sealed class GeneratorVisitor : DefaultVisitor {

            private readonly CodeBuilder cb;
            private ClassDeclaration currentClass;

            public GeneratorVisitor(CodeBuilder cb) {

                this.cb = cb ?? throw new ArgumentNullException(nameof(cb));
            }

            public override void Visit(ArgumentDeclaration decl) {

                cb.WriteLine();
                cb.WriteIndent();
                cb.Write(decl.ValueType.Name);
                cb.Write(' ');
                cb.Write(decl.Name);
            }

            public override void Visit(ClassDeclaration decl) {

                ClassDeclaration oldClass = currentClass;
                currentClass = decl;

                if (decl.HasMembers)
                    foreach (var member in decl.Members)
                        member.AcceptVisitor(this);

                currentClass = oldClass;
            }

            public override void Visit(ConstructorDeclaration decl) {

                cb.WriteIndent();
                cb.Write("{0}::{0}(", currentClass.Name);

                // Genera la llista d'arguments
                //
                if (decl.HasArguments) {
                    cb.Indent();
                    bool first = true;
                    foreach (var argument in decl.Arguments) {
                        if (first)
                            first = false;
                        else
                            cb.Write(", ");
                        argument.AcceptVisitor(this);
                    }
                    cb.Write(')');
                }
                else {
                    cb.Write(')');
                    cb.Indent();
                }
                if (decl.HasInitializers)
                    cb.Write(" :");
                else
                    cb.Write(" {");
                cb.WriteLine();

                // Genera la llistad'inicialitzadors
                //
                if (decl.HasInitializers) {
                    bool first = true;
                    foreach (var initializer in decl.Initializers) {
                        if (first)
                            first = false;
                        else
                            cb.WriteLine(", ");
                        cb.WriteIndent();
                        initializer.AcceptVisitor(this);
                    }
                    cb.Write(" {");
                    cb.WriteLine();
                }

                // Genera el cos de la funcio
                //
                if (decl.Body != null)
                    decl.Body.AcceptVisitor(this);

                cb.Unindent();
                cb.WriteIndent();
                cb.Write('}');
                cb.WriteLine();
                cb.WriteLine();
            }

            public override void Visit(ConstructorInitializer initializer) {

                cb.Write("{0}(", initializer.Name);
                initializer.Expression.AcceptVisitor(this);
                cb.Write(')');
            }

            public override void Visit(ForwardClassDeclaration decl) {

                cb.WriteLine("class {0};", decl.Name);
                cb.WriteLine();
            }

            public override void Visit(FunctionCallExpression exp) {

                exp.Function.AcceptVisitor(this);
                cb.Write('(');
                if (exp.HasArguments)
                    foreach (var argument in exp.Arguments)
                        argument.AcceptVisitor(this);

                cb.Write(')');
            }

            public override void Visit(FunctionCallStatement stmt) {

                cb.WriteIndent();
                base.Visit(stmt);
                cb.Write(';');
                cb.WriteLine();
            }

            public override void Visit(IdentifierExpression obj) {

                cb.Write(obj.Name);
            }

            public override void Visit(IfThenElseStatement stmt) {

                cb.WriteIndent();
                cb.Write("if (");
                if (stmt.ConditionExpression == null)
                    throw new InvalidOperationException("No se especifico la condicion.");

                stmt.ConditionExpression.AcceptVisitor(this);
                cb.Write(") {");
                cb.WriteLine();
                cb.Indent();
                if (stmt.TrueStmt == null)
                    throw new InvalidOperationException("No se especifico el bloque true.");

                stmt.TrueStmt.AcceptVisitor(this);
                cb.Unindent();
                cb.WriteIndent();
                cb.Write("}");
                cb.WriteLine();
                if (stmt.FalseStmt != null) {
                    cb.WriteIndent();
                    cb.WriteLine("else {");
                    cb.Indent();
                    stmt.FalseStmt.AcceptVisitor(this);
                    cb.Unindent();
                    cb.WriteIndent();
                    cb.Write('}');
                    cb.WriteLine();
                }
            }

            public override void Visit(InlineExpression obj) {

                cb.Write(obj.Code);
            }

            public override void Visit(InlineStatement stmt) {

                cb.WriteLine("{0};", stmt.Code);
            }

            public override void Visit(LiteralExpression obj) {

                cb.Write(obj.Value.ToString());
            }

            public override void Visit(VariableDeclaration decl) {

                if (decl.Implementation == ImplementationSpecifier.Static) {
                    cb.WriteIndent();
                    cb.Write("{0} {1}::{2}", decl.ValueType.Name, currentClass.Name, decl.Name);
                    if (decl.Initializer != null) {
                        cb.Write(" = ");
                        decl.Initializer.AcceptVisitor(this);
                    }
                    cb.Write(';');
                    cb.WriteLine();
                    cb.WriteLine();
                }
            }

            public override void Visit(FunctionDeclaration decl) {

                cb.WriteIndent();
                cb.Write("{0} {1}::{2}(", decl.ReturnType.Name, currentClass.Name, decl.Name);

                if (!decl.HasArguments) {
                    cb.Write(") {");
                    cb.WriteLine();
                    cb.Indent();
                }
                else {
                    cb.Indent();
                    bool first = true;
                    foreach (var argument in decl.Arguments) {
                        if (first)
                            first = false;
                        else
                            cb.Write(", ");

                        argument.AcceptVisitor(this);
                    }
                    cb.Write(") {");
                    cb.WriteLine();
                }

                if (decl.Body != null)
                    decl.Body.AcceptVisitor(this);

                cb.Unindent();
                cb.WriteIndent();
                cb.Write('}');
                cb.WriteLine();
                cb.WriteLine();
            }

            public override void Visit(NamespaceDeclaration decl) {

                bool global = decl.Name == "::";

                if (!global) {
                    cb.WriteLine("namespace {0} {{", decl.Name);
                    cb.WriteLine();
                    cb.Indent();
                }

                base.Visit(decl);

                if (!global) {
                    cb.Unindent();
                    cb.WriteIndent();
                    cb.Write('}');
                    cb.WriteLine();
                }
            }

            public override void Visit(ReturnStatement stmt) {

                cb.WriteIndent();
                cb.Write("return");
                if (stmt.Expression != null) {
                    cb.Write(' ');
                    stmt.Expression.AcceptVisitor(this);
                }
                cb.Write(';');
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
