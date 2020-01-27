namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    internal sealed class StateHeaderVisitor: DefaultVisitor {

        private readonly TextWriter writer;
        private readonly CPPGeneratorOptions options;
        private CodeBuilder codeBuilder = new CodeBuilder();

        public StateHeaderVisitor(TextWriter writer, CPPGeneratorOptions options) {

            this.writer = writer;
            this.options = options;
        }

        public override void Visit(Machine machine) {

            string guardString = Path.GetFileName(options.StateHeaderFileName).ToUpper().Replace(".", "_");

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

            codeBuilder
                .WriteLine("class {0};", options.MachineClassName)
                .WriteLine("class {0};", options.ContextClassName)
                .WriteLine();

            codeBuilder
                .WriteLine("class {0} {{", options.StateClassName)
                .Indent()
                .WriteLine("protected:")
                .Indent()
                .WriteLine("{0}();", options.StateClassName)
                .UnIndent()
                .WriteLine("public:")
                .Indent();

            foreach(string transitionName in machine.GetTransitionNames())
                codeBuilder
                    .WriteLine("virtual {0}* on{1}({2}* machine);", options.StateClassName, transitionName, options.MachineClassName);

            codeBuilder
                .UnIndent()
                .UnIndent()
                .WriteLine("};")
                .WriteLine();

            foreach (State state in machine.States)
                state.AcceptVisitor(this);

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

        public override void Visit(State state) {

            codeBuilder
                .WriteLine("class {0}: public {1} {{", state.FullName, options.StateClassName)
                .Indent()
                .WriteLine("private:")
                .Indent()
                .WriteLine("static {0}* instance;", state.FullName)
                .UnIndent()
                .WriteLine("private:")
                .Indent()
                .WriteLine("{0}();", state.FullName)
                .UnIndent()
                .WriteLine("public:")
                .Indent()
                .WriteLine("static {0}* getInstance();", state.FullName);
            
            foreach (string transitionName in state.GetTransitionNames()) {
                codeBuilder
                    .WriteLine("{0}* on{1}({2}* machine) override;", options.StateClassName, transitionName, options.MachineClassName);
            }

            codeBuilder
                .UnIndent()
                .UnIndent()
                .WriteLine("};")
                .WriteLine();
        }
    }
}

