namespace MikroPicDesigns.FSMCompiler.v1.Generator {

    using System;
    using System.Text;

    internal sealed class CodeBuilder {

        private readonly StringBuilder sb = new StringBuilder();
        private int indentLevel = 0;

        public CodeBuilder Clear() {

            sb.Clear();
            return this;
        }

        public CodeBuilder Write(string text) {

            if (indentLevel > 0)
                sb.Append(new String(' ', indentLevel * 4));
            sb.Append(text);
            return this;
        }

        public CodeBuilder Write(string format, params object[] args) {

            if (indentLevel > 0)
                sb.Append(new String(' ', indentLevel * 4));
            sb.Append(String.Format(format, args));
            return this;
        }

        public CodeBuilder WriteLine(string text) {

            Write(text);
            return WriteLine();
        }

        public CodeBuilder WriteLine(string format, params object[] args) {

            Write(format, args);
            return WriteLine();
        }

        public CodeBuilder WriteLine() {

            sb.AppendLine();
            return this;
        }

        public CodeBuilder Indent() {

            indentLevel++;

            return this;
        }

        public CodeBuilder UnIndent() {

            if (indentLevel > 0)
                indentLevel--;

            return this;
        }

        public override string ToString() {

            return sb.ToString();
        }
    }
}
