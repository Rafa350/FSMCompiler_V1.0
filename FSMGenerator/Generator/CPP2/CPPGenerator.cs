namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.IO;
    using System.Text;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeGenerator;

    public sealed class CPPGenerator: GeneratorBase {

        private readonly CPPGeneratorOptions options;

        public CPPGenerator(GeneratorParameters generatorParameters) {

            CPPGeneratorOptions options = new CPPGeneratorOptions();
            generatorParameters.Populate(options);
            this.options = options;
        }

        public CPPGenerator(CPPGeneratorOptions options) {

            if (options == null)
                options = new CPPGeneratorOptions();
            this.options = options;
        }

        public override void Generate(Machine machine) {

            UnitDeclaration contextUnit = ContextUnitGenerator.Generate(machine, options);
            GenerateContextHeader(contextUnit);
            GenerateContextCode(contextUnit);

            UnitDeclaration stateUnit = StateUnitGenerator.Generate(machine, options);
            GenerateStateHeader(stateUnit);
            GenerateStateCode(stateUnit);
        }

        private void GenerateContextHeader(UnitDeclaration unitDeclaration) {

            if (!String.IsNullOrEmpty(options.ContextHeaderFileName))
                using (StreamWriter writer = File.CreateText(options.ContextHeaderFileName)) {

                    string header = HeaderGenerator.Generate(unitDeclaration);

                    string guardString = Path.GetFileName(options.ContextHeaderFileName).ToUpper().Replace(".", "_");

                    StringBuilder sb = new StringBuilder();
                    sb
                        .AppendFormat("#ifndef __{0}", guardString).AppendLine()
                        .AppendFormat("#define __{0}", guardString).AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine("#include \"eos.h\"")
                        .AppendLine("#include \"Services/Fsm/eosFsmContextBase.h\"")
                        .AppendLine()
                        .AppendLine()
                        .AppendLine(header)
                        .AppendLine()
                        .AppendFormat("#endif // __{0}", guardString).AppendLine();

                    writer.Write(sb.ToString());
                }
        }

        private void GenerateContextCode(UnitDeclaration unitDeclaration) {

            if (!String.IsNullOrEmpty(options.ContextCodeFileName))
                using (StreamWriter writer = File.CreateText(options.ContextCodeFileName)) {

                    string code = CodeGenerator.Generate(unitDeclaration);

                    string contextHeaderFileName = Path.GetFileName(options.ContextHeaderFileName);
                    string stateHeaderFileName = Path.GetFileName(options.StateHeaderFileName);

                    StringBuilder sb = new StringBuilder();
                    sb
                        .AppendFormat("#include \"{0}\"", contextHeaderFileName).AppendLine()
                        .AppendFormat("#include \"{0}\"", stateHeaderFileName).AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .Append(code);

                    writer.Write(sb.ToString());
                }
        }

        private void GenerateStateHeader(UnitDeclaration unitDeclaration) {

            if (!String.IsNullOrEmpty(options.StateHeaderFileName))
                using (StreamWriter writer = File.CreateText(options.StateHeaderFileName)) {

                    string header = HeaderGenerator.Generate(unitDeclaration);

                    string guardString = Path.GetFileName(options.StateHeaderFileName).ToUpper().Replace(".", "_");

                    StringBuilder sb = new StringBuilder();
                    sb
                        .AppendFormat("#ifndef __{0}", guardString).AppendLine()
                        .AppendFormat("#define __{0}", guardString).AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .AppendLine("#include \"eos.h\"")
                        .AppendLine("#include \"Services/Fsm/eosFsmStateBase.h\"")
                        .AppendLine()
                        .AppendLine()
                        .AppendLine(header)
                        .AppendLine()
                        .AppendFormat("#endif // __{0}", guardString).AppendLine();

                    writer.Write(sb.ToString());
                }
        }

        private void GenerateStateCode(UnitDeclaration unitDeclaration) {

            if (!String.IsNullOrEmpty(options.StateCodeFileName))
                using (StreamWriter writer = File.CreateText(options.StateCodeFileName)) {

                    string code = CodeGenerator.Generate(unitDeclaration);

                    string contextHeaderFileName = Path.GetFileName(options.ContextHeaderFileName);
                    string stateHeaderFileName = Path.GetFileName(options.StateHeaderFileName);

                    StringBuilder sb = new StringBuilder();
                    sb
                        .AppendFormat("#include \"{0}\"", contextHeaderFileName).AppendLine()
                        .AppendFormat("#include \"{0}\"", stateHeaderFileName).AppendLine()
                        .AppendLine()
                        .AppendLine()
                        .Append(code);

                    writer.Write(sb.ToString());
                }
        }
    }
}
