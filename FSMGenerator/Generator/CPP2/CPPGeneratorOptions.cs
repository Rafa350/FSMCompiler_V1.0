namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    public sealed class CPPGeneratorOptions {

        public string NsName;
        public string ContextClassName;
        public string ContextHeaderFileName;
        public string MachineClassName;
        public string MachineHeaderFileName;
        public string MachineCodeFileName;
        public string StateClassName;
        public string StateHeaderFileName;
        public string StateCodeFileName;
        public bool UseStateNames;

        public CPPGeneratorOptions() {

            NsName = "app";

            MachineClassName = "Machine";
            StateClassName = "State";
            ContextClassName = "Context";

            ContextHeaderFileName = "fsmContext.h";
            MachineHeaderFileName = "fsmMachine.h";
            MachineCodeFileName = "fsmMachine.cpp";
            StateHeaderFileName = "fsmState.h";
            StateCodeFileName = "fsmState.cpp";
            
            UseStateNames = true;
        }
   }
    
}
