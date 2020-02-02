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

                string ToString(AccessMode access) {
                    
                    switch(access) {
                        case AccessMode.Public:
                            return "public";
                        case AccessMode.Protected:
                            return "protected";
                        default:
                            return "private";
                    }
                }

                ClassDeclaration oldClass = currentClass;
                currentClass = obj;

                sb.AppendIndent(indent);
                sb.Append("class ");
                sb.Append(obj.Name);
                if (!String.IsNullOrEmpty(obj.BaseName))
                    sb.AppendFormat(": {0} {1}", ToString(obj.BaseAccess), obj.BaseName);
                sb.Append(" {");
                sb.AppendLine();

                indent++;

                // Genera les declaracions de variables.
                //
                if (obj.Variables != null) {
                    AccessMode access = AccessMode.Private;
                    bool first = true;
                    foreach (var variable in obj.Variables) {

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
                if (obj.Constructors != null) {
                    AccessMode access = AccessMode.Private;
                    bool first = true;
                    foreach (var constructor in obj.Constructors) {

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
                if (obj.Destructor != null) {
                    sb.AppendIndent(indent);
                    sb.AppendFormat("{0}:", ToString(obj.Destructor.Access));
                    sb.AppendLine();
                    indent++;
                    obj.Destructor.AcceptVisitor(this);
                    indent--;
                }

                // Declara les funcions.
                //
                if (obj.Functions!= null) {
                    AccessMode access = AccessMode.Private;
                    bool first = true;
                    foreach (var function in obj.Functions) {

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

                sb.AppendIndent(indent);
                if (obj.Mode == MemberVariableMode.Static)
                    sb.Append("static ");
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
