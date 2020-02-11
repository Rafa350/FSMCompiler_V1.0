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

            // Crea el tipus enumerador pels valosr del estat.
            //
            List<string> stateList = new List<string>();
            foreach (var state in machine.States) {
                stateList.Add(String.Format("State_{0}", state.Name));
            }
            EnumeratorDeclaration stateType = new EnumeratorDeclaration("State", stateList);
            memberList.Add(stateType);

            // Crea la variabe 'state'
            //
            VariableDeclaration stateVariable = new VariableDeclaration();
            stateVariable.Name = String.Format("{0}_state", machine.Name);
            stateVariable.ValueType = TypeIdentifier.FromName("State");
            memberList.Add(stateVariable);

            // Crea la funcio [machine]_Start()
            //
            memberList.Add(MakeStartFunction(machine));

            // Crea les funcions de deptch de les transicions [machine]_on[transition]()
            //
            foreach (var transitionName in machine.GetTransitionNames()) {
                memberList.Add(MakeMachineTransitionFunction(machine, transitionName));
            }

            // Crea les funcions de transicio [machine]_[state]_on[transition]()
            //
            foreach (var state in machine.States) {
                foreach (var transitionName in state.GetTransitionNames()) {
                    memberList.Add(MakeStateTransitionFunction(machine, state, transitionName));
                }
            }

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
            function.Name = String.Format("{0}_Start", machine.Name);
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

            // Accions d'inici del estat inicial.
            //
            if (machine.Start.EnterAction != null) {
                foreach (var activity in machine.Start.EnterAction.Activities) {
                    if (activity is CallActivity callActivity) {
                        body.AddStatement(
                            new FunctionCallStatement(
                                new FunctionCallExpression(
                                    new IdentifierExpression(callActivity.MethodName),
                                    null)));
                    }
                }
            }

            // Seleccio del estat inicial.
            //
            body.AddStatement(
                new AssignStatement(
                    String.Format("{0}_state", machine.Name),
                new LiteralExpression(
                    String.Format("State_{0}", machine.Start.Name))));

            return body;
        }

        private static FunctionDeclaration MakeMachineTransitionFunction(Machine machine, string transitionName) {

            FunctionDeclaration function = new FunctionDeclaration();
            function.Name = String.Format("{0}_on{1}", machine.Name, transitionName);
            function.ReturnType = TypeIdentifier.FromName("void");
            function.Body = MakeMachineTransitionFunctionBody(machine, transitionName);

            return function;
        }

        private static Block MakeMachineTransitionFunctionBody(Machine machine, string transitionName) {

            Block body = new Block();

            SwitchStatement switchStmt = new SwitchStatement();
            switchStmt.Expression = new IdentifierExpression(
                String.Format("{0}_state", machine.Name));

            foreach (var state in machine.States) {
                foreach (var transition in state.Transitions) {
                    if (transition.Name == transitionName) {

                        SwitchCaseStatement caseStmt = new SwitchCaseStatement();
                        caseStmt.Expression = new LiteralExpression(
                            String.Format("State_{0}", state.Name));

                        Block caseStmtBody = new Block();
                        caseStmtBody.AddStatement(
                            new FunctionCallStatement(
                                new FunctionCallExpression(
                                    new IdentifierExpression(
                                        String.Format("{0}_{1}_on{2}", machine.Name, state.Name, transitionName)),
                                    null)));

                        caseStmt.Body = caseStmtBody;

                        switchStmt.AddSwitchCase(caseStmt);
                    }
                }
            }

            switchStmt.AddSwitchCase(new SwitchCaseStatement(null, null));

            body.AddStatement(switchStmt);

            return body;
        }

        /// <summary>
        /// Crea la funcio [machine]_[state]_on[transition]()
        /// </summary>
        /// <param name="state">La maquina.</param>
        /// <param name="machineName">El nom de la maquina.</param>
        /// <param name="transitionName">El nom de la transicio.</param>
        /// <returns>La declaracio de la funcio.</returns>
        /// 
        private static FunctionDeclaration MakeStateTransitionFunction(Machine machine, State state, string transitionName) {

            FunctionDeclaration function = new FunctionDeclaration();
            function.Name = String.Format("{0}_{1}_on{2}", machine.Name, state.Name, transitionName);
            function.ReturnType = TypeIdentifier.FromName("void");
            function.Body = MakeStateTransitionFunctionBody(machine, state, transitionName);

            return function;
        }

        private static Block MakeStateTransitionFunctionBody(Machine machine, State state, string transitionName) {

            Block body = new Block();
            foreach (var transition in state.Transitions) {
                if (transition.Name == transitionName) {

                    Block trueBlock = new Block();

                    // Accions de sortida del estat actual
                    //
                    if ((state.ExitAction != null) && (state.ExitAction.Activities != null)) {
                        foreach (var activity in state.ExitAction.Activities) {
                            if (activity is CallActivity callActivity) {
                                trueBlock.AddStatement(
                                    new FunctionCallStatement(
                                        new FunctionCallExpression(
                                            new IdentifierExpression(callActivity.MethodName),
                                            null)));
                            }
                        }
                    }

                    // Accions de la transicio
                    //
                    if ((transition.Action != null) && (transition.Action.Activities != null)) {
                        foreach (var activity in transition.Action.Activities) {
                            if (activity is CallActivity callActivity) {
                                trueBlock.AddStatement(
                                    new FunctionCallStatement(
                                        new FunctionCallExpression(
                                            new IdentifierExpression(callActivity.MethodName),
                                            null)));
                            }
                        }
                    }

                    // Acciona d'entrada del nou estat
                    //
                    if ((transition.NextState != null) && (transition.NextState.EnterAction != null) && (transition.NextState.EnterAction.Activities != null)) {
                        foreach (var activity in transition.NextState.EnterAction.Activities) {
                            if (activity is CallActivity callActivity) {
                                trueBlock.AddStatement(
                                    new FunctionCallStatement(
                                        new FunctionCallExpression(
                                            new IdentifierExpression(callActivity.MethodName),
                                            null)));
                            }
                        }
                    }

                    // Seeccio el nou estat
                    //
                    if (transition.NextState != null) {
                        trueBlock.AddStatement(
                            new AssignStatement(
                                String.Format("{0}_state", machine.Name),
                            new LiteralExpression(
                                String.Format("State_{0}", transition.NextState.Name))));
                    }

                    if (transition.Guard == null) {
                        body.AddStatement(new IfThenElseStatement(
                            new LiteralExpression(1),
                            trueBlock,
                            null));
                    }
                    else {
                        body.AddStatement(new IfThenElseStatement(
                            new FunctionCallExpression(
                                new IdentifierExpression(transition.Guard.Expression),
                                null),
                            trueBlock,
                            null));
                    }
                }
            }

            return body;
        }
    }
}
