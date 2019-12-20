namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

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

            codeBuilder
                .WriteLine("#ifndef __{0}", guardString)
                .WriteLine("#define __{0}", guardString)
                .WriteLine()    
                .WriteLine()
                .WriteLine("class {0};", options.StateClassName)
                .WriteLine("class {0};", options.ContextClassName)
                .WriteLine()
                .WriteLine()
                .WriteLine("class {0} {{", options.MachineClassName)
                .Indent()
                .WriteLine("private:")
                .Indent()
                .WriteLine("{0}* state;", options.StateClassName)
                .WriteLine("{0}* context;", options.ContextClassName)
                .UnIndent()
                .WriteLine("public:")
                .Indent()
                .WriteLine("{0}({1}* context);", options.MachineClassName, options.ContextClassName)
                .WriteLine("inline {0}* getState() const {{ return state; }}", options.StateClassName)
                .WriteLine("inline {0}* getContext() const {{ return context; }}", options.ContextClassName)
                .UnIndent()
                .UnIndent()
                .WriteLine("};")
                .UnIndent()
                .WriteLine()
                .WriteLine()
                .WriteLine("#endif");

            writer.Write(codeBuilder.ToString());
        }
    }
}

