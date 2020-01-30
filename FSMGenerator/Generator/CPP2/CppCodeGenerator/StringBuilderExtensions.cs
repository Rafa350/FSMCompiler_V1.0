namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeGenerator {

    using System.Text;

    internal static class StringBuilderExtensions {

        public static StringBuilder AppendIndent(this StringBuilder sb, int level) {

            while (level > 0) {
                sb.Append("    ");
                level -= 1;
            }

            return sb;
        }

        public static StringBuilder AppendLine(this StringBuilder sb, string format, params object[] args) {

            sb.AppendFormat(format, args);
            sb.AppendLine();

            return sb;
        }
    }
}
