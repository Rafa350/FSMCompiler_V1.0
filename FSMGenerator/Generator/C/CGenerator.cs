namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    public sealed class CGeneratorOptions {

        private string machineHeaderFileName;
        private string machineCodeFileName;
        private string stateHeaderFileName;
        private string stateCodeFileName;
        private bool useStateNames;

        public CGeneratorOptions() {

            useStateNames = true;
        }

        public string MachineHeaderFileName {
            get { return machineHeaderFileName; }
            set { machineHeaderFileName = value; }
        }

        public string MachineCodeFileName {
            get { return machineCodeFileName; }
            set { machineCodeFileName = value; }
        }

        public string StateHeaderFileName {
            get { return stateHeaderFileName; }
            set { stateHeaderFileName = value; }
        }
        
        public string StateCodeFileName {
            get { return stateCodeFileName; }
            set { stateCodeFileName = value; }
        }

        public bool UseStateNames {
            get { return useStateNames; }
            set { useStateNames = value; }
        }
    }
    
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
                    StateCodeVisitor visitor = new StateCodeVisitor(writer, options);
                    machine.AcceptVisitor(visitor);
                }
        }
    }
}
