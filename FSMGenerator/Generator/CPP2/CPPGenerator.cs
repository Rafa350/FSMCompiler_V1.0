namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;
 
    public sealed class CPPGenerator: GeneratorBase {

        private readonly CPPGeneratorOptions options;

        public CPPGenerator(GeneratorParameters generatorParameters) {

            CPPGeneratorOptions options = new CPPGeneratorOptions();
            generatorParameters.Populate(options);
            this.options = options;
        }

        public CPPGenerator(CPPGeneratorOptions options) {

            if (options == null)
                options = new CPPGeneratorOptions();
            this.options = options;
        }

        public override void Generate(Machine machine) {

            GenerateMachineHeader(machine);
            GenerateMachineCode(machine);
            GenerateStateHeader(machine);
            GenerateStateCode(machine);
        }

        private void GenerateMachineHeader(Machine machine) {

            if (!String.IsNullOrEmpty(options.ContextHeaderFileName))
                using (StreamWriter writer = File.CreateText(options.ContextHeaderFileName)) {
                    ContextHeaderVisitor visitor = new ContextHeaderVisitor(writer, options);
                    machine.AcceptVisitor(visitor);
                }
        }

        private void GenerateMachineCode(Machine machine) { 

            if (!String.IsNullOrEmpty(options.ContextCodeFileName))
                using (StreamWriter writer = File.CreateText(options.ContextCodeFileName)) {
                    ContextCodeVisitor visitor = new ContextCodeVisitor(writer, options);
                    machine.AcceptVisitor(visitor);
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
                    StateCodeVisitor visitor = new StateCodeVisitor(writer, options);
                    machine.AcceptVisitor(visitor);
                }
        }
    }
}
