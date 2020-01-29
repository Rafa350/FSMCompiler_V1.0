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
    }
}
