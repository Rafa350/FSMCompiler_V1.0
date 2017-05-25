namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    internal sealed class StateHeaderVisitor: DefaultVisitor {

        private enum Mode {
            generateEnums,
            generateForwards
        }

        private readonly TextWriter writer;
        private readonly CGeneratorOptions options;
        private readonly CodeBuilder codeBuilder = new CodeBuilder();
        private Mode mode;

        public StateHeaderVisitor(TextWriter writer, CGeneratorOptions options) {

            this.writer = writer;
            this.options = options;
        }

        public override void Visit(Machine machine) {

            codeBuilder
                .WriteLine("#include \"fsmDefines.h\"")
                .WriteLine()
                .WriteLine();

            codeBuilder
                .WriteLine("enum States {")
                .Indent();
            mode = Mode.generateEnums;
            foreach (State state in machine.States)
                state.AcceptVisitor(this);
            codeBuilder
                .UnIndent()
                .WriteLine("};")
                .WriteLine();

            mode = Mode.generateForwards;
            foreach (State state in machine.States)
                state.AcceptVisitor(this);

            writer.Write(codeBuilder.ToString());
        }

        public override void Visit(State state) {

            if (mode == Mode.generateEnums) {
                codeBuilder
                    .WriteLine("ST_{0}, ", state.Name);
            }

            else if (mode == Mode.generateForwards) {
                if (state.EnterActions != null)
                    codeBuilder
                        .WriteLine("extern void {0}Enter(void);", state.Name);

                if (state.ExitActions != null)
                    codeBuilder
                        .WriteLine("extern void {0}Exit(void);", state.Name);

                codeBuilder
                    .WriteLine("extern void {0}Transition(uint8_t event);", state.Name);
            }
        }
    }
}
