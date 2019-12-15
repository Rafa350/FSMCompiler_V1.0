namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP {

    public sealed class CPPGeneratorOptions {

        public string OutputPath { get; set; }
        public string MachineHeaderFileName { get; set; }
        public string MachineCodeFileName { get; set; }
        public string StateHeaderFileName { get; set; }
        public string EventHeaderFileName { get; set; }
        public int FirstStateNum { get; set; }
        public int FirstEventNum { get; set; }
        public bool UseStateNames { get; set; }
    }
}
