namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    internal sealed class StateHeaderVisitor: DefaultVisitor {

        private readonly TextWriter writer;
        private readonly CPPGeneratorOptions options;
        private readonly CppCodeBuilder codeBuilder = new CppCodeBuilder();

        public StateHeaderVisitor(TextWriter writer, CPPGeneratorOptions options) {

            this.writer = writer;
            this.options = options;
        }

        public override void Visit(Machine machine) {

            string guard = Path.GetFileName(options.StateHeaderFileName).ToUpper().Replace(".", "_");

            codeBuilder
                .WriteIncludeStartGuard(guard)
                .WriteLine()
                .WriteLine();

            codeBuilder
                .WriteInclude("eos.h")
                .WriteInclude("Services/Fsm/eosFsmStateBase.h")
                .WriteLine()
                .WriteLine();

            if (!String.IsNullOrEmpty(options.NsName))
                codeBuilder
                    .WriteBeginNamespace(options.NsName)
                    .WriteLine();

            codeBuilder
                .WriteForwardClassDeclaration(options.ContextClassName)
                .WriteLine();

            codeBuilder
                .WriteBeginClassDeclaration(options.StateClassName, options.StateBaseClassName);

            codeBuilder
                .WriteBeginClassSection(CppCodeBuilder.ProtectionLevel.Protected)
                .WriteLine("{0}();", options.StateClassName);
            codeBuilder
                .WriteEndClassSection();

            codeBuilder
                .WriteBeginClassSection(CppCodeBuilder.ProtectionLevel.Public);

            foreach(string transitionName in machine.GetTransitionNames())
                codeBuilder
                    .WriteLine("virtual void on{0}({1}* context);", transitionName, options.ContextClassName);

            codeBuilder
                .WriteEndClassSection()
                .WriteEndClassDeclaration()
                .WriteLine();

            foreach (State state in machine.States)
                state.AcceptVisitor(this);

            if (!String.IsNullOrEmpty(options.NsName))
                codeBuilder
                    .WriteEndNamespace();

            codeBuilder
                .WriteLine()
                .WriteLine();
            codeBuilder
                .WriteIncludeEndGuard();

            writer.Write(codeBuilder.ToString());
        }

        public override void Visit(State state) {

            codeBuilder
                .WriteBeginClassDeclaration(state.FullName, options.StateBaseClassName)
                .WriteBeginClassSection(CppCodeBuilder.ProtectionLevel.Private)
                .WriteLine("static {0}* instance;", state.FullName);

            codeBuilder
                .WriteEndClassSection()
                .WriteBeginClassSection(CppCodeBuilder.ProtectionLevel.Private)
                .WriteLine("{0}();", state.FullName);

            codeBuilder
                .WriteEndClassSection()
                .WriteBeginClassSection(CppCodeBuilder.ProtectionLevel.Public)
                .WriteLine("static {0}* getInstance();", state.FullName);
            
            foreach (string transitionName in state.GetTransitionNames()) {
                codeBuilder
                    .WriteLine("void on{0}({1}* context) override;", transitionName, options.ContextClassName);
            }

            codeBuilder
                .WriteEndClassSection()
                .WriteEndClassDeclaration()
                .WriteLine();
        }
    }
}

