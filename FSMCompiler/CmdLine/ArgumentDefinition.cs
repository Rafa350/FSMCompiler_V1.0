namespace MikroPicDesigns.FSMCompiler.CmdLine {

    using System;

    public sealed class ArgumentDefinition {

        private readonly string name;
        private readonly string description;
        private readonly int index;
        private readonly bool required;

        public ArgumentDefinition(string name, int index, string description = null, bool required = false) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.name = name;
            this.index = index;
            this.description = description;
            this.required = required;
        }

        public string Name {
            get {
                return name;
            }
        }

        public int Index {
            get {
                return index;
            }
        }

        public string Description {
            get {
                return description;
            }
        }

        public bool Required {
            get {
                return required;
            }
        }
    }
}

