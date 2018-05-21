namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    public sealed class CGeneratorOptions {

        private string machineHeaderFileName;
        private string machineCodeFileName;
        private string stateHeaderFileName;
        private string stateCodeFileName;
        private bool useStateNames;

        public CGeneratorOptions() {

            useStateNames = true;
        }

        public string OutputPath { get; set; }

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
}
