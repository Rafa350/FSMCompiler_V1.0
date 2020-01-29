namespace MikroPicDesigns.FSMCompiler.v1.Generator {

    using System;
    using System.Text;

    /// <summary>
    /// Clase que implementa un generador de codi font.
    /// </summary>
    public class CodeBuilder {

        private readonly StringBuilder sb = new StringBuilder();
        private int indentLevel = 0;

        /// <summary>
        /// Borra el codi generat.
        /// </summary>
        /// <returns>El propi objecte.</returns>
        /// 
        public CodeBuilder Clear() {

            sb.Clear();

            return this;
        }

        /// <summary>
        /// Genera una linia de codi.
        /// </summary>
        /// <param name="level">Nivell d'identacio.</param>
        /// <param name="fmt">Text amb parametres de format.</param>
        /// <param name="args">Parametres opcionals.</param>
        /// <returns>El propi objecte.</returns>
        /// 
        public CodeBuilder Write(int level, string fmt, params object[] args) {

            if (level > 0)
                sb.Append(new String(' ', level * 4));
            sb.Append(String.Format(fmt, args));

            return this;
        }

        /// <summary>
        /// Genera una linia de codi a genera un salt de linia.
        /// </summary>
        /// <param name="level">Nivell d'identacio.</param>
        /// <param name="fmt">Text amb parametres de format.</param>
        /// <param name="args">Parametres opcionals.</param>
        /// <returns>El propi objecte.</returns>
        /// 
        public CodeBuilder WriteLine(int level, string fmt, params object[] args) {

            Write(level, fmt, args);
            sb.AppendLine();

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
