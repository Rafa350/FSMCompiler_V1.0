namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeGenerator;

    internal class ContextHeaderVisitor: Model.DefaultVisitor {

        private readonly TextWriter writer;
        private readonly CPPGeneratorOptions options;
        private readonly CodeBuilder codeBuilder = new CodeBuilder();

        public ContextHeaderVisitor(TextWriter writer, CPPGeneratorOptions options) {

            this.writer = writer;
            this.options = options;
        }

        public override void Visit(Machine machine) {

            string guardString = Path.GetFileName(options.ContextHeaderFileName).ToUpper().Replace(".", "_");

            // Escriu la capcelera del fitxer.
            //
            codeBuilder
                .WriteLine("#ifndef __{0}", guardString)
                .WriteLine("#define __{0}", guardString)
                .WriteLine()
                .WriteLine()
                .WriteLine("#include \"eos.h\"")
                .WriteLine("#include \"Services/Fsm/eosFsmContextBase.h\"")
                .WriteLine()
                .WriteLine();

            if (!String.IsNullOrEmpty(options.NsName))
                codeBuilder
                    .WriteLine("namespace {0} {{", options.NsName)
                    .Indent()
                    .WriteLine();

            // Escriu la definicio de la clase
            //
            codeBuilder
                .WriteLine("class {0};", options.StateClassName)
                .WriteLine()
                .Write("class {0}", options.ContextClassName);
            if (String.IsNullOrEmpty(options.ContextBaseClassName))
                codeBuilder
                    .WriteLine(" {");
            else 
                codeBuilder
                    .WriteLine(": public {0} {{", options.ContextBaseClassName);

            codeBuilder
                .Indent()
                .WriteLine("public:")
                .Indent()
                .WriteLine("{0}();", options.ContextClassName)
                .WriteLine("void start();")
                .WriteLine("void terminate();");

            foreach (string transitionName in machine.GetTransitionNames())
                codeBuilder
                    .WriteLine("void on{0}();", transitionName);

            foreach (string commandName in machine.GetCommandNames())
                codeBuilder
                    .WriteLine("void do{0}();", commandName);

            codeBuilder
                .UnIndent()
                .UnIndent()
                .WriteLine("};")
                .UnIndent();

            if (!String.IsNullOrEmpty(options.NsName))
                codeBuilder
                    .UnIndent()
                    .WriteLine("}");

            codeBuilder
                .WriteLine()
                .WriteLine()
                .WriteLine("#endif // __{0}", guardString);

            writer.Write(codeBuilder.ToString());

            List<IClassMember> members = new List<IClassMember>();

            members.Add(new MethodDeclaration(
                "start",
                "void",
                AccessSpecifier.Public,
                null,
                null));

            members.Add(new MethodDeclaration(
                "terminate",
                "void",
                AccessSpecifier.Public,
                null,
                null));

            ArgumentDefinition contextArgument = new ArgumentDefinition("context", String.Format("{0}*", options.ContextClassName));

            foreach (string transitionName in machine.GetTransitionNames())
                members.Add(new MethodDeclaration(
                    String.Format("on{0}", transitionName),
                    "void",
                    AccessSpecifier.Public,
                    new List<ArgumentDefinition>() { contextArgument },
                    null));

            foreach (string commandName in machine.GetCommandNames())
                members.Add(new MethodDeclaration(
                    String.Format("do{0}", commandName),
                    "void",
                    AccessSpecifier.Public,
                    new List<ArgumentDefinition>() { contextArgument },
                    null));

            ClassDeclaration classDeclaration = new ClassDeclaration(
                options.StateClassName,
                options.StateBaseClassName,
                AccessSpecifier.Public,
                members );

            List<IUnitMember> unitMembers = new List<IUnitMember>();

            if (String.IsNullOrEmpty(options.NsName))
                unitMembers.Add(classDeclaration);
            else {
                List<INamespaceMember> namespaceMembers = new List<INamespaceMember>();
                namespaceMembers.Add(classDeclaration);
                NamespaceDeclaration namespaceDeclaration = new NamespaceDeclaration(options.NsName, namespaceMembers);
                unitMembers.Add(namespaceDeclaration);
            }

            UnitDeclaration unitDeclaration = new UnitDeclaration(unitMembers);

            Generator gen = new Generator();
            string s = gen.Generate(unitDeclaration);
        }
    }
}

