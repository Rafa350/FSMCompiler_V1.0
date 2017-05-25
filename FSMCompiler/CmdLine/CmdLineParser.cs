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

        public CmdLineParser(string title, string description = null, bool caseSensitive = false) {

            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");

            this.title = title;
            this.description = description;
            this.caseSensitive = caseSensitive;
        }

        public void Add(ArgumentDefinition argumentDefinition) {

            if (argumentDefinition == null)
                throw new ArgumentNullException("argumentDefinion");

            argumentDefinitions.Add(argumentDefinition);
        }

        public void Add(OptionDefinition optionDefinition) {

            if (optionDefinition == null)
                throw new ArgumentNullException("optionDefinition");

            optionDefinitions.Add(optionDefinition.Name, optionDefinition);
        }

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

        public string Title {
            get {
                return title;
            }
        }

        public string Description {
            get {
                return description;
            }
        }

        public bool CaseSensitive {
            get {
                return caseSensitive;
            }
        }

        public IEnumerable<OptionInfo> Options {
            get {
                return optionInfos;
            }
        }

        public IEnumerable<ArgumentInfo> Arguments {
            get {
                return argumentInfos;
            }
        }

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
