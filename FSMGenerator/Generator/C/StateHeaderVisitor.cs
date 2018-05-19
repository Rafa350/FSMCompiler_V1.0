namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    using System;
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

            string[] stdIncludes = { "stdint.h", "stdbool.h", "stdlib.h" };

            string guardName = String.Format("__{0}__", Path.GetFileNameWithoutExtension(options.StateHeaderFileName));

            string template =
                "typedef void (*Action)(Context *context);\n" +
                "typedef bool (*Guard)(Context *context);\n" +
                "\n" +
                "typedef struct {\n" +
                "  Event event;\n" +
                "  State nect;\n" +
                "  Action action;\n" +
                "} EventDescriptor;\n\n" +
                "typedef struct {\n" +
                "  State state;\n" +
                "  Action enter;\n" +
                "  Action exit;\n" +
                "  uint8_t transitionOffset;\n" +
                "} StateDescriptor;\n\n" +
                "typedef struct {\n" +
                "   StateDescriptor states[MAX_STATES];\n" +
                "} MachineDescriptor;\n\n" +
                "typedef struct {\n" +
                "  State state;\n" +
                "} Context;\n" +
                "\n";

            codeBuilder
                .WriteLine("#ifndef {0}", guardName)
                .WriteLine("#define {0}", guardName)
                .WriteLine()
                .WriteLine();

            // Includes standards
            //
            foreach (string include in stdIncludes)
                codeBuilder
                    .WriteLine("#include <{0}>", include);                    
            codeBuilder
                .WriteLine()
                .WriteLine();

            StateHeaderGenerator.GenerateStateTypedef(codeBuilder, machine);
            StateHeaderGenerator.GenerateEventTypedef(codeBuilder, machine);

            codeBuilder
                .WriteLine(template);

            mode = Mode.generateForwards;
            foreach (State state in machine.States)
                state.AcceptVisitor(this);

            codeBuilder
                .WriteLine()
                .WriteLine("#endif // {0}", guardName);

            writer.Write(codeBuilder.ToString());
        }

        public override void Visit(State state) {

            if (mode == Mode.generateEnums) {
                codeBuilder
                    .WriteLine("State_{0}, ", state.Name);
            }

            else if (mode == Mode.generateForwards) {
                if (state.EnterAction != null)
                    codeBuilder
                        .WriteLine("extern void {0}State_OnEnter(void);", state.Name);

                if (state.ExitAction != null)
                    codeBuilder
                        .WriteLine("extern void {0}State_OnExit(void);", state.Name);

                codeBuilder
                    .WriteLine("extern void {0}State_OnEvent(uint8_t event);", state.Name);
            }
        }

        public override void Visit(Event ev) {

            if (mode == Mode.generateEnums)
                codeBuilder
                    .WriteLine("Event_{0}, ", ev.Name);
        }
    }
}
