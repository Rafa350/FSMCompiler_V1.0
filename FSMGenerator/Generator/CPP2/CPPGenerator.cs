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

            GenerateStateIdHeader(machine);
            GenerateEventIdHeader(machine);
            GenerateMachineHeader(machine);
            GenerateMachineCode(machine);
            GenerateStateHeader(machine);
            GenerateStateCode(machine);
        }

        private void GenerateStateIdHeader(Machine machine) {

            if (!String.IsNullOrEmpty(options.StateIdHeaderFileName))
                using (StreamWriter writer = File.CreateText(options.StateIdHeaderFileName)) {
                    StateIdHeaderVisitor visitor = new StateIdHeaderVisitor(writer, options);
                    machine.AcceptVisitor(visitor);
                }
        }

        private void GenerateEventIdHeader(Machine machine) {

            if (!String.IsNullOrEmpty(options.EventIdHeaderFileName))
                using (StreamWriter writer = File.CreateText(options.EventIdHeaderFileName)) {
                    EventIdHeaderVisitor visitor = new EventIdHeaderVisitor(writer, options);
                    machine.AcceptVisitor(visitor);
                }
        }

        private void GenerateMachineHeader(Machine machine) {

            if (!String.IsNullOrEmpty(options.MachineHeaderFileName))
                using (StreamWriter writer = File.CreateText(options.MachineHeaderFileName)) {
                    MachineHeaderVisitor visitor = new MachineHeaderVisitor(writer, options);
                    machine.AcceptVisitor(visitor);
                }
        }

        private void GenerateMachineCode(Machine machine) { 

            if (!String.IsNullOrEmpty(options.MachineCodeFileName))
                using (StreamWriter writer = File.CreateText(options.MachineCodeFileName)) {
                    MachineCodeVisitor visitor = new MachineCodeVisitor(writer, options);
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
