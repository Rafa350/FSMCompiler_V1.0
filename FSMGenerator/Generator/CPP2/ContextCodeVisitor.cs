namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    internal class ContextCodeVisitor: DefaultVisitor {

        private readonly TextWriter writer;
        private readonly CPPGeneratorOptions options;
        private CodeBuilder codeBuilder = new CodeBuilder();

        public ContextCodeVisitor(TextWriter writer, CPPGeneratorOptions options) {

            this.writer = writer;
            this.options = options;
        }

        public override void Visit(Machine machine) {

            string contextHeaderFileName = Path.GetFileName(options.ContextHeaderFileName);
            string stateHeaderFileName = Path.GetFileName(options.StateHeaderFileName);

            codeBuilder
                .WriteLine("#include \"{0}\"", contextHeaderFileName)
                .WriteLine("#include \"{0}\"", stateHeaderFileName)
                .WriteLine()
                .WriteLine();

            if (!String.IsNullOrEmpty(options.NsName))
                codeBuilder
                    .WriteLine("using namespace {0};", options.NsName)
                    .WriteLine()
                    .WriteLine();

            codeBuilder
                .WriteLine("/// ----------------------------------------------------------------------")
                .WriteLine("/// \\brief    Constructor.")
                .WriteLine("///")
                .WriteLine("{0}::{0}() {{", options.ContextClassName)
                .WriteLine()
                .WriteLine("}")
                .WriteLine()
                .WriteLine();

            codeBuilder
                .WriteLine("/// ----------------------------------------------------------------------")
                .WriteLine("/// \\brief    Set to initial state.")
                .WriteLine("///")
                .WriteLine("void {0}::start() {{", options.ContextClassName)
                .Indent()
                .WriteLine();

            if (machine.InitializeAction != null) {
                codeBuilder
                    .WriteLine("// Machine initialization actions.")
                    .WriteLine("//");
                machine.InitializeAction.AcceptVisitor(this);
                codeBuilder
                    .WriteLine();
            }

            if (machine.Start.EnterAction != null) {
                codeBuilder
                    .WriteLine("// Enter state actions.")
                    .WriteLine("//");
                machine.Start.EnterAction.AcceptVisitor(this);
                codeBuilder
                    .WriteLine();
            }

            codeBuilder
                .WriteLine("// Select initial state.")
                .WriteLine("//")
                .WriteLine("setState({0}::getInstance());", machine.Start.FullName)
                .UnIndent()
                .WriteLine("}")
                .WriteLine()
                .WriteLine();

            foreach (string transitionName in machine.GetTransitionNames()) {
                codeBuilder
                    .WriteLine("/// ----------------------------------------------------------------------")
                    .WriteLine("/// \\brief    Perform '{0}' transition.", transitionName)
                    .WriteLine("///")
                    .WriteLine("void {0}::on{1}() {{", options.ContextClassName, transitionName)
                    .Indent()
                    .WriteLine()
                    .WriteLine("static_cast<{1}*>(getState())->on{0}(this);", transitionName, options.StateClassName)
                    .UnIndent()
                    .WriteLine("}")
                    .WriteLine()
                    .WriteLine();
            }

            writer.Write(codeBuilder.ToString());
        }

        public override void Visit(CodeActity action) {

            if (!System.String.IsNullOrEmpty(action.Text)) {
                string[] lines = action.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                    if (!System.String.IsNullOrEmpty(line))
                        codeBuilder.WriteLine(line.Trim());
            }
        }

        public override void Visit(CallActivity action) {

            if (!System.String.IsNullOrEmpty(action.MethodName)) {
                codeBuilder.WriteLine("do{0}();", action.MethodName);
            }
        }
    }
}
