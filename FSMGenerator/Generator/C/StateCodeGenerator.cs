namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Commands;
    using System.Text;

    internal static class StateCodeGenerator {

        /// <summary>
        /// Genera la taula d'estats.
        /// </summary>
        /// <param name="codeBuilder">Constructor de codi font.</param>
        /// <param name="machine">La maquina a generar.</param>
        /// 
        public static void GenerateStateDescriptorTable(CodeBuilder codeBuilder, Machine machine) {

            codeBuilder
                .WriteLine("// -----------------------------------------------------------------------")
                .WriteLine("// State descriptor table.")
                .WriteLine("//")
                .WriteLine("static const StateDescriptor states[] = {")
                .Indent();

            int actionNum = 0;
            int transitionOffset = 0;
            foreach (State state in machine.States) {

                StringBuilder sb = new StringBuilder();
                sb.Append("{ ");
                sb.AppendFormat("State_{0}, ", state.Name);
                if (state.EnterAction == null)
                    sb.Append("NULL, ");
                else
                    sb.AppendFormat("Action{0}, ", actionNum++);
                if (state.ExitAction == null)
                    sb.Append("NULL, ");
                else
                    sb.AppendFormat("Action{0}, ", actionNum++);

                if (state.HasTransitions)
                    sb.AppendFormat("{0}, ", transitionOffset);
                else
                    sb.Append("0, ");

                int transitionCount = 0;
                foreach (Transition transition in state.Transitions)
                    transitionCount++;

                sb.AppendFormat("{0}", transitionCount);

                sb.Append(" }, ");

                codeBuilder.WriteLine(sb.ToString());

                foreach (Transition transition in state.Transitions) { 
                    transitionOffset++;
                    if (transition.Action != null)
                        actionNum++;
                }
            }
            codeBuilder
                .UnIndent()
                .WriteLine("};")
                .WriteLine();
        }

        /// <summary>
        /// Genera la taula de transicions.
        /// </summary>
        /// <param name="codeBuilder">Constructor de codi font.</param>
        /// <param name="machine">La maquina.</param>
        /// 
        public static void GenerateTransitionDescriptorTable(CodeBuilder codeBuilder, Machine machine) {

            codeBuilder
                .WriteLine("// -----------------------------------------------------------------------")
                .WriteLine("// Transition descriptor table.")
                .WriteLine("//")
                .WriteLine("static const TransitionDescriptor transitions[] = {")
                .Indent();

            int actionNum = 0;
            int guardNum = 0;            
            foreach (State state in machine.States) {
                if (state.EnterAction != null)
                    actionNum++;
                if (state.ExitAction != null)
                    actionNum++;
                foreach (Transition transition in state.Transitions) {

                    StringBuilder sb = new StringBuilder();
                    sb.Append("{ ");
                    sb.AppendFormat("Event_{0}, ",transition.Event.Name);
                    if (transition.NextState == null)
                        sb.AppendFormat("State_{0}, ", state.Name);
                    else
                        sb.AppendFormat("State_{0}, ", transition.NextState.Name);
                    if (transition.Guard == null)
                        sb.Append("NULL, ");
                    else
                        sb.AppendFormat("Guard{0}, ", guardNum++);
                    if (transition.Action == null)
                        sb.Append("NULL");
                    else
                        sb.AppendFormat("Action{0}", actionNum++);
                    sb.Append(" },");

                    codeBuilder.WriteLine(sb.ToString());
                }
            }
            codeBuilder
                .UnIndent()
                .WriteLine("};")
                .WriteLine();
        }

        public static void GenerateMachineDescriptor(CodeBuilder codeBuilder, Machine machine) {

            codeBuilder
                .WriteLine("// -----------------------------------------------------------------------")
                .WriteLine("// Machine descriptor.")
                .WriteLine("//")
                .WriteLine("const MachineDescriptor machine = {")
                .Indent();

            codeBuilder
                .WriteLine("State_{0},", machine.Start.Name)
                .WriteLine("MAX_STATES,")
                .WriteLine("MAX_EVENTS,")
                .WriteLine("states,")
                .WriteLine("transitions");

            codeBuilder
                .UnIndent()
                .WriteLine("};")
                .WriteLine();
        }

        /// <summary>
        /// Genera les funcions d'accio.
        /// </summary>
        /// <param name="codeBuilder">Constructor de codi font.</param>
        /// <param name="machine">La maquina.</param>
        /// 
        public static void GenerateActionImplementation(CodeBuilder codeBuilder, Machine machine) {

            int actionNum = 0;
            foreach (State state in machine.States) {
                if (state.EnterAction != null) {
                    codeBuilder
                        .WriteLine("// -----------------------------------------------------------------------")
                        .WriteLine("// Enter action")
                        .WriteLine("//     State: {0}", state.Name)
                        .WriteLine("//")
                        .WriteLine("static void Action{0}(Context *context) {{", actionNum++)
                        .WriteLine();

                    WriteAction(codeBuilder, state.EnterAction);

                    codeBuilder
                        .WriteLine("}")
                        .WriteLine();
                }
                if (state.ExitAction != null) {
                    codeBuilder
                        .WriteLine("// -----------------------------------------------------------------------")
                        .WriteLine("// Exit action")
                        .WriteLine("//     State: {0}", state.Name)
                        .WriteLine("//")
                        .WriteLine("static void Action{0}(Context *context) {{", actionNum++)
                        .WriteLine();

                    WriteAction(codeBuilder, state.ExitAction);

                    codeBuilder
                        .WriteLine("}")
                        .WriteLine();
                }
                foreach (Transition transition in state.Transitions) {
                    if (transition.Action != null) {
                        codeBuilder
                            .WriteLine("// -----------------------------------------------------------------------")
                            .WriteLine("// Transition action")
                            .WriteLine("//     State: {0}", state.Name)
                            .WriteLine("//     Event: {0}", transition.Event.Name)
                            .WriteLine("//")
                            .WriteLine("static void Action{0}(Context *context) {{", actionNum++)
                            .WriteLine();

                        WriteAction(codeBuilder, transition.Action);

                        codeBuilder
                            .WriteLine("}")
                            .WriteLine();
                    }
                }
            }
            if (actionNum > 0)
                codeBuilder.WriteLine();
        }

        /// <summary>
        /// Genera les fiuncions de guarda.
        /// </summary>
        /// <param name="codeBuilder">Constructor de codi font.</param>
        /// <param name="machine">La maquina.</param>
        /// 
        public static void GenerateGuardImplementation(CodeBuilder codeBuilder, Machine machine) {

            int guardNum = 0;
            foreach (State state in machine.States) {
                foreach (Transition transition in state.Transitions) {
                    if (transition.Guard != null) {
                        codeBuilder
                            .WriteLine("// -----------------------------------------------------------------------")
                            .WriteLine("// Transition guard")
                            .WriteLine("//     State: {0}", state.Name)
                            .WriteLine("//     Event: {0}", transition.Event.Name)
                            .WriteLine("//")
                            .WriteLine("static bool Guard{0}(Context *context) {{", guardNum++)
                            .WriteLine()
                            .Indent()
                            .WriteLine("return {0};", transition.Guard.Condition)
                            .UnIndent()
                            .WriteLine("}")
                            .WriteLine(); 
                    }
                }
            }
            if (guardNum > 0)
                codeBuilder.WriteLine();
        }

        /// <summary>
        /// Genera la implementacio d'una accio.
        /// </summary>
        /// <param name="codeBuilder">Constructor de codi.</param>
        /// <param name="action">L'accio.</param>
        /// 
        private static void WriteAction(CodeBuilder codeBuilder, Action action) {

            foreach (Command command in action.Commands) {
                InlineCommand inlineCmd = command as InlineCommand;
                if (inlineCmd != null) {
                    codeBuilder.WriteLine(inlineCmd.Text.TrimStart());
                }
            }
        }
    }
}
