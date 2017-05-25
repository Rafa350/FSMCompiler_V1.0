namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP {

    public sealed class CPPGeneratorOptions {

        private string machineClassName;
        private string machineBaseClassName;
        private string contextClassName;
        private string eventIdHeaderFileName;
        private string stateIdHeaderFileName;
        private string machineHeaderFileName;
        private string machineCodeFileName;
        private string stateBaseClassName;
        private string stateHeaderFileName;
        private string stateCodeFileName;
        private bool useStateNames;

        public CPPGeneratorOptions() {

            machineBaseClassName = "eos::fsm::Machine";
            stateBaseClassName = "eos::fsm::State";
            contextClassName = "eos::fsm::IContext";
            
            useStateNames = true;
        }

        public string MachineClassName {
            get { return machineClassName; }
            set { machineClassName = value; }
        }

        public string MachineBaseClassName {
            get { return machineBaseClassName; }
            set { machineBaseClassName = value; }
        }

        public string ContextClassName {
            get { return contextClassName; }
            set { contextClassName = value; }
        }

        public string EventIdHeaderFileName {
            get { return eventIdHeaderFileName; }
            set { eventIdHeaderFileName = value; }
        }

        public string StateIdHeaderFileName {
            get { return stateIdHeaderFileName; }
            set { stateIdHeaderFileName = value; }
        }

        public string MachineHeaderFileName {
            get { return machineHeaderFileName; }
            set { machineHeaderFileName = value; }
        }

        public string MachineCodeFileName {
            get { return machineCodeFileName; }
            set { machineCodeFileName = value; }
        }

        public string StateBaseClassName {
            get { return stateBaseClassName; }
            set { stateBaseClassName = value; }
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
