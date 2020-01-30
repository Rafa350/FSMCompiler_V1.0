namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeGenerator {

    using System;
    using System.Collections.Generic;
    using System.Text;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel;

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
                sb.Append(obj.TypeName);
                sb.Append(' ');
                sb.Append(obj.Name);
            }

            public override void Visit(ClassDeclaration obj) {

                ClassDeclaration oldClass = currentClass;
                currentClass = obj;

                if (obj.Constructors != null)
                    foreach (var constructor in obj.Constructors)
                        constructor.AcceptVisitor(this);

                IEnumerable<MethodDeclaration> methods = obj.GetMethods();
                if (methods != null)
                    foreach (var method in methods)
                        method.AcceptVisitor(this);

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
                sb.AppendLine();

                if (!String.IsNullOrEmpty(obj.Body)) {
                    sb.Append(obj.Body);
                    sb.AppendLine();
                }

                indent--;
                sb.AppendIndent(indent);
                sb.Append('}');
                sb.AppendLine();
                sb.AppendLine();
            }

            public override void Visit(FieldDeclaration obj) {

                sb.AppendIndent(indent);
                sb.AppendLine("{0} {1};", obj.TypeName, obj.Name);
            }

            public override void Visit(MethodDeclaration obj) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} {1}::{2}(", obj.TypeName, currentClass.Name, obj.Name);

                if (obj.Arguments != null) {
                    bool first = true;
                    foreach (var argument in obj.Arguments) {
                        if (first)
                            first = false;
                        else
                            sb.Append(", ");
                        argument.AcceptVisitor(this);
                    }
                }

                sb.AppendLine(") {");
                sb.AppendLine();
                indent++;

                if (!String.IsNullOrEmpty(obj.Body)) {
                    sb.Append(obj.Body);
                    sb.AppendLine();
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

                if (obj.Members != null)
                    foreach (var member in obj.Members)
                        member.AcceptVisitor(this);

                indent--;
                sb.AppendIndent(indent);
                sb.Append('}');
                sb.AppendLine();
            }

            public override void Visit(UnitDeclaration obj) {

                if (obj.Members != null)
                    foreach (var member in obj.Members)
                        member.AcceptVisitor(this);
            }
        }

        public string Generate(UnitDeclaration unitDeclaration) {

            if (unitDeclaration == null)
                throw new ArgumentNullException(nameof(unitDeclaration));

            StringBuilder sb = new StringBuilder();
            var visitor = new GeneratorVisitor(sb);
            
            visitor.Visit(unitDeclaration);
            
            return sb.ToString();
        }
    }
}
