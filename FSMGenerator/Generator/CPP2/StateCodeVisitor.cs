namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Commands;

    internal sealed class StateCodeVisitor: DefaultVisitor {

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
            string eventIdHeaderFileName = Path.GetFileName(options.EventIdHeaderFileName);
            string stateIdHeaderFileName = Path.GetFileName(options.StateIdHeaderFileName);

            codeBuilder
                //.WriteLine("#include \"fsmDefines.h\"")
                //.WriteLine("#include \"{0}\"", eventIdHeaderFileName)
                //.WriteLine("#include \"{0}\"", stateIdHeaderFileName)
                .WriteLine("#include \"{0}\"", machineHeaderFileName)
                .WriteLine("#include \"{0}\"", stateHeaderFileName)
                .WriteLine()
                .WriteLine();

            codeBuilder
                .WriteLine("{0}::{0}({1}* machine):", options.StateClassName, options.MachineClassName)
                .Indent()
                .WriteLine("machine(machine) {")
                .UnIndent()
                .WriteLine("}")
                .WriteLine();

            foreach (State state in machine.States)
                state.AcceptVisitor(this);

            writer.Write(codeBuilder.ToString());
        }

        public override void Visit(State state) {

            codeBuilder
                .WriteLine("{0}State::{0}State({1}* machine):", state.FullName, options.MachineClassName)
                .Indent()
                .WriteLine("{0}(machine) {{", options.StateClassName)
                .UnIndent()
                .WriteLine("}")
                .WriteLine();

            State s;
            bool hasActions;

            // Combina les accions 'Enter' del pares
            //
            hasActions = false;
            s = state;
            while (s != null) {
                if (s.EnterAction != null) {
                    if (!hasActions) {
                        codeBuilder
                            .WriteLine("void {0}State::enter() {{", state.FullName)
                            .WriteLine()
                            .Indent();
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
                    .WriteLine();

            // Combina les accions 'Exit' del pares
            //
            hasActions = false;
            s = state;
            while (s != null) {
                if (s.ExitAction != null) {
                    if (!hasActions) {
                        codeBuilder
                            .WriteLine("void {0}State::exit() {{", state.FullName)
                            .WriteLine()
                            .Indent();
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
                    .WriteLine();

            if (state.HasTransitions || state.Parent != null) {
                codeBuilder
                    .WriteLine("void {0}State::transition(unsigned eventId) {{", state.FullName)
                    .WriteLine()
                    .Indent();

                codeBuilder
                    .WriteLine("switch (eventId) {")
                    .Indent();

                if (state.HasTransitions)
                    foreach (Transition transition in state.Transitions)
                        transition.AcceptVisitor(this);

                codeBuilder
                    .UnIndent()
                    .WriteLine("}");

                codeBuilder
                    .UnIndent()
                    .WriteLine("}")
                    .WriteLine();
            }
        }

        public override void Visit(Transition transition) {

            codeBuilder
                .WriteLine()
                .WriteLine("case EV_{0}:", transition.Event.Name)
                .Indent();

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
                    codeBuilder.WriteLine("setState(ST_{0});", transition.NextState.FullName);
                    break;

                case TransitionMode.CallToState:
                    codeBuilder.WriteLine("pushState(ST_{0});", transition.NextState.FullName);
                    break;

                case TransitionMode.ReturnFromState:
                    codeBuilder.WriteLine("popState();");
                    break;
            }
            if (transition.Guard != null) {
                codeBuilder
                    .UnIndent()
                    .WriteLine("}");
            }

            codeBuilder
                .WriteLine("break;")
                .UnIndent();
        }

        public override void Visit(InlineCommand action) {

            if (!System.String.IsNullOrEmpty(action.Text)) {
                string[] lines = action.Text.Split(new char[] { '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                    if (!System.String.IsNullOrEmpty(line))
                        codeBuilder.WriteLine(line.Trim());
            }
        }

        public override void Visit(RaiseCommand action) {

            codeBuilder.WriteLine("raiseEvent(Event_{0}, {1});", action.Event.Name, action.DelayText);
        }
    }
}


