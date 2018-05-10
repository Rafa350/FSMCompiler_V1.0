namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Actions;

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
                .WriteLine("#include \"fsmDefines.h\"")
                .WriteLine("#include \"{0}\"", eventIdHeaderFileName)
                .WriteLine("#include \"{0}\"", stateIdHeaderFileName)
                .WriteLine("#include \"{0}\"", machineHeaderFileName)
                .WriteLine("#include \"{0}\"", stateHeaderFileName)
                .WriteLine()
                .WriteLine();

            foreach (State state in machine.States)
                state.AcceptVisitor(this);

            writer.Write(codeBuilder.ToString());
        }

        public override void Visit(State state) {

            codeBuilder
                .WriteLine("{0}State::{0}State({1} *machine):", state.FullName, options.MachineBaseClassName)
                .Indent()
                .WriteLine("{0}(machine) {{", options.StateBaseClassName)
                .UnIndent()
                .WriteLine("}")
                .WriteLine();

            State s;
            bool hasActions;

            // Combina les accions 'onEnter' del pares
            //
            hasActions = false;
            s = state;
            while (s != null) {
                if (s.EnterActions != null) {
                    if (!hasActions) {
                        codeBuilder
                            .WriteLine("void {0}State::onEnter() {{", state.FullName)
                            .WriteLine()
                            .Indent();
                        hasActions = true;
                    }
                    s.EnterActions.AcceptVisitor(this);
                }
                s = s.Parent;
            }
            if (hasActions)
                codeBuilder
                    .UnIndent()
                    .WriteLine("}")
                    .WriteLine();

            // Combina les accions 'onExit' del pares
            //
            hasActions = false;
            s = state;
            while (s != null) {
                if (s.ExitActions != null) {
                    if (!hasActions) {
                        codeBuilder
                            .WriteLine("void {0}State::onExit() {{", state.FullName)
                            .WriteLine()
                            .Indent();
                        hasActions = true;
                    }
                    s.ExitActions.AcceptVisitor(this);
                }
                s = s.Parent;
            }

            if (hasActions)
                codeBuilder
                    .UnIndent()
                    .WriteLine("}")
                    .WriteLine();

            if (state.Transitions.HasTransitions || state.Parent != null) {
                codeBuilder
                    .WriteLine("void {0}State::onEvent(unsigned eventId) {{", state.FullName)
                    .WriteLine()
                    .Indent();

                codeBuilder
                    .WriteLine("switch (eventId) {")
                    .Indent();

                if (state.Transitions.HasTransitions)
                    state.Transitions.AcceptVisitor(this);

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

            if (!System.String.IsNullOrEmpty(transition.Condition)) {
                codeBuilder
                    .WriteLine("if ({0}) {{", transition.Condition)
                    .Indent();
            }

            if (transition.Actions != null)
                transition.Actions.AcceptVisitor(this);

            switch (transition.Mode) {
                case TransitionMode.Null:
                    break;

                case TransitionMode.JumpToState:
                    codeBuilder.WriteLine("setState(ST_{0});", transition.Next.FullName);
                    break;

                case TransitionMode.CallToState:
                    codeBuilder.WriteLine("pushState(ST_{0});", transition.Next.FullName);
                    break;

                case TransitionMode.ReturnFromState:
                    codeBuilder.WriteLine("popState();");
                    break;
            }
            if (!System.String.IsNullOrEmpty(transition.Condition)) {
                codeBuilder
                    .UnIndent()
                    .WriteLine("}");
            }

            codeBuilder
                .WriteLine("break;")
                .UnIndent();
        }

        public override void Visit(InlineAction action) {

            if (!String.IsNullOrEmpty(action.Condition)) {
                codeBuilder
                    .WriteLine("if ({0}) {{", action.Condition)
                    .Indent();
            }

            if (!System.String.IsNullOrEmpty(action.Text)) {
                string[] lines = action.Text.Split(new char[] { '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                    if (!System.String.IsNullOrEmpty(line))
                        codeBuilder.WriteLine(line.Trim());
            }

            if (!String.IsNullOrEmpty(action.Condition)) {
                codeBuilder
                    .UnIndent()
                    .WriteLine("}");
            }
        }

        public override void Visit(RaiseAction action) {

            if (!String.IsNullOrEmpty(action.Condition)) {
                codeBuilder
                    .WriteLine("if ({0}) {{", action.Condition)
                    .Indent();
            }

            codeBuilder.WriteLine("raiseEvent(EV_{0}, {1});", action.Event.Name, action.DelayText);

            if (!String.IsNullOrEmpty(action.Condition)) {
                codeBuilder
                    .UnIndent()
                    .WriteLine("}");
            }
        }
    }
}


