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

                int count = 0;
                foreach (State state in machine.States)
                    cb.WriteLine("#define State_{0} {1}", state.Name, options.FirstStateNum + count++);
                cb
                    .WriteLine("#define NUM_STATES {0}", count)
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

                int count = 0;
                foreach (Event ev in machine.Events) {
                    cb.WriteLine("#define Event_{0} {1}", ev.Name, options.FirstEventNum + count++);
                }
                cb
                    .WriteLine("#define NUM_EVENTS {0}", count)
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

                CodeGenerator generator = new CodeGenerator(machine);
                
                // Genera les funcions
                //
                generator.GenerateActionImplementation(codeBuilder);
                generator.GenerateGuardImplementation(codeBuilder);
                codeBuilder
                    .WriteLine("#ifdef FSM_IMPL_SWITCHCASE")
                    .WriteLine();
                generator.GenerateProcessorImplementation(codeBuilder);
                codeBuilder
                    .WriteLine("#endif")
                    .WriteLine();

                // Genera les taules 
                //
                codeBuilder
                    .WriteLine("#ifdef FSM_IMPL_TABLEDRIVEN")
                    .WriteLine();
                generator.GenerateTransitionDescriptorTable(codeBuilder);
                generator.GenerateStateDescriptorTable(codeBuilder);
                generator.GenerateMachineDescriptorTable(codeBuilder);
                codeBuilder
                    .WriteLine("#endif")
                    .WriteLine();

                writer.Write(codeBuilder.ToString());
            }
        }
    }
}