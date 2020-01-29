namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    internal sealed class StateCodeVisitor : DefaultVisitor {

        private readonly TextWriter writer;
        private readonly CPPGeneratorOptions options;
        private readonly CppCodeBuilder codeBuilder = new CppCodeBuilder();
        public State currentState;

        public StateCodeVisitor(TextWriter writer, CPPGeneratorOptions options) {

            this.writer = writer;
            this.options = options;
        }

        public override void Visit(Machine machine) {

            WriteIncludes();
            WriteUsing();
            WriteConstructor(options.StateClassName);

            foreach (string transitionName in machine.GetTransitionNames()) {
                codeBuilder
                    .WriteLine("/// ----------------------------------------------------------------------")
                    .WriteLine("/// \\brief    Perform '{0}' transition.", transitionName)
                    .WriteLine("/// \\param    context: The context.")
                    .WriteLine("///")
                    .WriteLine("void {0}::on{1}({2}* context) {{", options.StateClassName, transitionName, options.ContextClassName)
                    .WriteLine()
                    .WriteLine("}")
                    .WriteLine()
                    .WriteLine();
            }

            foreach (State state in machine.States)
                state.AcceptVisitor(this);

            writer.Write(codeBuilder.ToString());
        }

        public override void Visit(State state) {

            currentState = state;

            // Genera el constructor.
            //            
            codeBuilder
                .WriteLine("/// ----------------------------------------------------------------------")
                .WriteLine("/// \\brief    Constructor.")
                .WriteLine("///")
                .WriteLine("{0}::{0}() {{", state.FullName)
                .WriteLine("}")
                .WriteLine()
                .WriteLine();
    
            codeBuilder
                .WriteLine("/// ----------------------------------------------------------------------")
                .WriteLine("/// \\brief    Get a instance of class.")
                .WriteLine("///")
                .WriteLine("{0}* {0}::getInstance() {{", state.FullName)
                .WriteLine()
                .Indent()
                .WriteLine("if (instance == nullptr)")
                .Indent()
                .WriteLine("instance = new {0}();", state.FullName)
                .UnIndent()
                .WriteLine("")
                .WriteLine("return instance;")
                .UnIndent()
                .WriteLine("}")
                .WriteLine()
                .WriteLine("{0}* {0}::instance = nullptr;", state.FullName)
                .WriteLine()
                .WriteLine();

            // Genera les transicions. 
            //
            foreach (string transitionName in state.GetTransitionNames()) {

                WriteTransitionMethodHeader(options.ContextClassName, state.FullName, transitionName);
                foreach (Transition transition in state.Transitions) {
                    if (transition.Name == transitionName)
                        transition.AcceptVisitor(this);
                }
                WriteMethodTail();
            }

            currentState = null;
        }

        public override void Visit(Transition transition) {

            // Genera el codi de les guardes.
            //
            codeBuilder
                .WriteLine("// Check transition guard.")
                .WriteLine("//");
            if (transition.Guard != null)
                codeBuilder
                    .WriteLine("if ({0}) {{", transition.Guard.Expression);
            else
                codeBuilder
                    .WriteLine("if (true) {");
            codeBuilder
                .Indent()
                .WriteLine();

            // Genera el codi de l'accio de sortida del estat actual.
            //
            if (transition.NextState != null) {
                if (currentState.ExitAction != null) {
                    codeBuilder
                        .WriteLine("// Exit state action.")
                        .WriteLine("//");
                    currentState.ExitAction.AcceptVisitor(this);
                    codeBuilder
                        .WriteLine();
                }
            }

            // Genera el code de l'accio de la transicio.
            //
            if (transition.Action != null) {
                codeBuilder
                    .WriteLine("// Transition action.")
                    .WriteLine("//");
                transition.Action.AcceptVisitor(this);
                codeBuilder
                    .WriteLine();
            }

            // Genera el codi de l'accio d'entrada del nou estat.
            //
            if (transition.NextState != null) {
                if (transition.NextState.EnterAction != null) {
                    codeBuilder
                        .WriteLine("// Enter state action.")
                        .WriteLine("//");
                    transition.NextState.EnterAction.AcceptVisitor(this);
                    codeBuilder
                        .WriteLine();
                }
            }

            switch (transition.Mode) {
                case TransitionMode.ExternalLoop:
                case TransitionMode.InternalLoop:
                    codeBuilder
                        .WriteLine()
                        .WriteLine("return;");
                    break;

                case TransitionMode.Jump:
                    codeBuilder
                        .WriteLine("// Set the next state.")
                        .WriteLine("//")
                        .WriteLine("context->setState({0}::getInstance());", transition.NextState.FullName)
                        .WriteLine()
                        .WriteLine("return;");
                    break;

                    /*case TransitionMode.CallToState:
                        codeBuilder
                            .WriteLine()
                            .WriteLine("pushState(machine, {0}::getInstance());", transition.NextState.FullName);
                        break;

                    case TransitionMode.ReturnFromState:
                        codeBuilder
                            .WriteLine()
                            .WriteLine("popState();");
                        break;*/
            }

            // Final del bloc de la guarda.
            //
            codeBuilder
                .UnIndent()
                .WriteLine("}")
                .WriteLine();
        }

        public override void Visit(CodeActity action) {

            if (!System.String.IsNullOrEmpty(action.Text)) {
                string[] lines = action.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                    if (!System.String.IsNullOrEmpty(line))
                        codeBuilder.WriteLine(line.Trim());
            }
        }

        public override void Visit(CallActivity action) {

            if (!System.String.IsNullOrEmpty(action.MethodName)) {
                codeBuilder.WriteLine("context->do{0}();", action.MethodName);
            }
        }

        private void WriteIncludes() {

            string contextHeaderFileName = Path.GetFileName(options.ContextHeaderFileName);
            string stateHeaderFileName = Path.GetFileName(options.StateHeaderFileName);

            codeBuilder
                .WriteInclude(contextHeaderFileName)
                .WriteInclude(stateHeaderFileName)
                .WriteLine()
                .WriteLine();
        }

        private void WriteUsing() {

            if (!String.IsNullOrEmpty(options.NsName))
                codeBuilder
                    .WriteUsingNamespace(options.NsName)
                    .WriteLine()
                    .WriteLine();
        }

        private void WriteConstructor(string className) {

            codeBuilder
                .WriteLine("/// ----------------------------------------------------------------------")
                .WriteLine("/// \\brief    Constructor.")
                .WriteLine("///")
                .WriteLine("{0}::{0}() {{", className)
                .WriteLine("}")
                .WriteLine()
                .WriteLine();
        }

        private void WriteTransitionMethodHeader(string machineClassName, string stateClassName, string transitionName) {

            codeBuilder
                .WriteLine("/// ----------------------------------------------------------------------")
                .WriteLine("/// \\brief    Perform '{0}' transition to other state.", transitionName)
                .WriteLine("/// \\param    context: The context.")
                .WriteLine("///")
                .WriteLine("void {0}::on{1}({2}* context) {{", stateClassName, transitionName, machineClassName)
                .Indent()
                .WriteLine();
        }

        private void WriteMethodTail() {

            codeBuilder
                .UnIndent()
                .WriteLine("}")
                .WriteLine()
                .WriteLine();
        }
    }
}



