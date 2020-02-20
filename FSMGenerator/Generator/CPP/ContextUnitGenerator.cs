namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP {

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

            bool useNamespace = !String.IsNullOrEmpty(options.NsName);

            // Obre un nou espai de noms, si cal
            //
            if (useNamespace)
                ub.BeginNamespace(options.NsName);

            ub.AddForwardClassDeclaration(options.StateClassName);

            // Obra la declaracio de clase 'Context'
            //
            ub.BeginClass(options.ContextClassName, options.ContextBaseClassName, AccessSpecifier.Public);

            // Declara un enumerador pels estats
            //
            List<string> elements = new List<string>(machine.GetStateNames());
            ub.AddMemberDeclaration(new EnumerationDeclaration {
                Name = "StateID",
                Elements = elements
            });

            // Declara la variable per les instancies dels estats.
            //
            ub.AddMemberDeclaration(new VariableDeclaration {
                Name = String.Format("states[{0}]", elements.Count),
                ValueType = TypeIdentifier.FromName("State*")
            }); ;

            // Afegeix el constructor
            //
            ub.AddMemberDeclaration(MakeConstructor(machine));

            // Afegeix les funcions membre
            //
            ub.AddMemberDeclaration(MakeGetStateInstanceFunction(machine));
            ub.AddMemberDeclaration(MakeStartFunction(machine));
            ub.AddMemberDeclaration(MakeEndFunction(machine));
            foreach (var transitionName in machine.GetTransitionNames())
                ub.AddMemberDeclaration(MakeTransitionFunction(transitionName, options.StateClassName));

            // Es defineixen en la capcelera, pero no en el codi, per que l'usuari defineixi les funcions en
            // un altre fitxer.
            //
            if (variant == Variant.Header)
                foreach (var activityName in machine.GetActivityNames())
                    ub.AddMemberDeclaration(MakeActivityFunction(activityName));

            // Tanca la declaracio de la clase 'Context'
            //
            ub.EndClass();

            // Tanca la declaracio del espai de noms, si cal.
            //
            if (useNamespace)
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

            StatementList statements = new StatementList();
            foreach (var stateName in machine.GetStateNames())
                statements.Add(new InlineStatement(
                    String.Format("states[(int)StateID::{0}] = new {0}(this)", stateName)));

            return new ConstructorDeclaration {
                Access = AccessSpecifier.Public,
                Body = new BlockStatement(statements)
            };
        }

        private static FunctionDeclaration MakeGetStateInstanceFunction(Machine machine) {

            BlockStatement body = new BlockStatement();
            body.Statements.Add(
                new InlineStatement("return states[(int)id]"));

            return new FunctionDeclaration {
                Name = "getStateInstance",
                ReturnType = TypeIdentifier.FromName("State*"),
                Access = AccessSpecifier.Public,
                Arguments = new ArgumentDeclarationList {
                    new ArgumentDeclaration {
                        Name = "id",
                        ValueType = TypeIdentifier.FromName("StateID")
                    }
                },
                Body = body
            };
        }

        /// <summary>
        /// Crea la declaracio del metode 'start'
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private static FunctionDeclaration MakeStartFunction(Machine machine) {

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
                new FunctionCallStatement(
                    new FunctionCallExpression(
                        new IdentifierExpression("setState"),
                        new FunctionCallExpression(
                            new IdentifierExpression("getStateInstance"),
                            new IdentifierExpression(
                                String.Format("StateID::{0}", machine.Start.FullName))))));

            return new FunctionDeclaration {
                Name = "start",
                ReturnType = TypeIdentifier.FromName("void"),
                Access = AccessSpecifier.Public,
                Body = new BlockStatement(bodyStatements)
            };
        }

        /// <summary>
        /// Crea la declaracio del metode 'end'.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private static FunctionDeclaration MakeEndFunction(Machine machine) {

            return new FunctionDeclaration {
                Name = "end",
                ReturnType = TypeIdentifier.FromName("void"),
                Access = AccessSpecifier.Public
            };
        }

        /// <summary>
        /// Crea la declaracio d'un metode de transicio.
        /// </summary>
        /// <param name="transitionName">Nom de la transicio.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private static FunctionDeclaration MakeTransitionFunction(string transitionName, string stateClassName) {

            return new FunctionDeclaration {
                Name = String.Format("transition_{0}", transitionName),
                ReturnType = TypeIdentifier.FromName("void"),
                Access = AccessSpecifier.Public,
                Body = new BlockStatement {
                    Statements = new StatementList {
                        new InlineStatement(
                            String.Format("static_cast<{0}*>(getState())->transition_{1}()", stateClassName, transitionName))
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
        private static FunctionDeclaration MakeActivityFunction(string activityName) {

            return new FunctionDeclaration {
                Name = String.Format("do{0}", activityName),
                ReturnType = TypeIdentifier.FromName("void"),
                Access = AccessSpecifier.Public
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
