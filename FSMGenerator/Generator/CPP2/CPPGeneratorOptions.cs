﻿namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    public sealed class CPPGeneratorOptions {

        public string NsName { get; set; }
        public string ContextBaseClassName { get; set; }
        public string ContextClassName { get; set; }
        public string ContextHeaderFileName { get; set; }
        public string ContextCodeFileName { get; set; }
        public string StateBaseClassName { get; set; }
        public string StateClassName { get; set; }
        public string StateHeaderFileName { get; set; }
        public string StateCodeFileName { get; set; }

        public CPPGeneratorOptions() {

            NsName = "app";

            ContextBaseClassName = null;
            ContextClassName = "Context";

            StateBaseClassName = null;
            StateClassName = "State";

            ContextHeaderFileName = "fsmContext.h";
            ContextCodeFileName = "fsmContext.cpp";
            StateHeaderFileName = "fsmState.h";
            StateCodeFileName = "fsmState.cpp";
        }
    }
}
