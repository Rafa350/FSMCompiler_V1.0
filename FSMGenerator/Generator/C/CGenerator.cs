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
                    StateHeaderVisitor visitor = new StateHeaderVisitor(writer, options);
                    machine.AcceptVisitor(visitor);
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
