namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP {

    using System;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    /// <summary>
    /// Genera la clase de context.
    /// </summary>
    public static class ContextUnitGenerator {

        public enum Variant {
            Header,
            Code
        }

        /// <summary>
        /// Genera la unitat de compilacio.
        /// </summary>
        /// <param name="machine">La maquina</param>
        /// <returns>La unitat de compilacio.</returns>
        /// 
        public static UnitDeclaration Generate(Machine machine, CPPGeneratorOptions options, Variant variant) {

            UnitBuilder ub = new UnitBuilder();

            // Obre un nou espai de noms, si cal
            //
            if (!String.IsNullOrEmpty(options.NsName))
                ub.BeginNamespace(options.NsName);

            // Obra la declaracio d'una clase
            //
            ub.BeginClass(options.ContextClassName, options.ContextBaseClassName, AccessMode.Public);

            // Afegeix el constructor
            //
            ub.AddConstructorDeclaration(MakeConstructor(machine));

            // Afegeix les funcions membre
            //
            ub.AddMemberFunctionDeclaration(MakeStartFunction(machine));
            ub.AddMemberFunctionDeclaration(MakeEndFunction(machine));
            foreach (var transitionName in machine.GetTransitionNames())
                ub.AddMemberFunctionDeclaration(MakeTransitionFunction(transitionName, options.StateClassName));

            // Es defineixen en la capcelera, pero no en el codi, per que l'usuari defineixi les funcions en
            // un altre fitxer.
            //
            if (variant == Variant.Header)
                foreach (var activityName in machine.GetActivityNames())
                    ub.AddMemberFunctionDeclaration(MakeActivityFunction(activityName));

            // Tanca la declaracio de la clase
            //
            ub.EndClass();

            // Tanca la declaracio del espai de noms, si cal.
            //
            if (!String.IsNullOrEmpty(options.NsName))
                ub.EndNamespace();

            // Retorna l'unitat.
            //
            return ub.ToUnit();
        }

        /// <summary>
        /// Crea la declaracio del constructor.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio del constructor.</returns>
        /// 
        private static ConstructorDeclaration MakeConstructor(Machine machine) {

            return new ConstructorDeclaration {
                Access = AccessMode.Public
            };
        }

        /// <summary>
        /// Crea la declaracio del metode 'start'
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private static MemberFunctionDeclaration MakeStartFunction(Machine machine) {

            StatementList bodyStatements = new StatementList();
            if (machine.InitializeAction != null) {
                var statements = MakeActionStatements(machine.InitializeAction);
                if (statements != null)
                    bodyStatements.AddRange(statements);
            }
            if (machine.Start.EnterAction != null) {
                var statements = MakeActionStatements(machine.Start.EnterAction);
                if (statements != null)
                    bodyStatements.AddRange(statements);
            }
            bodyStatements.Add(
                new InlineStatement(
                    String.Format("setState({0}::getInstance())", machine.Start.FullName)));

            return new MemberFunctionDeclaration {
                Name = "start",
                ReturnType = TypeIdentifier.FromName("void"),
                Access = AccessMode.Public,
                Body = new BlockStatement(bodyStatements)
            };
        }

        /// <summary>
        /// Crea la declaracio del metode 'end'.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private static MemberFunctionDeclaration MakeEndFunction(Machine machine) {

            return new MemberFunctionDeclaration {
                Name = "end",
                ReturnType = TypeIdentifier.FromName("void"),
                Access = AccessMode.Public
            };
        }

        /// <summary>
        /// Crea la declaracio d'un metode de transicio.
        /// </summary>
        /// <param name="transitionName">Nom de la transicio.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private static MemberFunctionDeclaration MakeTransitionFunction(string transitionName, string stateClassName) {

            return new MemberFunctionDeclaration {
                Name = String.Format("on{0}", transitionName),
                ReturnType = TypeIdentifier.FromName("void"),
                Access = AccessMode.Public,
                Body = new BlockStatement {
                    Statements = new StatementList {
                        new InlineStatement(
                            String.Format("static_cast<{0}*>(getState())->on{1}(this)", stateClassName, transitionName))
                    }
                }
            };
        }

        /// <summary>
        /// Crea la declaracio d'un metode d'activitat.
        /// </summary>
        /// <param name="activityName">El nom de l'activitat.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private static MemberFunctionDeclaration MakeActivityFunction(string activityName) {

            return new MemberFunctionDeclaration {
                Name = String.Format("do{0}", activityName),
                ReturnType = TypeIdentifier.FromName("void"),
                Access = AccessMode.Public
            };
        }

        /// <summary>
        /// Crea les instruccions coresponent a una accio.
        /// </summary>
        /// <param name="action">La accio.</param>
        /// <returns>Llista d'instruccions.</returns>
        /// 
        private static StatementList MakeActionStatements(Model.Action action) {

            StatementList stmtList = null;

            foreach (var activity in action.Activities) {
                if (activity is RunActivity callActivity) {
                    Statement stmt = new FunctionCallStatement(
                        new FunctionCallExpression(
                            new IdentifierExpression(
                                String.Format("do{0}", callActivity.ProcessName))));
                    if (stmtList == null)
                        stmtList = new StatementList();

                    stmtList.Add(stmt);
                }
            }

            return stmtList;
        }
    }
}
