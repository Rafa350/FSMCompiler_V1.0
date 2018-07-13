namespace MikroPicDesigns.FSMCompiler.v1.Generator.DOT {

    using MikroPicDesigns.FSMCompiler.v1.Model;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    public sealed class DOTGenerator: GeneratorBase {

        private const string nodeFont = "arial";
        private const int nodeFontSize = 14;
        private const string edgeFont = "arial";
        private const int edgeFontSize = 10;

        private readonly DOTGeneratorOptions options;

        public DOTGenerator(GeneratorParameters generatorParameters) {

            DOTGeneratorOptions options = new DOTGeneratorOptions();
            generatorParameters.Populate(options);
            this.options = options;
        }

        /// <summary>
        /// Genera el fitxer DOT
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// 
        public override void Generate(Machine machine) {

            string folder = options.OutputPath;
            if (String.IsNullOrEmpty(folder))
                folder = @".\";

            // Genera els noms de les guardes i de les accions
            //
            int actionCount = 0;
            int guardCount = 0;
            Dictionary<Model.Action, string> actionDict = new Dictionary<Model.Action, string>();
            Dictionary<Guard, string> guardDict = new Dictionary<Guard, string>();

            if (machine.InitializeAction != null)
                actionDict.Add(machine.InitializeAction, String.Format("Action{0}", actionCount++));

            if (machine.TerminateAction != null)
                actionDict.Add(machine.TerminateAction, String.Format("Action{0}", actionCount++));

            foreach (State state in machine.States) {

                if (state.EnterAction != null)
                    actionDict.Add(state.EnterAction, String.Format("Action{0}", actionCount++));

                if (state.ExitAction != null)
                    actionDict.Add(state.ExitAction, String.Format("Action{0}", actionCount++));

                foreach (Transition transition in state.Transitions) {

                    if (transition.Guard != null)
                        guardDict.Add(transition.Guard, String.Format("Guard{0}", guardCount++));

                    if (transition.Action != null)
                        actionDict.Add(transition.Action, String.Format("Action{0}", actionCount++));
                }
            }

            // Crea el firxer fsm_<machine>.dot
            //
            string fileName = String.Format("fsm_{0}.dot", machine.Name);
            string path = Path.Combine(folder, fileName);
            using (StreamWriter writer = File.CreateText(path)) {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("digraph {");
                sb.AppendLine("    ratio = \"fill\"");
                sb.AppendLine("    size = \"8.3,11.7!\"");
                sb.AppendLine("    margin = 0.5");
                sb.AppendLine("    fontname = \"arial\"");
                sb.AppendLine("    fontsize = 10");
                sb.AppendLine();

                sb.AppendLine("    node [");
                sb.AppendFormat("        fontname = \"{0}\",", nodeFont).AppendLine();
                sb.AppendFormat("        fontsize = {0},", nodeFontSize).AppendLine();
                sb.AppendLine("        shape = \"none\",");
                sb.AppendLine("        margin = 0");
                sb.AppendLine("    ]");
                sb.AppendLine();

                sb.AppendLine("    edge [");
                sb.AppendFormat("        fontname = \"{0}\",", edgeFont).AppendLine();
                sb.AppendFormat("        fontsize = {0}", edgeFontSize).AppendLine();
                sb.AppendLine("    ]");
                sb.AppendLine();

                // Declara el node 'START'
                //
                sb.AppendLine("    START [");
                sb.AppendLine("        label= \"\",");
                sb.AppendLine("        width = \"0.25\",");
                sb.AppendLine("        height = \"0.25\",");
                sb.AppendLine("        shape = \"circle\",");
                sb.AppendLine("        style = \"filled\",");
                sb.AppendLine("        fillcolor =\"black\"");
                sb.AppendLine("    ]");
                sb.AppendLine();

                // Crea els nodes de cada estat
                //
                foreach (State state in machine.States) {

                    bool needSeparator = true;

                    sb.AppendFormat("    {0} [", state.Name);
                    sb.AppendLine();
                    sb.Append("        label = <<table cellborder=\"0\" style=\"rounded\" bgcolor=\"lemonchiffon\">");
                    sb.AppendFormat("<tr><td><font point-size=\"{1}\"> {0} </font></td></tr>", state.Name, nodeFontSize);

                    // Transicio enter
                    //
                    if (state.EnterAction != null) {
                        if (needSeparator) {
                            needSeparator = false;
                            sb.Append("<hr/>");
                        }
                        sb.AppendFormat("<tr><td><font point-size=\"{1}\"> ENTRY/ {0} </font></td></tr>", actionDict[state.EnterAction], edgeFontSize);
                    }

                    // Transicio exit
                    //
                    if (state.ExitAction != null) {
                        if (needSeparator) {
                            needSeparator = false;
                            sb.Append("<hr/>");
                        }
                        sb.AppendFormat("<tr><td><font point-size=\"{1}\"> EXIT/ </font>{0} </td></tr>", actionDict[state.ExitAction], edgeFontSize);
                    }

                    // Transicions internes
                    //
                    foreach (Transition transition in state.Transitions) {
                        if (transition.NextState == null) {
                            if (needSeparator) {
                                needSeparator = false;
                                sb.Append("<hr/>");
                            }
                            sb.AppendFormat("<tr><td><font point-size=\"{2}\"> {0}/ {1} </font></td></tr>", transition.Event.Name, actionDict[transition.Action], edgeFontSize);
                        }
                    }

                    sb.Append("</table>>");
                    sb.AppendLine();
                    sb.Append("    ];");
                    sb.AppendLine();
                    sb.AppendLine();
                }
                sb.AppendLine();

                // Declara les transicions externes o auto-transicions
                //
                sb.AppendFormat("    START->{0}", machine.Start.Name).AppendLine();
                sb.AppendLine();

                foreach (State state in machine.States) {
                    foreach (Transition transition in state.Transitions) {
                        if (transition.NextState != null) {
                            sb.AppendFormat("    {0}->{1} [", state.Name, transition.NextState.Name).AppendLine();
                            sb.AppendFormat("        label = \"{0}", transition.Event.Name);
                            if (transition.Action != null)
                                sb.AppendFormat(" / {0}", actionDict[transition.Action]);
                            sb.Append("\"").AppendLine();
                            sb.Append("    ];");
                            sb.AppendLine();
                        }
                    }
                    sb.AppendLine();
                }
                sb.Append("}");

                writer.WriteLine(sb.ToString());
            }
        }
    }
}
