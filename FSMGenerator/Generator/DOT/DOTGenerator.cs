namespace MikroPicDesigns.FSMCompiler.v1.Generator.DOT {

    using MikroPicDesigns.FSMCompiler.v1.Model;
    using System;
    using System.Text;
    using System.IO;

    public sealed class DOTGenerator: GeneratorBase {

        private readonly DOTGeneratorOptions options;

        public DOTGenerator(GeneratorParameters generatorParameters) {

            DOTGeneratorOptions options = new DOTGeneratorOptions();
            generatorParameters.Populate(options);
            this.options = options;
        }

        public override void Generate(Machine machine) {

            string folder = options.OutputPath;
            if (String.IsNullOrEmpty(folder))
                folder = @".\";

            // Crea el firxer fsm_<machine>_states.h
            //
            string fileName = String.Format("fsm_{0}.dot", machine.Name);
            string path = Path.Combine(folder, fileName);
            using (StreamWriter writer = File.CreateText(path)) {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("digraph {");
                sb.AppendLine("    ratio = \"fill\";");
                sb.AppendLine("    size = \"8.3,11.7!\";");
                sb.AppendLine("    margin = 0.5;");
                sb.AppendLine();
                foreach (State state in machine.States) {
                    foreach (Transition transition in state.Transitions) {
                        sb.Append("    ");
                        sb.Append(state.Name);
                        sb.Append(" -> ");
                        sb.AppendFormat("{0}[", transition.NextState == null ? state.Name : transition.NextState.Name);
                        sb.AppendFormat("label={0}", transition.Event.Name);
                        sb.Append("];");
                        sb.AppendLine();
                    }
                }
                sb.Append("}");

                writer.WriteLine(sb.ToString());
            }
        }
    }
}
