namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Commands;

    internal sealed class StateCodeVisitor : DefaultVisitor {

        private readonly TextWriter writer;
        private readonly CPPGeneratorOptions options;
        private readonly CodeBuilder codeBuilder = new CodeBuilder();

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
                .WriteLine("/// \\param    machine: Pointer the machine.")
                .WriteLine("///")
                .WriteLine("{0}::{0}({1}* machine):", options.StateClassName, options.MachineClassName)
                .Indent()
                .WriteLine("machine(machine) {")
                .WriteLine()
                .UnIndent()
                .WriteLine("}")
                .WriteLine()
                .WriteLine();

            foreach (string transitionName in machine.GetTransitionNames()) {
                codeBuilder
                    .WriteLine("/// ----------------------------------------------------------------------")
                    .WriteLine("/// \\brief    Perform '{0}' transition.", transitionName)
                    .WriteLine("///")
                    .WriteLine("void {0}::{1}() {{", options.StateClassName, transitionName)
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

            // Genera el constructor.
            //
            codeBuilder
                .WriteLine("/// ----------------------------------------------------------------------")
                .WriteLine("/// \\brief    Constructor.")
                .WriteLine("/// \\param    machine: Pointer the machine.")
                .WriteLine("///")
                .WriteLine("{0}::{0}(", state.FullName)
                .Indent()
                .WriteLine("{0}* machine):", options.MachineClassName)
                .WriteLine()
                .WriteLine("{0}(machine) {{", options.StateClassName)
                .WriteLine()
                .UnIndent()
                .WriteLine("}")
                .WriteLine()
                .WriteLine();

            State s;
            bool hasActions;

            // Genera l'accio 'enter'.
            //
            hasActions = false;
            s = state;
            while (s != null) {
                if (s.EnterAction != null) {
                    if (!hasActions) {
                        codeBuilder
                            .WriteLine("/// ----------------------------------------------------------------------")
                            .WriteLine("/// \\brief    Perform 'enter' action.")
                            .WriteLine("///")
                            .WriteLine("void {0}::enter() {{", state.FullName)
                            .Indent()
                            .WriteLine();
                        hasActions = true;
                    }
                    s.EnterAction.AcceptVisitor(this);
                }
                s = s.Parent;
            }
            if (hasActions)
                codeBuilder
                    .UnIndent()
                    .WriteLine("}")
                    .WriteLine()
                    .WriteLine();

            // Genera l'accio 'exit'.
            //
            hasActions = false;
            s = state;
            while (s != null) {
                if (s.ExitAction != null) {
                    if (!hasActions) {
                        codeBuilder
                            .WriteLine("/// ----------------------------------------------------------------------")
                            .WriteLine("/// \\brief    Perform 'exit' action.")
                            .WriteLine("///")
                            .WriteLine("void {0}::exit() {{", state.FullName)
                            .Indent()
                            .WriteLine();
                        hasActions = true;
                    }
                    s.ExitAction.AcceptVisitor(this);
                }
                s = s.Parent;
            }

            if (hasActions)
                codeBuilder
                    .UnIndent()
                    .WriteLine("}")
                    .WriteLine()
                    .WriteLine();

            // Genera les transicions.
            //
            foreach (string transitionName in state.GetTransitionNames()) {
                codeBuilder
                    .WriteLine("/// ----------------------------------------------------------------------")
                    .WriteLine("/// \\brief    Perform '{0}' transition.", transitionName)
                    .WriteLine("///")
                    .WriteLine("void {0}::{1}() {{", state.FullName, transitionName)
                    .Indent()
                    .WriteLine();

                foreach (Transition transition in state.Transitions) {
                    if (transition.Name == transitionName)
                        transition.AcceptVisitor(this);
                }

                codeBuilder
                    .UnIndent()
                    .WriteLine("}")
                    .WriteLine()
                    .WriteLine();
            }
        }

        public override void Visit(Transition transition) {

            if (transition.Guard != null) {
                codeBuilder
                    .WriteLine("if ({0}) {{", transition.Guard.Expression)
                    .Indent();
            }

            if (transition.Action != null)
                transition.Action.AcceptVisitor(this);

            switch (transition.Mode) {
                case TransitionMode.Null:
                    break;

                case TransitionMode.JumpToState:
                    codeBuilder
                        .WriteLine()
                        .WriteLine("setState(getMachine()->state{0});", transition.NextState.FullName);
                    break;

                case TransitionMode.CallToState:
                    codeBuilder
                        .WriteLine()
                        .WriteLine("pushState(getMachine()->state{0});", transition.NextState.FullName);
                    break;

                case TransitionMode.ReturnFromState:
                    codeBuilder
                        .WriteLine()
                        .WriteLine("popState();");
                    break;
            }

            if (transition.Guard != null) {
                codeBuilder
                    .UnIndent()
                    .WriteLine("}");
            }
        }

        public override void Visit(InlineCommand action) {

            if (!System.String.IsNullOrEmpty(action.Text)) {
                string[] lines = action.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                    if (!System.String.IsNullOrEmpty(line))
                        codeBuilder.WriteLine(line.Trim());
            }
        }
    }
}



