namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeGenerator {

    using System;
    using System.Collections.Generic;
    using System.Text;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel;

    public sealed class HeaderGenerator {

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

                sb.Append(obj.ValueType.Name);
                sb.Append(' ');
                sb.Append(obj.Name);
            }

            public override void Visit(ClassDeclaration obj) {

                ClassDeclaration oldClass = currentClass;
                currentClass = obj;

                sb.AppendIndent(indent);
                sb.Append("class ");
                sb.Append(obj.Name);
                if (!String.IsNullOrEmpty(obj.BaseName)) { 
                    sb.Append(": ");
                    if (obj.BaseAccess == AccessMode.Private)
                        sb.Append("private ");
                    else if (obj.BaseAccess == AccessMode.Protected)
                        sb.Append("protected ");
                    else
                        sb.Append("public ");
                    sb.Append(obj.BaseName);
                }
                sb.Append(" {");
                sb.AppendLine();

                indent++;

                IEnumerable<ConstructorDeclaration> privateConstructors = obj.GetConstructors(AccessMode.Private);
                IEnumerable<ConstructorDeclaration> protectedConstructors = obj.GetConstructors(AccessMode.Protected);
                IEnumerable<ConstructorDeclaration> publicConstructors = obj.GetConstructors(AccessMode.Public);

                IEnumerable<MemberFunctionDeclaration> privateMethods = obj.GetMemberFunctions(AccessMode.Private);
                IEnumerable<MemberFunctionDeclaration> protectedMethods = obj.GetMemberFunctions(AccessMode.Protected);
                IEnumerable<MemberFunctionDeclaration> publicMethods = obj.GetMemberFunctions(AccessMode.Public);

                if ((privateConstructors != null) || (privateMethods != null)) {
                    sb.AppendLine();
                    sb.AppendIndent(indent);
                    sb.Append("private:");
                    sb.AppendLine();
                    indent++;
                    if (privateConstructors != null)
                        foreach (var constructor in privateConstructors)
                            constructor.AcceptVisitor(this);
                    if (privateMethods != null)
                        foreach (var member in privateMethods)
                            member.AcceptVisitor(this);
                    indent--;
                }

                if ((protectedConstructors != null) || (protectedMethods != null)) {
                    sb.AppendLine();
                    sb.AppendIndent(indent);
                    sb.Append("protected:");
                    sb.AppendLine();
                    indent++;
                    if (protectedConstructors != null)
                        foreach (var constructor in protectedConstructors)
                            constructor.AcceptVisitor(this);
                    if (protectedMethods != null)
                        foreach (var member in protectedMethods)
                            member.AcceptVisitor(this);
                    indent--;
                }

                if ((publicConstructors != null) || (publicMethods != null)) {
                    sb.AppendLine();
                    sb.AppendIndent(indent);
                    sb.Append("public:");
                    sb.AppendLine();
                    indent++;
                    if (publicConstructors != null)
                        foreach (var constructor in publicConstructors)
                            constructor.AcceptVisitor(this);
                    if (publicMethods != null)
                        foreach (var member in publicMethods)
                            member.AcceptVisitor(this);
                    indent--;
                }

                indent--;

                sb.AppendIndent(indent);
                sb.Append("};");
                sb.AppendLine();
                sb.AppendLine();

                currentClass = oldClass;
            }

            public override void Visit(ConstructorDeclaration obj) {

                sb.AppendIndent(indent);
                sb.AppendFormat("{0}(", currentClass.Name);

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

                sb.Append(");");
                sb.AppendLine();
            }

            public override void Visit(MemberVariableDeclaration obj) {

                if (obj.Mode == MemberVariableMode.Static)
                    sb.Append("static ");

                sb.AppendIndent(indent);
                sb.AppendLine("{0} {1};", obj.ValueType.Name, obj.Name);
            }

            public override void Visit(MemberFunctionDeclaration obj) {

                sb.AppendIndent(indent);
                switch (obj.Mode) {
                    case MemberFunctionMode.Virtual:
                        sb.Append("virtual ");
                        break;

                    case MemberFunctionMode.Static:
                        sb.Append("static ");
                        break;
                }
                
                sb.AppendFormat("{0} {1}(", obj.ReturnType.Name, obj.Name);
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
                sb.Append(")");
                
                switch (obj.Mode) {
                    case MemberFunctionMode.Abstract:
                        sb.Append(" = 0;");
                        break;

                    case MemberFunctionMode.Override:
                        sb.Append(" override;");
                        break;

                    default:
                        sb.Append(';');
                        break;
                }
                
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
                sb.Append('}');
                sb.AppendLine();
            }

            public override void Visit(UnitDeclaration obj) {

                if (obj.Members != null)
                    foreach (var member in obj.Members)
                        member.AcceptVisitor(this);
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
