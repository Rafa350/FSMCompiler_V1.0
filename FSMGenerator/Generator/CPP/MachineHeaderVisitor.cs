namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP {

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
                .WriteLine("#include \"fsmDefines.h\"")
                .WriteLine()
                .WriteLine()
                .WriteLine("class {0}: public {1} {{", options.MachineClassName, options.MachineBaseClassName)
                .Indent()
                .WriteLine("private:")
                .Indent()
                .WriteLine("{0} *states[{1}];", options.StateBaseClassName, machine.StateCount)
                .UnIndent()
                .WriteLine("public:")
                .Indent()
                .WriteLine("{0}({1} *context);", options.MachineClassName, options.ContextClassName)
                .WriteLine("{0} *getState(unsigned stateId) const {{ return state[stateId]; }}", options.StateBaseClassName)
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

