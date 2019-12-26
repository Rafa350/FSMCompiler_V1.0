namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    internal class MachineHeaderVisitor: DefaultVisitor {

        private readonly TextWriter writer;
        private readonly CPPGeneratorOptions options;
        private CodeBuilder codeBuilder = new CodeBuilder();

        public MachineHeaderVisitor(TextWriter writer, CPPGeneratorOptions options) {

            this.writer = writer;
            this.options = options;
        }

        public override void Visit(Machine machine) {

            string guardString = Path.GetFileName(options.MachineHeaderFileName).ToUpper().Replace(".", "_");

            // Escriu la capcelera del fitxer.
            //
            codeBuilder
                .WriteLine("#ifndef __{0}", guardString)
                .WriteLine("#define __{0}", guardString)
                .WriteLine()
                .WriteLine();

            if (!String.IsNullOrEmpty(options.NsName))
                codeBuilder
                    .WriteLine("namespace {0} {{", options.NsName)
                    .Indent()
                    .WriteLine();

            // Escriu la definicial de la clase
            //
            codeBuilder
                .WriteLine("class {0};", options.StateClassName)
                .WriteLine("class {0};", options.ContextClassName)
                .WriteLine()
                .WriteLine("class {0} {{", options.MachineClassName)
                .Indent()
                .WriteLine("private:")
                .Indent()
                .WriteLine("{0}* state;", options.StateClassName)
                .WriteLine("{0}* context;", options.ContextClassName)
                .UnIndent()
                .WriteLine("public:")
                .Indent();

            foreach (State state in machine.States)
                codeBuilder
                    .WriteLine("{0}* state{1};", options.StateClassName, state.FullName);

            codeBuilder
                .UnIndent()
                .WriteLine("private:")
                .Indent()
                .WriteLine("void setState({0}* state);", options.StateClassName)
                .WriteLine("void pushState({0}* state);", options.StateClassName)
                .WriteLine("void popState();")
                .UnIndent()
                .WriteLine("public:")
                .Indent()
                .WriteLine("{0}({1}* context);", options.MachineClassName, options.ContextClassName)
                .WriteLine("inline {0}* getState() const {{ return state; }}", options.StateClassName)
                .WriteLine("inline {0}* getContext() const {{ return context; }}", options.ContextClassName)
                .WriteLine("void start();");

            foreach (string transitionName in machine.GetTransitionNames())
                codeBuilder
                    .WriteLine("void {0}();", transitionName);

            codeBuilder
                .WriteLine()
                .UnIndent()
                .WriteLine("friend {0};", options.StateClassName)
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
        }
    }
}

