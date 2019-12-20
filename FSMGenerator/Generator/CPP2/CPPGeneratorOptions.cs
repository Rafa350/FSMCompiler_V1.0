namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    public sealed class CPPGeneratorOptions {

        private string machineClassName;
        private string contextClassName;
        private string eventIdHeaderFileName;
        private string stateIdHeaderFileName;
        private string machineHeaderFileName;
        private string machineCodeFileName;
        private string stateClassName;
        private string stateHeaderFileName;
        private string stateCodeFileName;
        private bool useStateNames;

        public CPPGeneratorOptions() {

            machineClassName = "Machine";
            stateClassName = "State";
            contextClassName = "IContext";
            
            machineHeaderFileName = "fsmMachine.h";
            machineCodeFileName = "fsmMachine.cpp";
            stateHeaderFileName = "fsmState.h";
            stateCodeFileName = "fsmState.cpp";
            
            useStateNames = true;
        }

        public string MachineClassName {
            get { return machineClassName; }
            set { machineClassName = value; }
        }

        public string StateClassName {
            get { return stateClassName; }
            set { stateClassName = value; }
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
