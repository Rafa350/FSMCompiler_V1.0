namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

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
                .WriteLine("#include \"{0}\"", machineHeaderFileName)
                .WriteLine("#include \"{0}\"", stateHeaderFileName)
                .WriteLine()
                .WriteLine()
                .WriteLine("/// ----------------------------------------------------------------------")
                .WriteLine("/// \\brief    Constructor.")
                .WriteLine("/// \\param    context: Pointer to context data.")
                .WriteLine("///")
                .WriteLine("{0}::{0}(", options.MachineClassName)
                .Indent()
                .WriteLine("{0}* context):", options.ContextClassName)
                .WriteLine()
                .WriteLine("state(nullptr),");

            foreach (State state in machine.States)
                codeBuilder
                    .WriteLine("state{0}(new {0}(this)),", state.Name);

            codeBuilder
                .WriteLine("context(context) {")
                .WriteLine()
                .UnIndent()
                .WriteLine("}")
                .WriteLine()
                .WriteLine();

            codeBuilder
                .WriteLine("/// ----------------------------------------------------------------------")
                .WriteLine("/// \\brief    Set to initial state.")
                .WriteLine("///")
                .WriteLine("void {0}::start() {{", options.MachineClassName)
                .Indent()
                .WriteLine()
                .WriteLine("state = state{0};", machine.Start.Name)
                .UnIndent()
                .WriteLine("}")
                .WriteLine()
                .WriteLine();

            foreach (string transitionName in machine.GetTransitionNames()) {
                codeBuilder
                    .WriteLine("/// ----------------------------------------------------------------------")
                    .WriteLine("/// \\brief    Perform '{0}' transition.", transitionName)
                    .WriteLine("///")
                    .WriteLine("void {0}::{1}() {{", options.MachineClassName, transitionName)
                    .Indent()
                    .WriteLine()
                    .WriteLine("state->{0}();", transitionName)
                    .UnIndent()
                    .WriteLine("}")
                    .WriteLine()
                    .WriteLine();
            }

            writer.Write(codeBuilder.ToString());
        }
    }
}
