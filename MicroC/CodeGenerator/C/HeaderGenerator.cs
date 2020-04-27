namespace MicroCompiler.CodeGenerator.C {

    using System;
    using MicroCompiler.CodeModel;

    public static class HeaderGenerator {

        private sealed class GeneratorVisitor : DefaultVisitor {

            private readonly CodeBuilder cb;

            public GeneratorVisitor(CodeBuilder cb) {

                this.cb = cb ?? throw new ArgumentNullException(nameof(cb));
            }

            public override void Visit(FunctionDeclaration decl) {

                if ((decl.Access == AccessSpecifier.Default) ||
                    (decl.Access == AccessSpecifier.Public)) {

                    cb.WriteIndent();
                    cb.Write("{0} {1}(", decl.ReturnType.Name, decl.Name);
                    if (!decl.HasArguments)
                        cb.Write("void");
                    cb.Write(");");
                    cb.WriteLine();

                    base.Visit(decl);
                }
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
