namespace MikroPicDesigns.FSMCompiler.CmdLine {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public enum CmdLineStyle {
        Windows,
        Unix
    }

    public enum CmdLineMode {
        Upper,
        Lowwer,
        Mixed,
        Insensitive
    }

    public sealed class CmdLineParser {

        private const string optionWindowsPrefix = "/";
        private const string optionUnixPrefix = "-";
        private const string optionValueSeparator = ":";
        private const string loadFromFileOption = "@";

        private readonly string title;
        private readonly string description;
        private readonly CmdLineMode mode = CmdLineMode.Insensitive;
        private readonly CmdLineStyle style = CmdLineStyle.Windows;
        private readonly string optionPrefix;
        private readonly List<ArgumentDefinition> argumentDefinitions = new List<ArgumentDefinition>();
        private readonly Dictionary<string, OptionDefinition> optionDefinitions = new Dictionary<string, OptionDefinition>();
        private readonly List<OptionInfo> optionInfos = new List<OptionInfo>();
        private readonly List<ArgumentInfo> argumentInfos = new List<ArgumentInfo>();

        /// <summary>
        /// Constructor del objecte.
        /// </summary>
        /// <param name="title">Titol</param>
        /// <param name="description">Descripcio.</param>
        /// <param name="mode">Modus d'interpretacio may/min.</param>
        /// <param name="style">Estil windows o unix.</param>
        /// 
        public CmdLineParser(string title, string description = null, CmdLineMode mode = CmdLineMode.Insensitive, CmdLineStyle style = CmdLineStyle.Windows) {

            if (String.IsNullOrEmpty(title)) {
                throw new ArgumentNullException("title");
            }

            this.title = title;
            this.description = description;
            this.mode = mode;
            this.style = style;

            optionPrefix = style == CmdLineStyle.Windows ? optionWindowsPrefix : optionUnixPrefix;
        }

        /// <summary>
        /// Afegeix una definicio d'argument.
        /// </summary>
        /// <param name="argumentDefinition">L'argument a afeigir.</param>
        /// 
        public void Add(ArgumentDefinition argumentDefinition) {

            if (argumentDefinition == null) {
                throw new ArgumentNullException("argumentDefinion");
            }

            argumentDefinitions.Add(argumentDefinition);
        }

        /// <summary>
        /// Afegeix una definicio d'opcio.
        /// </summary>
        /// <param name="optionDefinition">L'opcio a afeigir.</param>
        /// 
        public void Add(OptionDefinition optionDefinition) {

            if (optionDefinition == null) {
                throw new ArgumentNullException("optionDefinition");
            }

            optionDefinitions.Add(optionDefinition.Name, optionDefinition);
        }

        /// <summary>
        /// Procesa una llista d'arguments.
        /// </summary>
        /// <param name="args">Llista d'arguments.</param>
        /// 
        public void Parse(string[] args) {

            int argumentIndex = 0;
            foreach (string arg in args) {

                if (arg.StartsWith(optionPrefix)) {
                    OptionDefinition optionDefinition = FindOptionDefinition(arg);
                    OptionInfo optionInfo = new OptionInfo(
                        optionDefinition,
                        GetOptionValue(arg));
                    optionInfos.Add(optionInfo);
                }
                else if (arg.StartsWith(loadFromFileOption)) {

                }
                else {
                    ArgumentDefinition argumentDefinition = FindArgumentDefinition(arg, argumentIndex);
                    ArgumentInfo argumentInfo = new ArgumentInfo(
                        argumentDefinition,
                        arg);
                    argumentInfos.Add(argumentInfo);
                    argumentIndex += 1;
                }
            }
        }

        private void LoadFromFile(string fileName) {

            StreamReader reader = File.OpenText(fileName);
            string fileContent = reader.ReadToEnd();
        }

        private OptionDefinition FindOptionDefinition(string arg) {

            if (String.IsNullOrEmpty(arg)) {
                throw new ArgumentNullException("arg");
            }

            if (!arg.StartsWith(optionPrefix)) {
                throw new InvalidOperationException("No es una opcion.");
            }

            int len = arg.IndexOf(optionValueSeparator, 1);
            if (len == -1) {
                len = arg.Length;
            }

            string key = arg.Substring(1, len - 1);
            return optionDefinitions[key];
        }

