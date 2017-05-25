namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    using System.Text;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Actions;

    internal sealed class StateCodeVisitor: DefaultVisitor {

        private enum Mode {
            generateTable,
            generateFunctions
        }

        private readonly TextWriter writer;
        private readonly CGeneratorOptions options;
        private readonly CodeBuilder codeBuilder = new CodeBuilder();
        private Mode mode;

        public StateCodeVisitor(TextWriter writer, CGeneratorOptions options) {

            this.writer = writer;
            this.options = options;
        }

        public override void Visit(Machine machine) {

            codeBuilder
                .WriteLine("#include \"fsmDefines.h\"")
                .WriteLine("#include \"{0}\"", options.StateHeaderFileName)
                .WriteLine()
                .WriteLine();

            mode = Mode.generateTable;
            codeBuilder
                .WriteLine("StateDescriptor descriptors[] = {")
                .Indent();
            foreach (State state in machine.States)
                state.AcceptVisitor(this);
            codeBuilder
                .UnIndent()
                .WriteLine("};")
                .WriteLine()
                .WriteLine();

            mode = Mode.generateFunctions;
            foreach (State state in machine.States)
                state.AcceptVisitor(this);

            writer.Write(codeBuilder.ToString());
        }

        public override void Visit(State state) {

            if (mode == Mode.generateTable) {

                StringBuilder sb = new StringBuilder();

                if (state.EnterActions == null)
                    sb.Append("{ NULL, ");
                else
                    sb.AppendFormat("{{ {0}Enter, ", state.Name);

                if (state.ExitActions == null)
                    sb.Append("NULL, ");
                else
                    sb.AppendFormat("{0}Exit, ", state.Name);

                sb.AppendFormat("{0}Transition }},", state.Name);

                codeBuilder.WriteLine(sb.ToString());
            }

            else if (mode == Mode.generateFunctions) {

                if (state.EnterActions != null) {
                    codeBuilder
                        .WriteLine("void {0}Enter() {{", state.Name)
                        .WriteLine()
                        .Indent();
                    state.EnterActions.AcceptVisitor(this);
                    codeBuilder
                        .UnIndent()
                        .WriteLine("}")
                        .WriteLine();
                }
                if (state.ExitActions != null) {
                    codeBuilder
                        .WriteLine("void {0}Exit() {{", state.Name)
                        .WriteLine()
                        .Indent();
                    state.ExitActions.AcceptVisitor(this);
                    codeBuilder
                        .UnIndent()
                        .WriteLine("}")
                        .WriteLine();
                }

                codeBuilder
                    .WriteLine("void {0}Transition(uint8_t event) {{", state.Name)
                    .WriteLine()
                    .Indent()
                    .WriteLine("switch (event) {")
                    .Indent();

                state.Transitions.AcceptVisitor(this);

                codeBuilder
                    .UnIndent()
                    .WriteLine("}")
                    .UnIndent()
                    .WriteLine("}")
                    .WriteLine();
            }
        }

        public override void Visit(Transition transition) {

            codeBuilder
                .WriteLine("case {0}:", transition.Event.Name)
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
                    codeBuilder.WriteLine("fsmSetState(ST_{0});", transition.Next.Name);
                    break;

                case TransitionMode.CallToState:
                    codeBuilder.WriteLine("fsmPushState(ST_{0});", transition.Next.Name);
                    break;

                case TransitionMode.ReturnFromState:
                    codeBuilder.WriteLine("fsmPopState();");
                    break;
            }
            if (!System.String.IsNullOrEmpty(transition.Condition)) {
                codeBuilder
                    .UnIndent()
                    .WriteLine("}");
            }

            codeBuilder
                .WriteLine("break;")
                .WriteLine()
                .UnIndent();
        }

        public override void Visit(InlineAction action) {

            if (!System.String.IsNullOrEmpty(action.Text)) {
                string[] lines = action.Text.Split(new char[] { '\r', '\n'} );

                foreach (string line in lines)
                    if (!System.String.IsNullOrEmpty(line))
                        codeBuilder.WriteLine(line.Trim());
            }
        }
    }
}


