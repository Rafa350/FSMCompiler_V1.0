namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Commands;
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal sealed class CodeGenerator {

        private readonly Machine machine;
        private readonly Dictionary<Model.Action, string> actionDict = new Dictionary<Model.Action, string>();
        private readonly Dictionary<Guard, string> guardDict = new Dictionary<Guard, string>();

        /// <summary>
        /// Constructor del objecte.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// 
        public CodeGenerator(Machine machine) {

            this.machine = machine;

            int actionCount = 0;
            int guardCount = 0;
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
        }

        /// <summary>
        /// Genera la taula d'estats. Per implementacions basades en taules.
        /// </summary>
        /// <param name="codeBuilder">Constructor de codi font.</param>
        /// 
        public void GenerateStateDescriptorTable(CodeBuilder codeBuilder) {

            codeBuilder
                .WriteLine("// -----------------------------------------------------------------------")
                .WriteLine("// State descriptor table.")
                .WriteLine("//")
                .WriteLine("static const StateDescriptor states[] = {")
                .Indent();

            int transitionOffset = 0;
            foreach (State state in machine.States) {

                StringBuilder sb = new StringBuilder();
                sb.Append("{ ");
                sb.AppendFormat("State_{0}, ", state.Name);
                if (state.EnterAction == null)
                    sb.Append("NULL, ");
                else
                    sb.AppendFormat("{0}, ", actionDict[state.EnterAction]);
                if (state.ExitAction == null)
                    sb.Append("NULL, ");
                else
                    sb.AppendFormat("{0}, ", actionDict[state.ExitAction]);

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

                foreach (Transition transition in state.Transitions) 
                    transitionOffset++;
            }
            codeBuilder
                .UnIndent()
                .WriteLine("};")
                .WriteLine();
        }

        /// <summary>
        /// Genera la taula de transicions. Per implementacions basades en taules.
        /// </summary>
        /// <param name="cb">Constructor de codi font.</param>
        /// 
        public void GenerateTransitionDescriptorTable(CodeBuilder cb) {

            cb
                .WriteLine("// -----------------------------------------------------------------------")
                .WriteLine("// Transition descriptor table.")
                .WriteLine("//")
                .WriteLine("static const TransitionDescriptor transitions[] = {")
                .Indent();

            foreach (State state in machine.States) {
                foreach (Transition transition in state.Transitions) {

                    StringBuilder sb = new StringBuilder();
                    sb.Append("{ ");
                    sb.AppendFormat("Event_{0}, ", transition.Event.Name);
                    if (transition.NextState == null)
                        sb.AppendFormat("State_{0}, ", state.Name);
                    else
                        sb.AppendFormat("State_{0}, ", transition.NextState.Name);
                    if (transition.Guard == null)
                        sb.Append("NULL, ");
                    else
                        sb.AppendFormat("{0}, ", guardDict[transition.Guard]);
                    if (transition.Action == null)
                        sb.Append("NULL");
                    else
                        sb.AppendFormat("{0}", actionDict[transition.Action]);
                    sb.Append(" },");

                    cb.WriteLine(sb.ToString());
                }
            }
            cb
                .UnIndent()
                .WriteLine("};")
                .WriteLine();
        }


        /// <summary>
        /// Genera la taula de maquines. Per implementacio basada en taules.
        /// </summary>
        /// <param name="cb">Constructor de codi font.</param>
        /// 
        public void GenerateMachineDescriptorTable(CodeBuilder cb) {

            cb
                .WriteLine("// -----------------------------------------------------------------------")
                .WriteLine("// Machine descriptor table.")
                .WriteLine("//")
                .WriteLine("const MachineDescriptor machine = {")
                .Indent();

            cb
                .WriteLine("State_{0},", machine.Start.Name)
                .WriteLine("NUM_STATES,")
                .WriteLine("NUM_EVENTS,")
                .WriteLine("states,")
                .WriteLine("transitions");

            cb
                .UnIndent()
                .WriteLine("};")
                .WriteLine();
        }

        /// <summary>
        /// Genera les funcions d'accio.
        /// </summary>
        /// <param name="cb">Constructor de codi font.</param>
        /// 
        public void GenerateActionImplementation(CodeBuilder cb) {

            foreach (State state in machine.States) {

                if (state.EnterAction != null) {
                    cb
                        .WriteLine("// -----------------------------------------------------------------------")
                        .WriteLine("// Enter action")
                        .WriteLine("//     State: {0}", state.Name)
                        .WriteLine("//");
                    EmitActionDeclarationStart(cb, state.EnterAction);
                    EmitActionBody(cb, state.EnterAction);
                    EmitActionDeclarationEnd(cb, state.EnterAction);
                }

                if (state.ExitAction != null) {
                    cb
                        .WriteLine("// -----------------------------------------------------------------------")
                        .WriteLine("// Exit action")
                        .WriteLine("//     State: {0}", state.Name)
                        .WriteLine("//");
                    EmitActionDeclarationStart(cb, state.ExitAction);
                    EmitActionBody(cb, state.ExitAction);
                    EmitActionDeclarationEnd(cb, state.ExitAction);
                }

                foreach (Transition transition in state.Transitions) {
                    if (transition.Action != null) {
                        cb
                            .WriteLine("// -----------------------------------------------------------------------")
                            .WriteLine("// Transition action")
                            .WriteLine("//     State: {0}", state.Name)
                            .WriteLine("//     Event: {0}", transition.Event.Name)
                            .WriteLine("//");
                        EmitActionDeclarationStart(cb, transition.Action);
                        EmitActionBody(cb, transition.Action);
                        EmitActionDeclarationEnd(cb, transition.Action);
                    }
                }
            }
        }

        /// <summary>
        /// Genera les funcions de guarda.
        /// </summary>
        /// <param name="cb">Constructor de codi font.</param>
        /// 
        public void GenerateGuardImplementation(CodeBuilder cb) {

            foreach (State state in machine.States) {
                foreach (Transition transition in state.Transitions) {
                    if (transition.Guard != null) {
                        cb
                            .WriteLine("// -----------------------------------------------------------------------")
                            .WriteLine("// Transition guard")
                            .WriteLine("//     State: {0}", state.Name)
                            .WriteLine("//     Event: {0}", transition.Event == null ? "default" : transition.Event.Name)
                            .WriteLine("//");
                        EmitGuardDeclarationStart(cb, transition.Guard);
                        EmitGuardBody(cb, transition.Guard);
                        EmitGuardDeclarationEnd(cb, transition.Guard);
                    }
                }
            }
        }

        /// <summary>
        /// Genera la funcio de procesament de la maquina d'estats. Versio de maquina amb "switch"
        /// </summary>
        /// <param name="cb">Constructor de codi font.</param>
        /// 
        public void GenerateProcessorImplementation(CodeBuilder cb) {

            cb
                .WriteLine("// -----------------------------------------------------------------------")
                .WriteLine("// Internal vars.")
                .WriteLine("//")
                .WriteLine("static State state;")
                .WriteLine();

            cb
                .WriteLine("// -----------------------------------------------------------------------")
                .WriteLine("// State machine setup")
                .WriteLine("//")
                .WriteLine("void {0}Setup(Context context) {{", machine.Name)
                .WriteLine()
                .Indent()
                .WriteLine("state = State_{0};", machine.Start.Name);

            if (machine.Start.EnterAction != null)
                EmitActionCall(cb, machine.Start.EnterAction);

            cb
                .UnIndent()
                .WriteLine("}")
                .WriteLine();

            cb
                .WriteLine("// -----------------------------------------------------------------------")
                .WriteLine("// State machine event processor")
                .WriteLine("//")
                .WriteLine("void {0}Run(Event event, Context context) {{", machine.Name)
                .WriteLine()
                .Indent()
                .WriteLine("switch (state) {")
                .Indent();

            foreach (State state in machine.States) {

                cb
                    .WriteLine("case State_{0}: ", state.Name)
                    .Indent();

                bool first = true;
                foreach (Transition transition in state.Transitions) {

                    StringBuilder sb = new StringBuilder();

                    if (first) {
                        first = false;
                        sb.Append("if (");
                    }
                    else
                        sb.Append("else if (");
                    if (transition.Guard != null)
                        sb.Append('(');
                    sb.AppendFormat("event == Event_{0}", transition.Event.Name);
                    if (transition.Guard != null)
                        sb.AppendFormat(") && {0}(context)", guardDict[transition.Guard]);
                    sb.Append(") {");

                    cb
                        .WriteLine(sb.ToString())
                        .Indent();

                    // Si hi ha canvi d'estat, genera la crida a la ExitAction del estat actual
                    //
                    if ((transition.NextState != null) && (transition.NextState != state)) {
                        if (state.ExitAction != null)
                            EmitActionCall(cb, state.ExitAction);
                    }

                    // Genera la crida a la accio de la transicio
                    //
                    if (transition.Action != null)
                        EmitActionCall(cb, transition.Action);

                    // Si hi ha canvi d'estat, genera la crida a la EnterAction del nou estat.
                    //
                    if ((transition.NextState != null) && (transition.NextState != state)) {
                        if (transition.NextState.EnterAction != null)
                            EmitActionCall(cb, transition.NextState.EnterAction);
                        cb
                            .WriteLine("state = State_{0};", transition.NextState.Name);
                    }

                    cb
                        .UnIndent()
                        .WriteLine("}");
                }

                cb
                    .WriteLine("break; ")
                    .UnIndent()
                    .WriteLine();
            }

            cb
                .UnIndent()
                .WriteLine("}")
                .UnIndent()
                .WriteLine("}")
                .WriteLine();
        }

        /// <summary>
        /// Genera el inici de la declaracio de l'accio.
        /// </summary>
        /// <param name="cb">Constructor de codi.</param>
        /// <param name="action">L'accio.</param>
        /// 
        private void EmitActionDeclarationStart(CodeBuilder cb, Model.Action action) {

            cb
                .WriteLine("static void {0}(Context context) {{", actionDict[action])
                .WriteLine()
                .Indent();
        }

        /// <summary>
        /// Finalitza la declaracio de l'accio.
        /// </summary>
        /// <param name="cb">Constructor de codi.</param>
        /// <param name="action">L'accio.</param>
        /// 
        private void EmitActionDeclarationEnd(CodeBuilder cb, Model.Action action) {

            cb
                .UnIndent()
                .WriteLine("}")
                .WriteLine();
        }

        /// <summary>
        /// Genera la implementacio d'una accio.
        /// </summary>
        /// <param name="cb">Constructor de codi.</param>
        /// <param name="action">L'accio.</param>
        /// 
        private void EmitActionBody(CodeBuilder cb, Model.Action action) {

            foreach (Command command in action.Commands) {
                InlineCommand inlineCmd = command as InlineCommand;
                if (inlineCmd != null)
                    cb.WriteLine(inlineCmd.Text);
            }
        }

        /// <summary>
        /// Genera la crida a l'accio.
        /// </summary>
        /// <param name="cb">Constructor de codi.</param>
        /// <param name="action">L'accio.</param>
        /// 
        private void EmitActionCall(CodeBuilder cb, Model.Action action) {

            cb
                .WriteLine("{0}(context);", actionDict[action]);
        }

        /// <summary>
        /// Genera la capcelera de la declaracio de la funcio de guarda.
        /// </summary>
        /// <param name="codeBuilder">Constructor de codi.</param>
        /// <param name="guard">La guarda.</param>
        /// 
        private void EmitGuardDeclarationStart(CodeBuilder codeBuilder, Guard guard) {

            codeBuilder
                .WriteLine("static bool {0}(Context context) {{", guardDict[guard])
                .WriteLine()
                .Indent();
        }

        /// <summary>
        /// Genera la implementacio d'una funcio de guarda
        /// </summary>
        /// <param name="codeBuilder">Constructor de codi.</param>
        /// <param name="action">La guarda.</param>
        /// 
        private void EmitGuardDeclarationEnd(CodeBuilder codeBuilder, Guard guard) {

            codeBuilder
                .UnIndent()
                .WriteLine("}")
                .WriteLine();
        }

        /// <summary>
        /// Finalitza la declaracio de la funcio de guarda.
        /// </summary>
        /// <param name="codeBuilder">Constructor de codi.</param>
        /// <param name="guard">La guarda.</param>
        /// 
        private void EmitGuardBody(CodeBuilder codeBuilder, Guard guard) {

            codeBuilder
                .WriteLine("return {0};", guard.Expression);
        }
    }
}