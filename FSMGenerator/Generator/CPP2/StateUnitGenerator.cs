namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.Collections.Generic;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    public static class StateUnitGenerator {

        private static CPPGeneratorOptions options;

        public static UnitDeclaration Generate(Machine machine, CPPGeneratorOptions options) {

            StateUnitGenerator.options = options;

            return MakeUnit(machine);
        }

        /// <summary>
        /// Crea la declaracio de la unitat de compilacio.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La unitat de compilacio.</returns>
        /// 
        private static UnitDeclaration MakeUnit(Machine machine) {

            // Crea la clase del estat
            //
            List<IUnitMember> classDeclList = new List<IUnitMember>();
            classDeclList.Add(MakeStateClass(machine));

            // Crea les clases dels estats derivats.
            //
            foreach (State state in machine.States)
                classDeclList.Add(MakeDerivedStateClass(state));

            // Crea la unitat de compilacio.
            //
            UnitDeclaration unitDecl = new UnitDeclaration();
            if (String.IsNullOrEmpty(options.NsName))
                unitDecl.AddMembers(classDeclList);
            else {
                NamespaceDeclaration namespaceDecl = new NamespaceDeclaration();
                namespaceDecl.Name = options.NsName;
                namespaceDecl.AddMembers(classDeclList);
                unitDecl.AddMember(namespaceDecl);
            }

            return unitDecl;
        }

        private static ClassDeclaration MakeStateClass(Machine machine) {

            ClassDeclaration classDecl = new ClassDeclaration();
            classDecl.Name = options.StateClassName;
            classDecl.BaseName = options.StateBaseClassName;
            classDecl.BaseAccess = AccessMode.Public;
            classDecl.AddConstructor(new ConstructorDeclaration(AccessMode.Protected));

            foreach (var transitionName in machine.GetTransitionNames()) {
                MemberFunctionDeclaration functionDecl = new MemberFunctionDeclaration();
                functionDecl.Access = AccessMode.Public;
                functionDecl.Mode = MemberFunctionMode.Virtual;
                functionDecl.ReturnType = TypeIdentifier.FromName("void");
                functionDecl.Name = String.Format("on{0}", transitionName);
                functionDecl.AddArgument(new ArgumentDefinition("context", TypeIdentifier.FromName(
                    String.Format("{0}*", options.ContextClassName))));

                classDecl.AddMemberFunction(functionDecl);
            }

            return classDecl;
        }

        private static ClassDeclaration MakeDerivedStateClass(State state) {

            ClassDeclaration classDecl = new ClassDeclaration();
            classDecl.Name = String.Format("{0}", state.Name);
            classDecl.BaseName = options.StateClassName;
            classDecl.BaseAccess = AccessMode.Public;

            // Afegeix la variable 'instance'
            //
            classDecl.AddMemberVariable(MakeInstanceVariable(state));

            // Afegeix el constructor
            //
            classDecl.AddConstructor(new ConstructorDeclaration(AccessMode.Protected));

            // Afegeix la funcio 'getInstance'
            //
            classDecl.AddMemberFunction(MakeGetInstanceFunction(state));

            // Afegeix les funcions de transicio
            foreach (var transitionName in state.GetTransitionNames())
                classDecl.AddMemberFunction(MakeOnTransitionFunction(state, transitionName));

            return classDecl;
        }

        /// <summary>
        /// Crea la funcio 'getInstance'
        /// </summary>
        /// <param name="state">El estat.</param>
        /// <returns>La funcio.</returns>
        /// 
        private static MemberFunctionDeclaration MakeGetInstanceFunction(State state) {

            Block body = new Block();
            body.AddStatement(new InlineStatement(
                String.Format("if (instance == nullptr) instance = new {0}()", state.Name)));
            body.AddStatement(
                new ReturnStatement(new IdentifierExpression("instance")));

            MemberFunctionDeclaration functionDecl = new MemberFunctionDeclaration();
            functionDecl.Name = "getInstance";
            functionDecl.Access = AccessMode.Public;
            functionDecl.Mode = MemberFunctionMode.Static;
            functionDecl.ReturnType = TypeIdentifier.FromName(String.Format("{0}*", state.Name));
            functionDecl.Body = body;

            return functionDecl;
        }

        /// <summary>
        /// Obte la funcio de les transicions.
        /// </summary>
        /// <param name="state">El estat.</param>
        /// <param name="transitionName">El nom de la transicio.</param>
        /// <returns>La funcio.</returns>
        /// 
        private static MemberFunctionDeclaration MakeOnTransitionFunction(State state, string transitionName) {

            Block body = new Block();

            foreach (Transition transition in state.Transitions) {
                if (transition.Name == transitionName) {

                    Block trueBody = new Block();

                    // Accio 'Exit'
                    //
                    if (transition.NextState != state) {
                        if (state.ExitAction != null)
                            trueBody.AddStatements(MakeActionStatements(state.ExitAction));
                    }

                    // Accio de transicio.
                    //
                    if (transition.Action != null)
                        trueBody.AddStatements(MakeActionStatements(transition.Action));

                    // Accio 'Enter'
                    //
                    if (transition.NextState != state) {
                        if (transition.NextState.EnterAction != null)
                            trueBody.AddStatements(MakeActionStatements(transition.NextState.EnterAction));
                    }

                    if (transition.NextState != null) {
                        trueBody.AddStatement(new FunctionCallStatement(
                            new FunctionCallExpression(
                                new IdentifierExpression("context->setState"),
                                new FunctionCallExpression(
                                    new IdentifierExpression(String.Format("{0}::getInstance", transition.NextState.Name))))));
                    }

                    ExpressionBase conditionExpr = new InlineExpression(transition.Guard == null ? "true" : transition.Guard.Expression);
                    body.AddStatement(new IfThenElseStatement(conditionExpr, trueBody, null));
                }
            }

            MemberFunctionDeclaration functionDecl = new MemberFunctionDeclaration();
            functionDecl.Access = AccessMode.Public;
            functionDecl.Mode = MemberFunctionMode.Override;
            functionDecl.ReturnType = TypeIdentifier.FromName("void");
            functionDecl.Name = String.Format("on{0}", transitionName);
            functionDecl.AddArgument(new ArgumentDefinition("context", TypeIdentifier.FromName(
                String.Format("{0}*", options.ContextClassName))));
            functionDecl.Body = body;

            return functionDecl;
        }

        /// <summary>
        /// Crea la variable 'instance'
        /// </summary>
        /// <param name="state">El estat.</param>
        /// <returns>La variable.</returns>
        /// 
        private static MemberVariableDeclaration MakeInstanceVariable(State state) {

            MemberVariableDeclaration variableDecl = new MemberVariableDeclaration();
            variableDecl.Access = AccessMode.Private;
            variableDecl.Name = "instance";
            variableDecl.ValueType = TypeIdentifier.FromName(String.Format("{0}*", state.Name));
            variableDecl.Mode = MemberVariableMode.Static;
            variableDecl.initializer = new LiteralExpression("nullptr");

            return variableDecl;
        }

        /// <summary>
        /// Crea el programa coresponent a una accio.
        /// </summary>
        /// <param name="action">La accio.</param>
        /// <returns>El programa.</returns>
        /// 
        private static IEnumerable<StatementBase> MakeActionStatements(Model.Action action) {

            List<StatementBase> stmtList = null;

            foreach (var activity in action.Activities) {
                if (activity is CallActivity callActivity) {
                    StatementBase stmt = new FunctionCallStatement(
                        new FunctionCallExpression(
                            new IdentifierExpression(
                                String.Format("context->do{0}", callActivity.MethodName)),
                            null));
                    if (stmtList == null)
                        stmtList = new List<StatementBase>();
                    stmtList.Add(stmt);
                }
            }

            return stmtList;
        }
    }
}
