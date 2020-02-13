namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.Collections.Generic;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    /// <summary>
    /// Genera la clase de context.
    /// </summary>
    public static class ContextUnitGenerator {

        private static CPPGeneratorOptions options;

        /// <summary>
        /// Genera la unitat de compilacio.
        /// </summary>
        /// <param name="machine">La maquina</param>
        /// <returns>La unitat de compilacio.</returns>
        /// 
        public static UnitDeclaration Generate(Machine machine, CPPGeneratorOptions options) {

            ContextUnitGenerator.options = options;

            return MakeUnitDeclaration(machine);
        }

        /// <summary>
        /// Crea la declaracio de la unitat de compilacio.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La unitat de compilacio.</returns>
        /// 
        private static UnitDeclaration MakeUnitDeclaration(Machine machine) {

            // Construeix la clase de context.
            //
            ClassDeclaration classDecl = MakeClassDeclaration(machine);

            // Construeix la unitat de compilacio.
            //
            UnitDeclaration unitDecl = new UnitDeclaration();
            if (String.IsNullOrEmpty(options.NsName)) {
                unitDecl.AddMember(classDecl);
            }
            else {
                NamespaceDeclaration namespaceDecl = new NamespaceDeclaration();
                namespaceDecl.Name = options.NsName;
                namespaceDecl.AddMember(classDecl);
                unitDecl.AddMember(namespaceDecl);
            }

            return unitDecl;
        }

        /// <summary>
        /// Crea la declaracio de la clase de context.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio de la clase.</returns>
        /// 
        private static ClassDeclaration MakeClassDeclaration(Machine machine) {

            ClassDeclaration classDecl = new ClassDeclaration();
            classDecl.Name = options.ContextClassName;
            classDecl.BaseName = options.ContextBaseClassName;
            classDecl.BaseAccess = AccessMode.Public;

            // Afegeix el constructor.
            //
            classDecl.AddConstructor(MakeConstructor());

            // Afegeix el metode 'start'.
            //
            classDecl.AddMemberFunction(MakeStartFunction(machine));

            // Afegeix la funcio 'end'
            //
            classDecl.AddMemberFunction(MakeEndFunction(machine));

            // Afegeix les funcions de les transicions.
            //
            foreach (var transitionName in machine.GetTransitionNames()) {
                classDecl.AddMemberFunction(MakeTransitionFunction(transitionName));
            }

            // Afegeix les funcions de les activitats.
            //
            foreach (var activityName in machine.GetActivityNames()) {
                classDecl.AddMemberFunction(MakeActivityFunction(activityName));
            }

            return classDecl;
        }

        /// <summary>
        /// Crea la declaracio del constructor.
        /// </summary>
        /// <returns>El constructor.</returns>
        /// 
        private static ConstructorDeclaration MakeConstructor() {

            return new ConstructorDeclaration(AccessMode.Public);
        }

        /// <summary>
        /// Crea la declaracio del metode 'start'
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private static MemberFunctionDeclaration MakeStartFunction(Machine machine) {

            Block body = new Block();

            if (machine.InitializeAction != null) {
                body.AddStatements(MakeActionStatements(machine.InitializeAction));
            }

            if (machine.Start.EnterAction != null) {
                body.AddStatements(MakeActionStatements(machine.Start.EnterAction));
            }

            body.AddStatement(
                new InlineStatement(
                    String.Format("setState({0}::getInstance())", machine.Start.FullName)));

            MemberFunctionDeclaration functionDecl = new MemberFunctionDeclaration();
            functionDecl.Name = "start";
            functionDecl.ReturnType = TypeIdentifier.FromName("void");
            functionDecl.Access = AccessMode.Public;
            functionDecl.Body = body;

            return functionDecl;
        }

        /// <summary>
        /// Crea la declaracio del metode 'end'.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private static MemberFunctionDeclaration MakeEndFunction(Machine machine) {

            MemberFunctionDeclaration functionDecl = new MemberFunctionDeclaration();
            functionDecl.Name = "end";
            functionDecl.ReturnType = TypeIdentifier.FromName("void");
            functionDecl.Access = AccessMode.Public;

            return functionDecl;
        }

        /// <summary>
        /// Crea la declaracio d'un metode de transicio.
        /// </summary>
        /// <param name="transitionName">Nom de la transicio.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private static MemberFunctionDeclaration MakeTransitionFunction(string transitionName) {

            Block body = new Block();
            body.AddStatement(
                new InlineStatement(
                    String.Format("static_cast<{0}*>(getState())->on{1}(this)", options.StateClassName, transitionName)));

            MemberFunctionDeclaration functionDecl = new MemberFunctionDeclaration();
            functionDecl.Name = String.Format("on{0}", transitionName);
            functionDecl.ReturnType = TypeIdentifier.FromName("void");
            functionDecl.Access = AccessMode.Public;
            functionDecl.Body = body;

            return functionDecl;
        }

        /// <summary>
        /// Crea la declaracio d'un metode d'activitat.
        /// </summary>
        /// <param name="activityName">El nom de l'activitat.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private static MemberFunctionDeclaration MakeActivityFunction(string activityName) {

            MemberFunctionDeclaration functionDecl = new MemberFunctionDeclaration();
            functionDecl.Name = String.Format("do{0}", activityName);
            functionDecl.ReturnType = TypeIdentifier.FromName("void");
            functionDecl.Access = AccessMode.Public;

            return functionDecl;
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
                if (activity is RunActivity callActivity) {
                    StatementBase stmt = new FunctionCallStatement(
                        new FunctionCallExpression(
                            new IdentifierExpression(
                                String.Format("do{0}", callActivity.ProcessName))));
                    if (stmtList == null) {
                        stmtList = new List<StatementBase>();
                    }

                    stmtList.Add(stmt);
                }
            }

            return stmtList;
        }
    }
}
