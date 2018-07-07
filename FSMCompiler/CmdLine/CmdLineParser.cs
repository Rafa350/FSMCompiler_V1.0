namespace MikroPicDesigns.FSMCompiler.CmdLine {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public sealed class CmdLineParser {

        private const string optionPrefix = "/";
        private const string optionValueSeparator = ":";

        private readonly string title;
        private readonly string description;
        private readonly bool caseSensitive = false;
        private readonly List<ArgumentDefinition> argumentDefinitions = new List<ArgumentDefinition>();
        private readonly Dictionary<string, OptionDefinition> optionDefinitions = new Dictionary<string, OptionDefinition>();
        private readonly List<OptionInfo> optionInfos = new List<OptionInfo>();
        private readonly List<ArgumentInfo> argumentInfos = new List<ArgumentInfo>();

        /// <summary>
        /// Constructor del objecte.
        /// </summary>
        /// <param name="title">Titol</param>
        /// <param name="description">Descripcio.</param>
        /// <param name="caseSensitive">Indica si te en compte may/min.</param>
        /// 
        public CmdLineParser(string title, string description = null, bool caseSensitive = false) {

            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");

            this.title = title;
            this.description = description;
            this.caseSensitive = caseSensitive;
        }

        /// <summary>
        /// Afegeix una definicio d'argument.
        /// </summary>
        /// <param name="argumentDefinition">L'argument a afeigir.</param>
        /// 
        public void Add(ArgumentDefinition argumentDefinition) {

            if (argumentDefinition == null)
                throw new ArgumentNullException("argumentDefinion");

            argumentDefinitions.Add(argumentDefinition);
        }

        /// <summary>
        /// Afegeix una definicio d'opcio.
        /// </summary>
        /// <param name="optionDefinition">L'opcio a afeigir.</param>
        /// 
        public void Add(OptionDefinition optionDefinition) {

            if (optionDefinition == null)
                throw new ArgumentNullException("optionDefinition");

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

                if (IsOption(arg)) {
                    OptionDefinition optionDefinition = FindOptionDefinition(arg);
                    OptionInfo optionInfo = new OptionInfo(
                        optionDefinition, 
                        GetOptionValue(arg));
                    optionInfos.Add(optionInfo);
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

        /// <summary>
        /// Comprova si una string es un argument.
        /// </summary>
        /// <param name="arg">La string a verificar.</param>
        /// <returns>El resultat de l'operacio.</returns>
        /// 
        private static bool IsOption(string arg) {

            return arg.StartsWith(optionPrefix);
        }

        private OptionDefinition FindOptionDefinition(string arg) {

            if (String.IsNullOrEmpty(arg))
                throw new ArgumentNullException("arg");

            if (!arg.StartsWith(optionPrefix))
                throw new InvalidOperationException("No es una opcion.");

            int len = arg.IndexOf(optionValueSeparator, 1);
            if (len == -1)
                len = arg.Length;
            string key = arg.Substring(1, len - 1);
            return optionDefinitions[key];
        }

        private ArgumentDefinition FindArgumentDefinition(string arg, int index) {

            if (String.IsNullOrEmpty(arg))
                throw new ArgumentNullException("arg");

            return argumentDefinitions[index];
        }

        private string GetOptionValue(string arg) {

            int p = arg.IndexOf(optionValueSeparator);
            if (p == -1)
                return null;
            else
                return arg.Substring(p + 1);
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
        /// Obte el indicador de may/min.
        /// </summary>
        /// 
        public bool CaseSensitive {
            get {
                return caseSensitive;
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
                if (!String.IsNullOrEmpty(title))
                    sb.AppendLine(title);
                if (!String.IsNullOrEmpty(description))
                    sb.AppendLine(description);
                foreach (ArgumentDefinition argument in argumentDefinitions) {
                    if (!argument.Required)
                        sb.Append('[');
                    sb.Append(argument.Name);
                    if (!argument.Required)
                        sb.Append(']');
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
