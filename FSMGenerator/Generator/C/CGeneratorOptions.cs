namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    public sealed class CGeneratorOptions {

        public string OutputPath { get; set; }
        public string OutputType { get; set; }
        public string IncludeFileName { get; set; }
        public string MachineHeaderFileName { get; set; }
        public string MachineCodeFileName { get; set; }
        public string StateHeaderFileName { get; set; }
        public string EventHeaderFileName { get; set; }
        public int FirstStateNum { get; set; }
        public int FirstEventNum { get; set; }
        public bool UseStateNames { get; set; }
    }
}
