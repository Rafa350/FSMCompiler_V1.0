namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    using MikroPicDesigns.FSMCompiler.v1.Model;
    using System.Text;

    internal static class StateCodeGenerator {

        /// <summary>
        /// Genera la taula d'estats.
        /// </summary>
        /// <param name="codeBuilder">Generador de codi font.</param>
        /// <param name="machine">La maquina a generar.</param>
        /// 
        public static void GenerateStateTable(CodeBuilder codeBuilder, Machine machine) {

            codeBuilder
                .WriteLine("static const StateDescriptor states[] = {")
                .Indent();

            int actionNum = 0;
            int transitionOffset = 0;
            foreach (State state in machine.States) {

                StringBuilder sb = new StringBuilder();
                sb.Append("{ ");
                sb.Append(state.Name);
                sb.Append(", ");
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
                .WriteLine("}")
                .WriteLine();
        }

        /// <summary>
        /// Genera la taula de transicions.
        /// </summary>
        /// <param name="codeBuilder">Generador de codi font.</param>
        /// <param name="machine">La maquina.</param>
        /// 
        public static void GenerateTransitionTable(CodeBuilder codeBuilder, Machine machine) {

            codeBuilder
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
                    sb.Append("{ Event_");
                    sb.Append(transition.Event.Name);
                    sb.Append(", ");
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
                .WriteLine("}")
                .WriteLine();
        }

        public static void GenerateActionImplementation(CodeBuilder codeBuilder, Machine machine) {

            int actionNum = 0;
            foreach (State state in machine.States) {
                if (state.EnterAction != null) {
                    codeBuilder
                        .WriteLine("// --------------------------------------")
                        .WriteLine("// Enter action")
                        .WriteLine("//     State: {0}", state.Name)
                        .WriteLine("//")
                        .WriteLine("static void Action{0}() {{", actionNum++)
                        .WriteLine("}")
                        .WriteLine();
                }
                if (state.ExitAction != null) {
                    codeBuilder
                        .WriteLine("// --------------------------------------")
                        .WriteLine("// Exit action")
                        .WriteLine("//     State: {0}", state.Name)
                        .WriteLine("//")
                        .WriteLine("static void Action{0}() {{", actionNum++)
                        .WriteLine("}")
                        .WriteLine();
                }
                foreach (Transition transition in state.Transitions) {
                    if (transition.Action != null) {
                        codeBuilder
                            .WriteLine("// --------------------------------------")
                            .WriteLine("// Transition action")
                            .WriteLine("//     State: {0}", state.Name)
                            .WriteLine("//     Event: {0}", transition.Event.Name)
                            .WriteLine("//")
                            .WriteLine("static void Action{0}() {{", actionNum++)
                        .WriteLine("}")
                        .WriteLine();
                    }
                }
            }
            if (actionNum > 0)
                codeBuilder.WriteLine();
        }

        public static void GenerateGuardImplementation(CodeBuilder codeBuilder, Machine machine) {

            int guardNum = 0;
            foreach (State state in machine.States) {
                foreach (Transition transition in state.Transitions) {
                    if (transition.Guard != null) {
                        codeBuilder
                            .WriteLine("// --------------------------------------")
                            .WriteLine("// Transition guard")
                            .WriteLine("//     State: {0}", state.Name)
                            .WriteLine("//     Event: {0}", transition.Event.Name)
                            .WriteLine("//")
                            .WriteLine("static bool Guard{0}() {{", guardNum++)
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
    }
}
