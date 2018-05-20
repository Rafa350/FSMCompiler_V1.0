namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;
   
    public sealed class CGenerator: GeneratorBase {

        private readonly CGeneratorOptions options;

        public CGenerator(GeneratorParameters generatorParameters) {

            CGeneratorOptions options = new CGeneratorOptions();
            generatorParameters.Populate(options);
            this.options = options;
        }

        public override void Generate(Machine machine) {

            GenerateMachineHeader(machine);
            GenerateMachineCode(machine);
            GenerateStateHeader(machine);
            GenerateStateCode(machine);
        }

        private void GenerateMachineHeader(Machine machine) {

            if (!String.IsNullOrEmpty(options.MachineHeaderFileName))
                using (StreamWriter writer = File.CreateText(options.MachineHeaderFileName)) {
                    //MachineHeaderVisitor visitor = new MachineHeaderVisitor(writer, options);
                    //machine.AcceptVisitor(visitor);
                }
        }

        private void GenerateMachineCode(Machine machine) { 

            if (!String.IsNullOrEmpty(options.MachineCodeFileName))
                using (StreamWriter writer = File.CreateText(options.MachineCodeFileName)) {
                    //MachineCodeVisitor visitor = new MachineCodeVisitor(writer, options);
                    //machine.AcceptVisitor(visitor);
                }
        }
        
        private void GenerateStateHeader(Machine machine) {

            if (!String.IsNullOrEmpty(options.StateHeaderFileName))
                using (StreamWriter writer = File.CreateText(options.StateHeaderFileName)) {

                    CodeBuilder codeBuilder = new CodeBuilder();

                    string[] stdIncludes = { "stdint.h", "stdbool.h", "stdlib.h" };

                    string guardName = String.Format("__{0}__", Path.GetFileNameWithoutExtension(options.StateHeaderFileName));

                    string template =
                        "typedef void *Context;\n" +
                        "typedef void (*Action)(Context *context);\n" +
                        "typedef bool (*Guard)(Context *context);\n" +
                        "\n" +
                        "typedef struct {\n" +
                        "  Event event;\n" +
                        "  State next;\n" +
                        "  const Guard guard;\n" +
                        "  const Action action;\n" +
                        "} TransitionDescriptor;\n\n" +
                        "typedef struct {\n" +
                        "  State state;\n" +
                        "  const Action enter;\n" +
                        "  const Action exit;\n" +
                        "  uint8_t transitionOffset;\n" +
                        "  uint8_t transitionCount;\n" +
                        "} StateDescriptor;\n" +
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

                    codeBuilder
                        .WriteLine("#endif // {0}", guardName);

                    writer.Write(codeBuilder.ToString());
                }
        }
        
        private void GenerateStateCode(Machine machine) {

            if (!String.IsNullOrEmpty(options.StateCodeFileName))
                using (StreamWriter writer = File.CreateText(options.StateCodeFileName)) {

                    CodeBuilder codeBuilder = new CodeBuilder();

                    codeBuilder
                        .WriteLine("#include \"{0}\"", options.StateHeaderFileName)
                        .WriteLine()
                        .WriteLine();

                    StateCodeGenerator.GenerateActionImplementation(codeBuilder, machine);
                    StateCodeGenerator.GenerateGuardImplementation(codeBuilder, machine);
                    StateCodeGenerator.GenerateTransitionTable(codeBuilder, machine);
                    StateCodeGenerator.GenerateStateTable(codeBuilder, machine);

                    writer.Write(codeBuilder.ToString());
                }
        }
    }
}
