namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    internal class EventIdHeaderVisitor: DefaultVisitor {

        private readonly TextWriter writer;
        private readonly CPPGeneratorOptions options;
        private CodeBuilder codeBuilder = new CodeBuilder();
        private int count = 0;

        public EventIdHeaderVisitor(TextWriter writer, CPPGeneratorOptions options) {

            this.writer = writer;
            this.options = options;
        }

        public override void Visit(Machine machine) {

            string guardString = Path.GetFileName(options.EventIdHeaderFileName).ToUpper().Replace(".", "_");

            codeBuilder
                .WriteLine("#ifndef __{0}", guardString)
                .WriteLine("#define __{0}", guardString)
                .WriteLine()
                .WriteLine();

            foreach (Event ev in machine.Events)
                ev.AcceptVisitor(this);

            codeBuilder
                .WriteLine()
                .WriteLine()
                .WriteLine("#endif");

            writer.Write(codeBuilder.ToString());
        }

        public override void Visit(Event ev) {

            codeBuilder.WriteLine("#define EV_{0} {1} ", ev.Name, count++);
        }
    }
}

