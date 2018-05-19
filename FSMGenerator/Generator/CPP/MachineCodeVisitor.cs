namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP {

    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    internal class MachineCodeVisitor: DefaultVisitor {

        private readonly TextWriter writer;
        private readonly CPPGeneratorOptions options;
        private CodeBuilder codeBuilder = new CodeBuilder();

        public MachineCodeVisitor(TextWriter writer, CPPGeneratorOptions options) {

            this.writer = writer;
            this.options = options;
        }

        public override void Visit(Machine machine) {

            string machineHeaderFileName = Path.GetFileName(options.MachineHeaderFileName);
            string stateHeaderFileName = Path.GetFileName(options.StateHeaderFileName);
            string stateIdHeaderFileName = Path.GetFileName(options.StateIdHeaderFileName);

            codeBuilder
                .WriteLine("#include \"fsmDefines.h\"")
                .WriteLine("#include \"{0}\"", stateIdHeaderFileName)
                .WriteLine("#include \"{0}\"", machineHeaderFileName)
                .WriteLine("#include \"{0}\"", stateHeaderFileName)
                .WriteLine()
                .WriteLine()
                .WriteLine("{0}::{0}({1} *context):", options.MachineClassName, options.ContextClassName)
                .Indent()
                .WriteLine("{0}(context) {{", options.MachineBaseClassName)
                .WriteLine();

            foreach (State state in machine.States)
                state.AcceptVisitor(this);
            
            codeBuilder
                .WriteLine()
                .WriteLine("start(states[ST_{0}]);", machine.InitialState.FullName)
                .UnIndent()
                .WriteLine("}");

            writer.Write(codeBuilder.ToString());
        }

        public override void Visit(State state) {

            codeBuilder.WriteLine("states[ST_{0}] = new {0}State(this);", state.FullName);
        }
    }
}
