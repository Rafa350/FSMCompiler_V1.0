namespace MicroCompiler.CodeGenerator.C {

    using System;
    using System.Text;
    using MicroCompiler.CodeModel;

    public static class HeaderGenerator {

        private sealed class GeneratorVisitor : DefaultVisitor {

            private readonly StringBuilder sb;
            private readonly int indent = 0;

            public GeneratorVisitor(StringBuilder sb, int indent) {

                this.sb = sb;
                this.indent = indent;
            }

            public override void Visit(FunctionDeclaration decl) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} {1}(", decl.ReturnType.Name, decl.Name);
                sb.AppendLine(");");

                base.Visit(decl);
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
