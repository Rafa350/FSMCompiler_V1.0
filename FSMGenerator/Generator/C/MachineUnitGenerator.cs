namespace MikroPicDesigns.FSMCompiler.v1.Generator.C {

    using System;
    using System.Collections.Generic;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    public static class MachineUnitGenerator {

        private static CGeneratorOptions options;

        public static UnitDeclaration Generate(Machine machine, CGeneratorOptions options) {

            MachineUnitGenerator.options = options;

            List<IUnitMember> memberList = new List<IUnitMember>();

            // Crea la funcio [machine]_Start()
            //
            memberList.Add(MakeStartFunction(machine));

            // Crea les funcions de transicio <machine>_on<transition>()
            //
            foreach (var transitionName in machine.GetTransitionNames())
                memberList.Add(MakeTransitionFunction(machine, transitionName));

            return new UnitDeclaration(memberList);
        }

        /// <summary>
        /// Crea la funcio [machine]_Start()
        /// </summary>
        /// <param name="machine">La maquina</param>
        /// <returns>La declaracio de la funcio.</returns>
        /// 
        private static FunctionDeclaration MakeStartFunction(Machine machine) {

            FunctionDeclaration function = new FunctionDeclaration();
            function.Name = String.Format("{0}Machine_Start", machine.Name);
            function.ReturnType = TypeIdentifier.FromName("void");
            function.Body = MakeStartFunctionBody(machine);

            return function;
        }

        /// <summary>
        /// Crea el cos de la funcio [machine]_Start()
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>El cos de la funcio.</returns>
        /// 
        private static Block MakeStartFunctionBody(Machine machine) {

            Block body = new Block();

            if (machine.Start.EnterAction != null) {
                foreach (var activity in machine.Start.EnterAction.Activities) {
                    if (activity is CallActivity callActivity)
                        body.AddStatement(
                            new FunctionCallStatement(
                                new FunctionCallExpression(
                                    new IdentifierExpression(callActivity.MethodName),
                                    null)));
                }
            }

            body.AddStatement(
                new AssignStatement("state",
                new LiteralExpression(
                    String.Format("State_{0}", machine.Start.Name))));

            return body;
        }

        /// <summary>
        /// Crtea la funcio [machine]_on[transcition]()
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <param name="transitionName">El nom de la transicio.</param>
        /// <returns>La declaracio de la funcio.</returns>
        /// 
        private static FunctionDeclaration MakeTransitionFunction(Machine machine, string transitionName) {

            FunctionDeclaration function = new FunctionDeclaration();
            function.Name = String.Format("{0}Machine_on{1}", machine.Name, transitionName);
            function.ReturnType = TypeIdentifier.FromName("void");
            function.Body = MakeTransitionFunctionBody(machine, transitionName);

            return function;
        }

        private static Block MakeTransitionFunctionBody(Machine machine, string transitionName) {

            SwitchStatement switchStmt = new SwitchStatement();
            switchStmt.Expression = new IdentifierExpression("state");

            foreach (var state in machine.States) {
                SwitchCaseStatement caseStmt = new SwitchCaseStatement();
                caseStmt.Expression = new LiteralExpression(
                    String.Format("State_{0}", state.Name));

                Block body = new Block();
                foreach (var transition in state.Transitions) {
                    if (transition.Name == transitionName) {

                        Block trueBlock = new Block();

                        if ((transition.Action != null) && (transition.Action.Activities != null)) {

                            // Accions de sortida del estat actual
                            //

                            // Accions de la transicio
                            //
                            foreach (var activity in transition.Action.Activities)
                                if (activity is CallActivity callActivity)
                                    trueBlock.AddStatement(
                                        new FunctionCallStatement(
                                            new FunctionCallExpression(
                                                new IdentifierExpression(callActivity.MethodName),
                                                null)));

                            // Acciona d'entrada del nou estat
                            //

                            // Canvi d'estat
                            //
                            if (transition.NextState != null)
                                trueBlock.AddStatement(
                                    new AssignStatement("state",
                                    new LiteralExpression(
                                        String.Format("State_{0}", transition.NextState.Name))));
                        }

                        if (transition.Guard == null) {
                            body.AddStatement(new IfThenElseStatement(
                                new InlineExpression("true"),
                                trueBlock,
                                null));
                        }
                        else {
                            body.AddStatement(new IfThenElseStatement(
                                new InlineExpression(transition.Guard.Expression),
                                trueBlock,
                                null));
                        }
                    }
                }
                caseStmt.Body = body;
                switchStmt.AddSwitchCase(caseStmt);
            }

            return new Block(switchStmt);
        }
    }
}
