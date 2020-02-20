namespace MicroCompiler.CodeGenerator.Cpp {

    using System;
    using System.Linq;
    using System.Text;
    using MicroCompiler.CodeModel;

    public sealed class HeaderGenerator {

        private sealed class GeneratorVisitor : DefaultVisitor {

            private readonly StringBuilder sb;
            private int indent;
            private ClassDeclaration currentClass;

            public GeneratorVisitor(StringBuilder sb, int indent = 0) {
                this.sb = sb ?? throw new ArgumentNullException(nameof(sb));
                this.indent = indent;
            }

            public override void Visit(ArgumentDeclaration decl) {

                sb.Append(decl.ValueType.Name);
                sb.Append(' ');
                sb.Append(decl.Name);
            }

            public override void Visit(ClassDeclaration decl) {

                string ToString(AccessSpecifier access) {

                    switch (access) {
                        case AccessSpecifier.Public:
                            return "public";
                        case AccessSpecifier.Protected:
                            return "protected";
                        case AccessSpecifier.Private:
                        case AccessSpecifier.Default:
                            return "private";
                        default:
                            throw new InvalidOperationException(
                                String.Format("El especificador de acceso '{0}' no es valido.", access.ToString()));
                    }
                }

                ClassDeclaration oldClass = currentClass;
                currentClass = decl;

                sb.AppendIndent(indent);
                sb.Append("class ");
                sb.Append(decl.Name);
                if (!String.IsNullOrEmpty(decl.BaseName))
                    sb.AppendFormat(": {0} {1}", ToString(decl.BaseAccess), decl.BaseName);

                sb.Append(" {");
                sb.AppendLine();

                indent++;

                bool hasMembers = decl.HasMembers;

                // Genera la declaracio d'enumeracions
                //
                if (hasMembers) {
                    AccessSpecifier access = AccessSpecifier.Private;
                    bool first = true;
                    foreach (var enumeration in decl.Members.OfType<EnumerationDeclaration>()) {

                        if (first || (access != enumeration.Access)) {
                            access = enumeration.Access;
                            first = false;
                            sb.AppendIndent(indent);
                            sb.AppendFormat("{0}:", ToString(access));
                            sb.AppendLine();
                        }

                        indent++;
                        enumeration.AcceptVisitor(this);
                        indent--;
                    }
                }

                // Genera les declaracions de variables.
                //
                if (hasMembers) {
                    AccessSpecifier access = AccessSpecifier.Private;
                    bool first = true;
                    foreach (var variable in decl.Members.OfType<VariableDeclaration>()) {

                        if (first || (access != variable.Access)) {
                            access = variable.Access;
                            first = false;
                            sb.AppendIndent(indent);
                            sb.AppendFormat("{0}:", ToString(access));
                            sb.AppendLine();
                        }

                        indent++;
                        variable.AcceptVisitor(this);
                        indent--;
                    }
                }

                // Genera la declaracio dels constructors.
                //
                if (hasMembers) {
                    AccessSpecifier access = AccessSpecifier.Private;
                    bool first = true;
                    foreach (var constructor in decl.Members.OfType<ConstructorDeclaration>()) {

                        if (first || (access != constructor.Access)) {
                            access = constructor.Access;
                            first = false;
                            sb.AppendIndent(indent);
                            sb.AppendFormat("{0}:", ToString(access));
                            sb.AppendLine();
                        }

                        indent++;
                        constructor.AcceptVisitor(this);
                        indent--;
                    }
                }

                // Declara el destructor.
                //
                if (hasMembers) {
                    foreach (var destructor in decl.Members.OfType<DestructorDeclaration>()) {
                        sb.AppendIndent(indent);
                        sb.AppendFormat("{0}:", ToString(destructor.Access));
                        sb.AppendLine();
                        indent++;
                        destructor.AcceptVisitor(this);
                        indent--;
                    }
                }

                // Declara les funcions.
                //
                if (hasMembers) {
                    AccessSpecifier access = AccessSpecifier.Private;
                    bool first = true;
                    foreach (var function in decl.Members.OfType<FunctionDeclaration>()) {

                        if (first || (access != function.Access)) {
                            access = function.Access;
                            first = false;
                            sb.AppendIndent(indent);
                            sb.AppendFormat("{0}:", ToString(access));
                            sb.AppendLine();
                        }

                        indent++;
                        function.AcceptVisitor(this);
                        indent--;
                    }
                }

                indent--;

                sb.AppendIndent(indent);
                sb.Append("};");
                sb.AppendLine();
                sb.AppendLine();

                currentClass = oldClass;
            }

            public override void Visit(ConstructorDeclaration decl) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0}(", currentClass.Name);

                if (decl.Arguments != null) {
                    bool first = true;
                    foreach (var argument in decl.Arguments) {
                        if (first)
                            first = false;
                        else
                            sb.Append(", ");

                        argument.AcceptVisitor(this);
                    }
                }

                sb.Append(");");
                sb.AppendLine();
            }

            public override void Visit(ForwardClassDeclaration decl) {

                sb.AppendIndent(indent);
                sb.AppendFormat("class {0};", decl.Name);
                sb.AppendLine();
                sb.AppendLine();
            }

            public override void Visit(EnumerationDeclaration decl) {

                sb.AppendIndent(indent);
                sb.AppendFormat("enum class {0} {{", decl.Name);
                sb.AppendLine();
                indent++;

                bool first = true;
                foreach (var element in decl.Elements) {
                    if (first)
                        first = false;
                    else {
                        sb.Append(',');
                        sb.AppendLine();
                    }
                    sb.AppendIndent(indent);
                    sb.Append(element);
                }
                sb.AppendLine();

                indent--;
                sb.AppendIndent(indent);
                sb.Append("};");
                sb.AppendLine();
            }

            public override void Visit(FunctionDeclaration decl) {

                bool isClassMember = currentClass != null;

                sb.AppendIndent(indent);

                if (isClassMember)
                    switch (decl.Implementation) {
                        case ImplementationSpecifier.Virtual:
                            sb.Append("virtual ");
                            break;

                        case ImplementationSpecifier.Static:
                            sb.Append("static ");
                            break;

                        case ImplementationSpecifier.Default:
                        case ImplementationSpecifier.Instance:
                        case ImplementationSpecifier.Override:
                            break;

                        default:
                            throw new InvalidOperationException(
                                String.Format("El especificador de implementacion '{0}' no es valido.", decl.Implementation.ToString()));
                    }
                else 
                    switch (decl.Access) {
                        case AccessSpecifier.Private:
                            sb.Append("static ");
                            break;

                        case AccessSpecifier.Public:
                        case AccessSpecifier.Default:
                            break;

                        default:
                            throw new InvalidOperationException(
                                String.Format("El especificador de acceso '{0}' no es valido.", decl.Access.ToString()));
                    }

                sb.AppendFormat("{0} {1}(", decl.ReturnType.Name, decl.Name);
                if (decl.Arguments != null) {
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
                }
                sb.Append(")");

                if (isClassMember)
                    switch (decl.Implementation) {
                        case ImplementationSpecifier.Abstract:
                            sb.Append(" = 0;");
                            break;

                        case ImplementationSpecifier.Override:
                            sb.Append(" override;");
                            break;

                        default:
                            sb.Append(';');
                            break;
                    }
                else
                    sb.Append(';');

                sb.AppendLine();
            }

            public override void Visit(VariableDeclaration decl) {

                bool isClassMember = currentClass != null;

                sb.AppendIndent(indent);
                if (isClassMember) {
                    switch (decl.Implementation) {
                        case ImplementationSpecifier.Static:
                            sb.Append("static ");
                            break;
                    }
                }
                else if (decl.Access == AccessSpecifier.Private)
                    sb.Append("static ");

                sb.AppendLine("{0} {1};", decl.ValueType.Name, decl.Name);
            }

            public override void Visit(NamespaceDeclaration decl) {

                bool global = decl.Name == "::";

                if (!global) {
                    sb.AppendIndent(indent);
                    sb.AppendLine("namespace {0} {{", decl.Name);
                    sb.AppendLine();
                    indent++;
                }

                base.Visit(decl);

                if (!global) {
                    indent--;
                    sb.AppendIndent(indent);
                    sb.Append('}');
                    sb.AppendLine();
                }
            }

            public override void Visit(UnitDeclaration unit) {

                if (unit.Namespace != null)
                    unit.Namespace.AcceptVisitor(this);
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
