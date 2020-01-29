namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeGenerator {

    using System;
    using System.Collections.Generic;
    using System.Text;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel;

    public sealed class Generator {

        private sealed class GeneratorVisitor : DefaultVisitor {

            private readonly StringBuilder sb;
            private int indent;

            public GeneratorVisitor(StringBuilder sb, int indent = 0) {

                if (sb == null)
                    throw new ArgumentNullException(nameof(sb));

                this.sb = sb;
                this.indent = indent;
            }

            public override void Visit(ArgumentDefinition obj) {

                sb.Append(obj.TypeName);
                sb.Append(' ');
                sb.Append(obj.Name);
            }

            public override void Visit(ClassDeclaration obj) {

                sb.AppendIndent(indent);
                sb.Append("class ");
                sb.Append(obj.Name);
                if (!String.IsNullOrEmpty(obj.BaseName)) { 
                    sb.Append(": ");
                    if (obj.BaseAccess == AccessSpecifier.Private)
                        sb.Append("private ");
                    else if (obj.BaseAccess == AccessSpecifier.Protected)
                        sb.Append("protected ");
                    else
                        sb.Append("public ");
                    sb.Append(obj.BaseName);
                }
                sb.Append(" {");
                sb.AppendLine();

                indent++;

                if (obj.Members != null) {
                    List<IClassMember> privateMembers = new List<IClassMember>();
                    foreach (var member in obj.Members) {
                        if (member.Access == AccessSpecifier.Private)
                            privateMembers.Add(member);
                    }

                    List<IClassMember> protectedMembers = new List<IClassMember>();
                    foreach (var member in obj.Members) {
                        if (member.Access == AccessSpecifier.Protected)
                            protectedMembers.Add(member);
                    }

                    List<IClassMember> publicMembers = new List<IClassMember>();
                    foreach (var member in obj.Members) {
                        if (member.Access == AccessSpecifier.Public)
                            publicMembers.Add(member);
                    }

                    if (privateMembers.Count > 0) {
                        sb.AppendLine();
                        sb.AppendIndent(indent);
                        sb.Append("private:");
                        sb.AppendLine();
                        indent++;
                        foreach (var member in privateMembers)
                            member.AcceptVisitor(this);
                        indent--;
                    }

                    if (protectedMembers.Count > 0) {
                        sb.AppendLine();
                        sb.AppendIndent(indent);
                        sb.Append("protected:");
                        sb.AppendLine();
                        indent++;
                        foreach (var member in protectedMembers)
                            member.AcceptVisitor(this);
                        indent--;
                    }

                    if (publicMembers.Count > 0) {
                        sb.AppendLine();
                        sb.AppendIndent(indent);
                        sb.Append("public:");
                        sb.AppendLine();
                        indent++;
                        foreach (var member in publicMembers)
                            member.AcceptVisitor(this);
                        indent--;
                    }
                }

                indent--;

                sb.AppendIndent(indent);
                sb.Append("};");
                sb.AppendLine();
                sb.AppendLine();
            }

            public override void Visit(FieldDeclaration obj) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} {1};", obj.TypeName, obj.Name);
                sb.AppendLine();
            }

            public override void Visit(MethodDeclaration obj) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0} {1}(", obj.TypeName, obj.Name);

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

                sb.Append(')');
                if (!String.IsNullOrEmpty(obj.Body)) {
                    sb.Append(" {");
                    sb.AppendLine();
                    indent++;

                    // Aqui el body

                    indent--;
                    sb.Append('}');
                    sb.AppendLine();
                }
                else
                    sb.Append(';');
                sb.AppendLine(); 
            }

            public override void Visit(NamespaceDeclaration obj) {

                sb.AppendIndent(indent);
                sb.AppendFormat("namespace {0} {{", obj.Name);
                sb.AppendLine();
                sb.AppendLine();
                indent++;

                foreach (var member in obj.Members)
                    member.AcceptVisitor(this);

                indent--;
                sb.Append('}');
                sb.AppendLine();
            }

            public override void Visit(UnitDeclaration obj) {

                foreach (var member in obj.Members)
                    member.AcceptVisitor(this);
            }
        }

        public string Generate(UnitDeclaration unitDeclaration) {

            StringBuilder sb = new StringBuilder();
            var visitor = new GeneratorVisitor(sb);
            
            visitor.Visit(unitDeclaration);
            
            return sb.ToString();
        }
    }
}
