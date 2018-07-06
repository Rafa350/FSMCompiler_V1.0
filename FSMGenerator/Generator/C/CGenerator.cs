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

            GenerateMachineCode(machine);
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
                    .WriteLine("#include \"fsm.h\"")
                    .WriteLine("#include \"fsm_defs.h\"")
                    .WriteLine()
                    .WriteLine();

                CodeGenerator generator = new CodeGenerator(machine);

                codeBuilder
                    .WriteLine("#ifndef __fsmChangeState")
                    .WriteLine("#define __fsmChangeState(newState)       state = newState")
                    .WriteLine("#endif")
                    .WriteLine()
                    .WriteLine("#ifndef __fsmDoAction")
                    .WriteLine("#define __fsmDoAction(action, context)   action(context)")
                    .WriteLine("#endif")
                    .WriteLine()
                    .WriteLine("#ifndef __fsmCheckGuard")
                    .WriteLine("#define __fsmCheckGuard(guard, context)  guard(context)")
                    .WriteLine("#endif")
                    .WriteLine()
                    .WriteLine();

                // Genera el enumerador amb els estats de la maquina
                //
                generator.GenerateStateTypeDeclaration(codeBuilder);
                
                // Genera les Accions i les guardes
                //
                generator.GenerateActionImplementation(codeBuilder);
                generator.GenerateGuardImplementation(codeBuilder);

                // Genera el procesador de la maquina en moduls 'switch/case'
                //
                generator.GenerateProcessorImplementation(codeBuilder);

                writer.Write(codeBuilder.ToString());
            }
        }
    }
}