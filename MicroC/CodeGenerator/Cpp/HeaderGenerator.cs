namespace MicroCompiler.CodeGenerator.Cpp {

    using System;
    using System.Linq;
    using MicroCompiler.CodeModel;

    public sealed class HeaderGenerator {

        private sealed class GeneratorVisitor : DefaultVisitor {

            private readonly CodeBuilder cb;
            private ClassDeclaration currentClass;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="cb">Objecte constructor de codi.</param>
            /// 
            public GeneratorVisitor(CodeBuilder cb) {

                this.cb = cb ?? throw new ArgumentNullException(nameof(cb));
            }

            /// <summary>
            /// Visita un objecte 'ArgumentDeclaration'
            /// </summary>
            /// <param name="decl">L'objecte a visitar.</param>
            /// 
            public override void Visit(ArgumentDeclaration decl) {

                cb.Write("{0} {1}", decl.ValueType.Name, decl.Name);
            }

            /// <summary>
            /// Visita un objecte 'ClassDeclaration'.
            /// </summary>
            /// <param name="decl">L'objecte a visitar.</param>
            /// 
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

                cb.WriteIndent();
                cb.Write("class ");
                cb.Write(decl.Name);
                if (!String.IsNullOrEmpty(decl.BaseName))
                    cb.Write(": {0} {1}", ToString(decl.BaseAccess), decl.BaseName);

                cb.Write(" {");
                cb.WriteLine();

                cb.Indent();

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
                            cb.WriteIndent();
                            cb.Write("{0}:", ToString(access));
                            cb.WriteLine();
                        }

                        cb.Indent();
                        enumeration.AcceptVisitor(this);
                        cb.Unindent();
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
                            cb.WriteIndent();
                            cb.Write("{0}:", ToString(access));
                            cb.WriteLine();
                        }

                        cb.Indent();
                        variable.AcceptVisitor(this);
                        cb.Unindent();
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
                            cb.WriteIndent();
                            cb.Write("{0}:", ToString(access));
                            cb.WriteLine();
                        }

                        cb.Indent();
                        constructor.AcceptVisitor(this);
                        cb.Unindent();
                    }
                }

                // Declara el destructor.
                //
                if (hasMembers) {
                    foreach (var destructor in decl.Members.OfType<DestructorDeclaration>()) {
                        cb.WriteIndent();
                        cb.Write("{0}:", ToString(destructor.Access));
                        cb.WriteLine();

                        cb.Indent();
                        destructor.AcceptVisitor(this);
                        cb.Unindent();
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
                            cb.WriteIndent();
                            cb.Write("{0}:", ToString(access));
                            cb.WriteLine();
                        }

                        cb.Indent();
                        function.AcceptVisitor(this);
                        cb.Unindent();
                    }
                }

                cb.Unindent();

                cb.WriteIndent();
                cb.Write("};");
                cb.WriteLine();
                cb.WriteLine();

                currentClass = oldClass;
            }

            /// <summary>
            /// Visita un objecte 'ConstructorDeclaration'.
            /// </summary>
            /// <param name="decl">L'objecte a visitar.</param>
            /// 
            public override void Visit(ConstructorDeclaration decl) {

                cb.WriteIndent();
                cb.Write("{0}(", currentClass.Name);

                if (decl.Arguments != null) {
                    bool first = true;
                    foreach (var argument in decl.Arguments) {
                        if (first)
                            first = false;
                        else
                            cb.Write(", ");

                        argument.AcceptVisitor(this);
                    }
                }

                cb.Write(");");
                cb.WriteLine();
            }

            public override void Visit(ForwardClassDeclaration decl) {

                cb.WriteLine("class {0};", decl.Name);
                cb.WriteLine();
            }

            public override void Visit(EnumerationDeclaration decl) {

                cb.WriteLine("enum class {0} {{", decl.Name);

                cb.Indent();

                bool first = true;
                foreach (var element in decl.Elements) {
                    if (first)
                        first = false;
                    else {
                        cb.Write(',');
                        cb.WriteLine();
                    }
                    cb.WriteIndent();
                    cb.Write(element);
                }
                cb.WriteLine();

                cb.Unindent();
                cb.WriteIndent();
                cb.Write("};");
                cb.WriteLine();
            }

            public override void Visit(FunctionDeclaration decl) {

                bool isClassMember = currentClass != null;

                cb.WriteIndent();

                if (isClassMember)
                    switch (decl.Implementation) {
                        case ImplementationSpecifier.Virtual:
                            cb.Write("virtual ");
                            break;

                        case ImplementationSpecifier.Static:
                            cb.Write("static ");
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
                            cb.Write("static ");
                            break;

                        case AccessSpecifier.Public:
                        case AccessSpecifier.Default:
                            break;

                        default:
                            throw new InvalidOperationException(
                                String.Format("El especificador de acceso '{0}' no es valido.", decl.Access.ToString()));
                    }

                cb.Write("{0} {1}(", decl.ReturnType.Name, decl.Name);
                if (decl.Arguments != null) {
                    bool first = true;
                    foreach (var argument in decl.Arguments) {
                        if (first)
                            first = false;
                        else
                            cb.Write(", ");

                        argument.AcceptVisitor(this);
                    }
                }
                cb.Write(')');

                if (isClassMember)
                    switch (decl.Implementation) {
                        case ImplementationSpecifier.Abstract:
                            cb.Write(" = 0;");
                            break;

                        case ImplementationSpecifier.Override:
                            cb.Write(" override;");
                            break;

                        default:
                            cb.Write(';');
                            break;
                    }
                else
                    cb.Write(';');

                cb.WriteLine();
            }

            public override void Visit(VariableDeclaration decl) {

                bool isClassMember = currentClass != null;

                cb.WriteIndent();
                if (isClassMember) {
                    switch (decl.Implementation) {
                        case ImplementationSpecifier.Static:
                            cb.Write("static ");
                            break;
                    }
                }
                else if (decl.Access == AccessSpecifier.Private)
                    cb.Write("static ");

                cb.Write("{0} {1};", decl.ValueType.Name, decl.Name);
                cb.WriteLine();
            }

            /// <summary>
            /// Visita un objecte 'NamespaceDeclaration.
            /// </summary>
            /// <param name="decl">L'objecte a visitar.</param>
            /// 
            public override void Visit(NamespaceDeclaration decl) {

                bool global = decl.Name == "::";

                if (!global) {
                    cb.WriteIndent();
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

            /// <summary>
            /// Visita un objecte 'UnitDeclaration'.
            /// </summary>
            /// <param name="decl">L'objecte a visitar.</param>
            /// 
            public override void Visit(UnitDeclaration decl) {

                if (decl.Namespace != null)
                    decl.Namespace.AcceptVisitor(this);
            }
        }

        /// <summary>
        /// Genera el codi font.
        /// </summary>
        /// <param name="unitDecl">Objecte 'UnitDeclaration'.</param>
        /// <returns>El codi generat.</returns>
        /// 
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
