namespace MicroCompiler.CodeGenerator {

    using System.Text;

    public sealed class CodeBuilder {

        private int indent = 0;
        private string indentString = "    ";

        private readonly StringBuilder sb = new StringBuilder();

        public CodeBuilder ResetIndent() {

            indent = 0;

            return this;
        }

        public CodeBuilder Indent() {

            indent++;

            return this;
        }

        public CodeBuilder Unindent() {

            indent--;

            return this;
        }

        public CodeBuilder WriteIndent() {

            for (int i = 0; i < indent; i++)
                sb.Append(indentString);

            return this;
        }

        public CodeBuilder Write(char ch) {

            sb.Append(ch);

            return this;
        }

        public CodeBuilder Write(string str) {

            sb.Append(str);

            return this;
        }

        public CodeBuilder Write(string format, params object[] objs) {

            sb.AppendFormat(format, objs);

            return this;
        }

        public CodeBuilder WriteLine(char ch) {

            return WriteIndent().Write(ch).WriteLine();
        }

        public CodeBuilder WriteLine(string str) {

            return WriteIndent().Write(str).WriteLine();
        }

        public CodeBuilder WriteLine(string format, params object[] objs) {

            return WriteIndent().Write(format, objs).WriteLine();
        }

        public CodeBuilder WriteLine() {

            sb.AppendLine();

            return this;
        }

        public override string ToString() {

            return sb.ToString();
        }

        public string IndentString { 
            get {
                return indentString;
            }
            set {
                indentString = value;
            }
        }
    }
}
