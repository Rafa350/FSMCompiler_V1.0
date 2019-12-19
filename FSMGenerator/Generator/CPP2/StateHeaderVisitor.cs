namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    internal sealed class StateHeaderVisitor: DefaultVisitor {

        private readonly TextWriter writer;
        private readonly CPPGeneratorOptions options;
        private CodeBuilder codeBuilder = new CodeBuilder();

        public StateHeaderVisitor(TextWriter writer, CPPGeneratorOptions options) {

            this.writer = writer;
            this.options = options;
        }
        public override void Visit(Machine machine) {

            string guardString = Path.GetFileName(options.StateHeaderFileName).ToUpper().Replace(".", "_");

            codeBuilder
                .WriteLine("#ifndef __{0}", guardString)
                .WriteLine("#define __{0}", guardString)
                .WriteLine()
                .WriteLine()
                .WriteLine("#include \"fsmDefines.h\"")
                .WriteLine()
                .WriteLine();

            codeBuilder
                .WriteLine("class {0} {{", options.StateBaseClassName)
                .Indent()
                .WriteLine("public:")
                .Indent()
                .WriteLine("{0}({1} *machine);", options.StateBaseClassName, options.MachineBaseClassName)
                .WriteLine("virtual void enter();")
                .WriteLine("virtual void exit();")
                .WriteLine("virtual void transition(unsigned eventId);");

            codeBuilder
                .UnIndent()
                .UnIndent()
                .UnIndent()
                .WriteLine("};")
                .WriteLine();

            foreach (State state in machine.States)
                state.AcceptVisitor(this);

            codeBuilder
                .WriteLine()
                .WriteLine("#endif");

            writer.Write(codeBuilder.ToString());
        }

        public override void Visit(State state) {

            codeBuilder
                .WriteLine("class {0}State: public {1} {{", state.FullName, options.StateBaseClassName)
                .Indent()
                .WriteLine("public:")
                .Indent()
                .WriteLine("{0}State({1} *machine);", state.FullName, options.MachineBaseClassName);
            
            if (state.EnterAction != null) 
                codeBuilder.WriteLine("void enter() override;");
            if (state.ExitAction != null)
                codeBuilder.WriteLine("void exit() override;");

            codeBuilder
                .WriteLine("void transition(unsigned eventId) override;")
                .UnIndent()
                .UnIndent()
                .WriteLine("};")
                .WriteLine()
                .UnIndent();
        }
    }
}

