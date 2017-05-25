namespace MikroPicDesigns.FSMCompiler.CmdLine {

    using System;

    public sealed class ArgumentInfo {

        private readonly ArgumentDefinition definition;
        private readonly string value;

        public ArgumentInfo(ArgumentDefinition definition,  string value = null) {

            if (definition == null)
                throw new ArgumentNullException("definition");

            this.definition = definition;
            this.value = value;
        }

        public string Name {
            get {
                return definition.Name;
            }
        }

        public string Value {
            get {
                return value;
            }
        }
    }
}
