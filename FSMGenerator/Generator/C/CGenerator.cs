namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    public sealed class CGenerator : GeneratorBase {

        private readonly CGeneratorOptions options;

        public CGenerator(GeneratorParameters generatorParameters) {

            CGeneratorOptions options = new CGeneratorOptions();
            generatorParameters.Populate(options);
            this.options = options;
        }

        public override void Generate(Machine machine) {

            GenerateStateHeader(machine);
            GenerateEventHeader(machine);
            GenerateMachineCode(machine);
        }

        /// <summary>
        /// Genera el fitxer de capcelera amb la definicio dels estats.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// 
        private void GenerateStateHeader(Machine machine) {

            string folder = options.OutputPath;
            if (String.IsNullOrEmpty(folder))
                folder = @".\";

            // Crea el firxer fsm_<machine>_states.h
            //
            string fileName = String.Format("fsm_{0}_states.h", machine.Name);
            string path = Path.Combine(folder, fileName);
            using (StreamWriter writer = File.CreateText(path)) {

                string guardName = String.Format("__fsm_{0}_states__", machine.Name);

                CodeBuilder cb = new CodeBuilder();
                cb
                    .WriteLine("#ifndef {0}", guardName)
                    .WriteLine("#define {0}", guardName)
                    .WriteLine();

                int stateNum = 0;
                foreach (State state in machine.States)
                    cb.WriteLine("#define State_{0} {1}", state.Name, stateNum++);
                cb
                    .WriteLine("#define MAX_STATES {0}", stateNum)
                    .WriteLine();

                cb
                    .WriteLine("#endif // {0}", guardName)
                    .WriteLine();

                writer.WriteLine(cb.ToString());
            }
        }

        /// <summary>
        /// Genera el fitxer de capcelera amb la definicio dels events
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// 
        private void GenerateEventHeader(Machine machine) {

            string folder = options.OutputPath;
            if (String.IsNullOrEmpty(folder))
                folder = @".\";

            // Crea el fitxer fsm_<machine>_events.h
            //
            string fileName = String.Format("fsm_{0}_events.h", machine.Name);
            string path = Path.Combine(folder, fileName);
            using (StreamWriter writer = File.CreateText(path)) {

                string guardName = String.Format("__fsm_{0}_events__", machine.Name);
                CodeBuilder cb = new CodeBuilder();
                cb
                    .WriteLine("#ifndef {0}", guardName)
                    .WriteLine("#define {0}", guardName)
                    .WriteLine();

                int eventNum = 0;
                foreach (Event ev in machine.Events) {
                    cb.WriteLine("#define Event_{0} {1}", ev.Name, eventNum++);
                }
                cb
                    .WriteLine("#define MAX_EVENTS {0}", eventNum)
                    .WriteLine();

                cb
                    .WriteLine("#endif // {0}", guardName)
                    .WriteLine();

                writer.WriteLine(cb.ToString());
            }
        }

        /// <summary>
        /// \brief Genera el fitxer de codi de la maquina d'estat.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// 
        private void GenerateMachineCode(Machine machine) {

            string folder = options.OutputPath;
            if (String.IsNullOrEmpty(folder))
                folder = @".\";

            // Crea el fitxer "fsm_<machine>.c"
            //
            string fileName = String.Format("fsm_{0}.c", machine.Name);
            string path = Path.Combine(folder, fileName);
            using (StreamWriter writer = File.CreateText(path)) {

                CodeBuilder codeBuilder = new CodeBuilder();

                codeBuilder
                    .WriteLine("#include \"fsm.h\"", machine.Name)
                    .WriteLine("#include \"fsm_{0}_states.h\"", machine.Name)
                    .WriteLine("#include \"fsm_{0}_events.h\"", machine.Name)
                    .WriteLine("#include \"fsm_defs.h\"")
                    .WriteLine()
                    .WriteLine();

                StateCodeGenerator.GenerateActionImplementation(codeBuilder, machine);
                StateCodeGenerator.GenerateGuardImplementation(codeBuilder, machine);
                StateCodeGenerator.GenerateTransitionDescriptorTable(codeBuilder, machine);
                StateCodeGenerator.GenerateStateDescriptorTable(codeBuilder, machine);
                StateCodeGenerator.GenerateMachineDescriptor(codeBuilder, machine);

                writer.Write(codeBuilder.ToString());
            }
        }
    }
}