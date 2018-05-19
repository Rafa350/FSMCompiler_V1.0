namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    using MikroPicDesigns.FSMCompiler.v1.Model;

    internal static class StateHeaderGenerator {

        public static void GenerateStateTypedef(CodeBuilder codeBuilder, Machine machine) {

            codeBuilder
                .WriteLine("typedef enum {")
                .Indent();

            foreach (State state in machine.States) {

                codeBuilder
                    .WriteLine("State_{0},", state.Name);
            }

            codeBuilder
                .WriteLine("MAX_STATES")
                .UnIndent()
                .WriteLine("} State;")
                .WriteLine();
        }

        public static void GenerateEventTypedef(CodeBuilder codeBuilder, Machine machine) {

            codeBuilder
                .WriteLine("typedef enum {")
                .Indent();

            foreach (Event ev in machine.Events) {

                codeBuilder
                    .WriteLine("Event_{0},", ev.Name);
            }

            codeBuilder
                .WriteLine("MAX_EVENTS")
                .UnIndent()
                .WriteLine("} Event;")
                .WriteLine();
        }
    }
}
