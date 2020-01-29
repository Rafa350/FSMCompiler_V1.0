namespace MikroPicDesigns.FSMCompiler.v1.Generator {

    using System;
    
    public sealed class CppCodeBuilder: CodeBuilder {

        public enum ProtectionLevel {
            Private,
            Protected,
            Public
        }

        private string guard;
        private bool openClass = false;
        private bool openSection = false;

        public CppCodeBuilder WriteInclude(string fileName) {

            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            WriteLine("#include \"{0}\"", fileName);

            return this;
        }

        public CppCodeBuilder WriteIncludeStartGuard(string guard) {

            if (String.IsNullOrEmpty(guard))
                throw new ArgumentNullException("guard");

            WriteLine("#ifndef __{0}", guard);
            WriteLine("#define __{0}", guard);

            this.guard = guard;

            return this;
        }

        public CppCodeBuilder WriteIncludeEndGuard() {

            if (String.IsNullOrEmpty(guard))
                throw new Exception("No se llamo a 'WriteIncludeStartGuard'.");

            WriteLine("#endif // __{0}", guard);

            guard = null;

            return this;
        }

        public CppCodeBuilder WriteUsingNamespace(string nsName) {

            WriteLine("using namespace {0};", nsName);

            return this;
        }

        public CppCodeBuilder WriteBeginNamespace(string nsName) {

            WriteLine("namespace {0} {{", nsName);
            Indent();

            return this;
        }

        public CppCodeBuilder WriteEndNamespace() {

            UnIndent();
            WriteLine("}");

            return this;
        }

        public CppCodeBuilder WriteForwardClassDeclaration(string className) {

            WriteLine("class {0};", className);

            return this;
        }

        public CppCodeBuilder WriteBeginClassDeclaration(string className) {

            WriteLine("class {0} {{", className);
            Indent();

            openClass = true;

            return this;
        }

        public CppCodeBuilder WriteBeginClassDeclaration(string className, string baseClassName) {

            if (String.IsNullOrEmpty(baseClassName))
                WriteLine("class {0} {{", className);
            else 
                WriteLine("class {0}: {1} {2} {{", className, "public", baseClassName);
            Indent();

            openClass = true;

            return this;
        }

        public CppCodeBuilder WriteEndClassDeclaration() {

            if (openSection)
                WriteEndClassSection();

            UnIndent();
            WriteLine("};");

            openClass = false;

            return this;
        }

        public CppCodeBuilder WriteBeginClassSection(ProtectionLevel protectionLevel) {

            if (openSection)
                UnIndent();

            WriteLine("{0}:", 
                (protectionLevel == ProtectionLevel.Private)   ? "private" :
                (protectionLevel == ProtectionLevel.Protected) ? "protected" : 
                                                                 "public");
            Indent();

            openSection = true;

            return this;
        }

        public CppCodeBuilder WriteEndClassSection() {

            if (openSection)
                UnIndent();

            UnIndent();

            openSection = false;

            return this;
        }
    }
}
