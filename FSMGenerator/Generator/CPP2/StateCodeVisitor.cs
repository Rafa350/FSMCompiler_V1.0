namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Commands;

    internal sealed class StateCodeVisitor : DefaultVisitor {

        private readonly TextWriter writer;
        private readonly CPPGeneratorOptions options;
        private readonly CodeBuilder codeBuilder = new CodeBuilder();
        public State currentState;

        public StateCodeVisitor(TextWriter writer, CPPGeneratorOptions options) {

            this.writer = writer;
            this.options = options;
        }

        public override void Visit(Machine machine) {

            string machineHeaderFileName = Path.GetFileName(options.MachineHeaderFileName);
            string stateHeaderFileName = Path.GetFileName(options.StateHeaderFileName);
            string contextHeaderFileName = Path.GetFileName(options.ContextHeaderFileName);

            codeBuilder
                .WriteLine("#include \"{0}\"", machineHeaderFileName)
                .WriteLine("#include \"{0}\"", stateHeaderFileName)
                .WriteLine("#include \"{0}\"", contextHeaderFileName)
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
                .WriteLine("{0}::{0}() {{", options.StateClassName)
                .WriteLine("}")
                .WriteLine()
                .WriteLine();


            foreach (string transitionName in machine.GetTransitionNames()) {
                codeBuilder
                    .WriteLine("/// ----------------------------------------------------------------------")
                    .WriteLine("/// \\brief    Perform '{0}' transition.", transitionName)
                    .WriteLine("/// \\param    machine: The state machine.")
                    .WriteLine("/// \\return   The next state.")
                    .WriteLine("///")
                    .WriteLine("{0}* {0}::on{1}({2}* machine) {{", options.StateClassName, transitionName, options.MachineClassName)
                    .WriteLine()
                    .WriteLine("}")
                    .WriteLine()
                    .WriteLine();
            }

            foreach (State state in machine.States)
                state.AcceptVisitor(this);

            writer.Write(codeBuilder.ToString());
        }

        public override void Visit(State state) {

            currentState = state;

            // Genera el constructor.
            //
            codeBuilder
                .WriteLine("/// ----------------------------------------------------------------------")
                .WriteLine("/// \\brief    Constructor.")
                .WriteLine("///")
                .WriteLine("{0}::{0}() {{", state.FullName)
                .WriteLine("}")
                .WriteLine()
                .WriteLine();
    
            codeBuilder
                .WriteLine("/// ----------------------------------------------------------------------")
                .WriteLine("/// \\brief    Get a instance of class.")
                .WriteLine("///")
                .WriteLine("{0}* {0}::getInstance() {{", state.FullName)
                .WriteLine()
                .Indent()
                .WriteLine("if (instance == nullptr)")
                .Indent()
                .WriteLine("instance = new {0}();", state.FullName)
                .UnIndent()
                .WriteLine("")
                .WriteLine("return instance;")
                .UnIndent()
                .WriteLine("}")
                .WriteLine()
                .WriteLine("{0}* {0}::instance = nullptr;", state.FullName)
                .WriteLine()
                .WriteLine();

            // Genera les transicions. 
            //
            foreach (string transitionName in state.GetTransitionNames()) {

                WriteTransitionMethodHeader(options.MachineClassName, state.FullName, transitionName);
                foreach (Transition transition in state.Transitions) {
                    if (transition.Name == transitionName)
                        transition.AcceptVisitor(this);
                }
                WriteMethodTail();
            }

            currentState = null;
        }

        public override void Visit(Transition transition) {

            // Genera el codi de les guardes.
            //
            codeBuilder
                .WriteLine("// Check transition guard.")
                .WriteLine("//");
            if (transition.Guard != null)
                codeBuilder
                    .WriteLine("if ({0}) {{", transition.Guard.Expression);
            else
                codeBuilder
                    .WriteLine("if (true) {");
            codeBuilder
                .Indent()
                .WriteLine();

            // Genera el codi de l'accio de sortida del estat actual.
            //
            if (transition.NextState != null) {
                if (currentState.ExitAction != null) {
                    codeBuilder
                        .WriteLine("// Exit state actions.")
                        .WriteLine("//");
                    currentState.ExitAction.AcceptVisitor(this);
                    codeBuilder
                        .WriteLine();
                }
            }

            // Genera el code de l'accio de la transicio.
            //
            if (transition.Action != null) {
                codeBuilder
                    .WriteLine("// Transition actions.")
                    .WriteLine("//");
                transition.Action.AcceptVisitor(this);
                codeBuilder
                    .WriteLine();
            }

            // Genera el codi de l'accio d'entrada del nou estat.
            //
            if (transition.NextState != null) {
                if (transition.NextState.EnterAction != null) {
                    codeBuilder
                        .WriteLine("// Enter state actions.")
                        .WriteLine("//");
                    transition.NextState.EnterAction.AcceptVisitor(this);
                    codeBuilder
                        .WriteLine();
                }
            }

            switch (transition.Mode) {
                case TransitionMode.Null:
                    codeBuilder
                        .WriteLine("// Return the same state.")
                        .WriteLine("//")
                        .WriteLine("return this;")
                        .WriteLine();
                    break;

                case TransitionMode.JumpToState:
                    codeBuilder
                        .WriteLine("// Return the next state.")
                        .WriteLine("//")
                        .WriteLine("return {0}::getInstance();", transition.NextState.FullName)
                        .WriteLine();
                    break;

                    /*case TransitionMode.CallToState:
                        codeBuilder
                            .WriteLine()
                            .WriteLine("pushState(machine, {0}::getInstance());", transition.NextState.FullName);
                        break;

                    case TransitionMode.ReturnFromState:
                        codeBuilder
                            .WriteLine()
                            .WriteLine("popState();");
                        break;*/
            }

            // Final del bloc de la guarda.
            //
            codeBuilder
                .UnIndent()
                .WriteLine("}")
                .WriteLine();
        }

        public override void Visit(InlineCommand action) {

            if (!System.String.IsNullOrEmpty(action.Text)) {
                string[] lines = action.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                    if (!System.String.IsNullOrEmpty(line))
                        codeBuilder.WriteLine(line.Trim());
            }
        }

        public override void Visit(MachineCommand action) {

            if (!System.String.IsNullOrEmpty(action.Text)) {
                codeBuilder.WriteLine("machine->do{0}();", action.Text.Trim());
            }
        }

        private void WriteTransitionMethodHeader(string machineClassName, string stateClassName, string transitionName) {

            codeBuilder
                .WriteLine("/// ----------------------------------------------------------------------")
                .WriteLine("/// \\brief    Perform '{0}' transition to other state.", transitionName)
                .WriteLine("/// \\param    machine: The state machine.")
                .WriteLine("/// \\return   The next state.")
                .WriteLine("///")
                .WriteLine("{0}* {1}::on{2}({3}* machine) {{", options.StateClassName, stateClassName, transitionName, machineClassName)
                .Indent()
                .WriteLine();
        }

        private void WriteMethodTail() {

            codeBuilder
                .UnIndent()
                .WriteLine("}")
                .WriteLine()
                .WriteLine();
        }
    }
}



