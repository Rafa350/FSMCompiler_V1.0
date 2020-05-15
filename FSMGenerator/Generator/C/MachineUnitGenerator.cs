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

            NamespaceMemberList memberList = new NamespaceMemberList();

            // Crea el tipus enumerador pels valors del estat.
            //
            List<string> stateList = new List<string>();
            foreach (var state in machine.States)
                stateList.Add(String.Format("State_{0}", state.Name));

            EnumerationDeclaration stateType = new EnumerationDeclaration("State", AccessSpecifier.Public, stateList);
            memberList.Add(stateType);

            // Crea la variabe 'state'
            //
            VariableDeclaration stateVariable = new VariableDeclaration();
            stateVariable.Access = AccessSpecifier.Private;
            stateVariable.Name = String.Format("{0}_state", machine.Name);
            stateVariable.ValueType = TypeIdentifier.FromName("State");
            memberList.Add(stateVariable);

            // Crea les funcions de transicio [machine]_[state]_[transition]()
            //
            foreach (var state in machine.States)
                foreach (var transitionName in state.GetTransitionNames())
                    memberList.Add(MakeStateTransitionFunction(machine, state, transitionName));

            // Crea la funcio [machine]_Start()
            //
            memberList.Add(MakeStartFunction(machine));

            // Crea les funcions de despatch de les transicions [machine]_[transition]()
            //
            foreach (var transitionName in machine.GetTransitionNames())
                memberList.Add(MakeMachineTransitionFunction(machine, transitionName));

            return new UnitDeclaration(new NamespaceDeclaration {
                Name = "",
                Members = memberList
            }); ;
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
        private static BlockStatement MakeStartFunctionBody(Machine machine) {

            StatementList bodyStmtList = new StatementList();

            // Accions d'inici del estat inicial.
            //
            if ((machine.Start.EnterAction != null) && machine.Start.EnterAction.HasActivities)
                foreach (var activity in machine.Start.EnterAction.Activities)
                    bodyStmtList.Add(MakeActivity(activity));

            // Seleccio del estat inicial.
            //
            bodyStmtList.Add(
                new AssignStatement(
                    String.Format("{0}_state", machine.Name),
                new LiteralExpression(
                    String.Format("State_{0}", machine.Start.Name))));

            return new BlockStatement(bodyStmtList);
        }

        private static FunctionDeclaration MakeMachineTransitionFunction(Machine machine, string transitionName) {

            FunctionDeclaration function = new FunctionDeclaration();
            function.Name = String.Format("{0}_{1}", machine.Name, transitionName);
            function.ReturnType = TypeIdentifier.FromName("void");
            function.Body = MakeMachineTransitionFunctionBody(machine, transitionName);

            return function;
        }

        private static BlockStatement MakeMachineTransitionFunctionBody(Machine machine, string transitionName) {

            StatementList bodyStmtList = new StatementList();

            SwitchStatement switchStmt = new SwitchStatement();
            switchStmt.Expression = new IdentifierExpression(
                String.Format("{0}_state", machine.Name));

            foreach (var state in machine.States) {
                if (state.HasTransitions)
                    foreach (var stateTransitionName in state.GetTransitionNames()) {
                        if (stateTransitionName == transitionName) {

                            StatementList caseStmtBodyStmtList = new StatementList();
                            caseStmtBodyStmtList.Add(
                                new InvokeStatement(
                                    new InvokeExpression(
                                        new IdentifierExpression(
                                            String.Format("{0}_{1}_{2}", machine.Name, state.Name, transitionName)))));

                            CaseStatement caseStmt = new CaseStatement(
                                new LiteralExpression(
                                    String.Format("State_{0}", state.Name)),
                                new BlockStatement(caseStmtBodyStmtList));

                            switchStmt.Cases.Add(caseStmt);
                        }
                    }
            }

            switchStmt.DefaultCaseStmt = new BlockStatement();

            bodyStmtList.Add(switchStmt);

            return new BlockStatement(bodyStmtList);
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
            function.Access = AccessSpecifier.Private;
            function.Name = String.Format("{0}_{1}_{2}", machine.Name, state.Name, transitionName);
            function.ReturnType = TypeIdentifier.FromName("void");
            function.Body = MakeStateTransitionFunctionBody(machine, state, transitionName);

            return function;
        }

        private static BlockStatement MakeStateTransitionFunctionBody(Machine machine, State state, string transitionName) {

            StatementList bodyStmtList = new StatementList();

            foreach (var transition in state.Transitions) {
                if (transition.TransitionEvent.Name == transitionName) {

                    StatementList trueBlockStmtList = new StatementList(); ;

                    // Accions de sortida del estat actual
                    //
                    if ((state.ExitAction != null) && state.ExitAction.HasActivities)
                        foreach (var activity in state.ExitAction.Activities)
                            trueBlockStmtList.Add(MakeActivity(activity));

                    // Accions de la transicio
                    //
                    if ((transition.Action != null) && transition.Action.HasActivities)
                        foreach (var activity in transition.Action.Activities)
                            trueBlockStmtList.Add(MakeActivity(activity));

                    // Acciona d'entrada del nou estat
                    //
                    if ((transition.NextState != null) && (transition.NextState.EnterAction != null) && transition.NextState.EnterAction.HasActivities)
                        foreach (var activity in transition.NextState.EnterAction.Activities)
                            trueBlockStmtList.Add(MakeActivity(activity));

                    // Seleccio el nou estat
                    //
                    if (transition.NextState != null)
                        trueBlockStmtList.Add(
                            new AssignStatement(
                                String.Format("{0}_state", machine.Name),
                            new LiteralExpression(
                                String.Format("State_{0}", transition.NextState.Name))));

                    // Retorn
                    //
                    trueBlockStmtList.Add(new ReturnStatement());

                    bodyStmtList.Add(new IfThenElseStatement(
                        transition.Guard == null ? 
                            (Expression) new LiteralExpression(true) : 
                            (Expression) new InlineExpression(transition.Guard.Expression),
                        new BlockStatement(trueBlockStmtList),
                        null));
                }
            }

            return new BlockStatement(bodyStmtList);
        }

        private static Statement MakeActivity(Activity activity) {

            if (activity is RunActivity runActivity)
                return new InvokeStatement(
                    new InvokeExpression(
                        new IdentifierExpression(runActivity.ProcessName)));

            else if (activity is InlineActity inlineActivity)
                return new InlineStatement(inlineActivity.Text);

            else
                throw new InvalidOperationException("Tipo de actividad no reconocida.");
        }
    }
}