        private ArgumentDefinition FindArgumentDefinition(string arg, int index) {

            if (String.IsNullOrEmpty(arg)) {
                throw new ArgumentNullException("arg");
            }

            return argumentDefinitions[index];
        }

        private string GetOptionValue(string arg) {

            int p = arg.IndexOf(optionValueSeparator);
            if (p == -1) {
                return null;
            }
            else {
                return arg.Substring(p + 1);
            }
        }

        /// <summary>
        /// Obpla les dades de les opcions i els arguments.
        /// </summary>
        /// <param name="data">Estructuda de dades a omplir.</param>
        /// <param name="ignoreIfNoExists">Ignora les opcions inexistens en la estructura de dades.</param>
        /// <param name="throwOnError">Dispara una excepcio en cas d'error.</param>
        /// <returns>True si tot es correcte.</returns>
        /// 
        public bool Pupulate(object data, bool ignoreIfNoExists = false, bool throwOnError = true) {

            if (data == null) {
                throw new ArgumentNullException("data");
            }

            try {
                Type dataType = data.GetType();

                foreach (OptionInfo optionInfo in Options) {
                    PropertyInfo propInfo = dataType.GetProperty(optionInfo.Name, BindingFlags.Instance | BindingFlags.Public);
                    if (propInfo != null) {
                        object value = Convert.ChangeType(optionInfo.Value, propInfo.PropertyType);
                        propInfo.SetValue(data, value, null);
                    }
                    else if (!ignoreIfNoExists) {
                        throw new InvalidOperationException(
                            String.Format("No se encontro la propiedad '{0}'.", optionInfo.Name));
                    }
                }

                return true;
            }
            catch {
                if (throwOnError) {
                    throw;
                }
                else {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obte el titol
        /// </summary>
        /// 
        public string Title {
            get {
                return title;
            }
        }

        /// <summary>
        /// Obte la descripcio.
        /// </summary>
        /// 
        public string Description {
            get {
                return description;
            }
        }

        /// <summary>
        /// Modus d'interpretacio de may/min.
        /// </summary>
        /// 
        public CmdLineMode Case {
            get {
                return mode;
            }
        }

        /// <summary>
        /// Estil windows, unix, etc.
        /// </summary>
        /// 
        public CmdLineStyle Style {
            get {
                return style;
            }
        }

        /// <summary>
        /// Enumera les opcions definides.
        /// </summary>
        /// 
        public IEnumerable<OptionInfo> Options {
            get {
                return optionInfos;
            }
        }

        /// <summary>
        /// Enumera els arguments definits.
        /// </summary>
        /// 
        public IEnumerable<ArgumentInfo> Arguments {
            get {
                return argumentInfos;
            }
        }

        /// <summary>
        /// Obte la cadena d'ajuda.
        /// </summary>
        /// 
        public string HelpText {
            get {
                StringBuilder sb = new StringBuilder();
                if (!String.IsNullOrEmpty(title)) {
                    sb.AppendLine(title);
                }

                if (!String.IsNullOrEmpty(description)) {
                    sb.AppendLine(description);
                }

                foreach (ArgumentDefinition argument in argumentDefinitions) {
                    if (!argument.Required) {
                        sb.Append('[');
                    }

                    sb.Append(argument.Name);
                    if (!argument.Required) {
                        sb.Append(']');
                    }

                    sb.AppendFormat("\t{0}", argument.Description);
                    sb.AppendLine();
                }
                foreach (KeyValuePair<string, OptionDefinition> kv in optionDefinitions) {
                    OptionDefinition option = kv.Value;
                    sb.AppendFormat("{0}{1}\t{2}", optionPrefix, option.Name, option.Description);
                    sb.AppendLine();
                }
                return sb.ToString();
            }
        }

        public string InfoText {
            get {
                StringBuilder sb = new StringBuilder();

                return sb.ToString();
            }
        }
    }
}
