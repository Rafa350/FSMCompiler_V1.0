namespace MikroPicDesigns.FSMCompiler.CmdLine {

    using System;

    public sealed class OptionDefinition {

        private readonly string name;
        private readonly string shortOption;
        private readonly string longOption;
        private readonly string description;
        private readonly bool required;
        private readonly bool multiple;

        public OptionDefinition(string name, string description = null, bool required = false, bool multiple = false) {

            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            this.name = name;
            this.shortOption = name.Substring(0, 1);
            this.longOption = name;
            this.description = description;
            this.required = required;
            this.multiple = multiple;
        }

        /// <summary>
        /// Obte el nom de l'opcio.
        /// </summary>
        /// 
        public string Name {
            get {
                return name;
            }
        }

        /// <summary>
        /// Text de l'opcio llarga (Ex: --opcio)
        /// </summary>
        /// 
        public string LongOption {
            get {
                return longOption;
            }
        }

        /// <summary>
        /// Text de l'opcio curta (Ex: -o)
        /// </summary>
        /// 
        public string ShortOption {
            get {
                return shortOption;
            }
        }

        /// <summary>
        /// Obte la descripcio de l'opcio.
        /// </summary>
        /// 
        public string Description {
            get {
                return description;
            }
        }

        /// <summary>
        /// Indica si es obligatori.
        /// </summary>
        /// 
        public bool Required {
            get {
                return required;
            }
        }

        /// <summary>
        /// Indica si pot apareixa diversos cops.
        /// </summary>
        /// 
        public bool Multiple {
            get {
                return multiple;
            }
        }
    }
}
