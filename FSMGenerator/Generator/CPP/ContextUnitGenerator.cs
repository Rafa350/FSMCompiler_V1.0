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
    /// 
    public class ContextUnitGenerator {

        private readonly string nsName;
        private readonly string ownerClassName;
        private readonly string contextClassName;
        private readonly string contextBaseClassName;
        private readonly string stateClassName;

        /// <summary>
        /// Constrcutor.
        /// </summary>
        /// <param name="options">Opcions.</param>
        /// 
        public ContextUnitGenerator(CPPGeneratorOptions options) {

            nsName = options.NsName;
            ownerClassName = options.OwnerClassName;
            contextBaseClassName = options.ContextBaseClassName;
            contextClassName = options.ContextClassName;
            stateClassName = options.StateClassName;
        }

        /// <summary>
        /// Genera la unitat de compilacio.
        /// </summary>
        /// <param name="machine">La maquina</param>
        /// <returns>La unitat de compilacio.</returns>
        /// 
        public UnitDeclaration Generate(Machine machine) {

            UnitBuilder ub = new UnitBuilder();

            bool useNamespace = !String.IsNullOrEmpty(nsName);

            // Obre un nou espai de noms, si cal
            //
            if (useNamespace)
                ub.BeginNamespace(nsName);

            ub.AddForwardClassDeclaration(stateClassName);
            ub.AddForwardClassDeclaration(ownerClassName);

            // Obra la declaracio de clase 'Context'
            //
            ub.BeginClass(contextClassName, contextBaseClassName, AccessSpecifier.Public);

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
                ValueType = TypeIdentifier.FromName(String.Format("{0}*", stateClassName))
            });

            // Afegeix la variable pel punter al objecte propietari
            //
            ub.AddMemberDeclaration(MakeOwnerVariable(machine));

            // Afegeix el constructor
            //
            ub.AddMemberDeclaration(MakeConstructor(machine));

            // Afegeix les funcions membre
            //
            ub.AddMemberDeclaration(MakeGetStateInstanceFunction(machine));
            ub.AddMemberDeclaration(MakeGetOwnerFunction(machine));
            ub.AddMemberDeclaration(MakeStartFunction(machine));
            ub.AddMemberDeclaration(MakeEndFunction(machine));
            foreach (var transitionName in machine.GetTransitionNames())
                ub.AddMemberDeclaration(MakeTransitionFunction(transitionName));

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
        /// Crea la declaracio de la variable 'owner'
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio de la variable.</returns>
        /// 
        private VariableDeclaration MakeOwnerVariable(Machine machine) {

            return new VariableDeclaration(
                "owner",
                AccessSpecifier.Private,
                ImplementationSpecifier.Instance,
                TypeIdentifier.FromName(String.Format("{0}*", ownerClassName)),
                null);
        }

        /// <summary>
        /// Crea la declaracio del constructor.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio del constructor.</returns>
        /// 
        private ConstructorDeclaration MakeConstructor(Machine machine) {

            StatementList statements = new StatementList();
            foreach (var stateName in machine.GetStateNames())
                statements.Add(new InlineStatement(
                    String.Format("states[int(StateID::{0})] = new {0}(this)", stateName)));

            return new ConstructorDeclaration(
                AccessSpecifier.Public,
                new ArgumentDeclarationList(
                    new ArgumentDeclaration("owner", TypeIdentifier.FromName(String.Format("{0}*", ownerClassName)))),
                new ConstructorInitializerList(
                    new ConstructorInitializer("owner", new IdentifierExpression("owner"))),
                statements);
        }

        /// <summary>
        /// Crea la funcio 'getInstance'
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio de la funcio.</returns>
        /// 
        private FunctionDeclaration MakeGetStateInstanceFunction(Machine machine) {

            StatementList statements = new StatementList(
                new ReturnStatement(
                    new SubscriptExpression(
                        new IdentifierExpression("states"),
                        new CastExpression(
                            TypeIdentifier.FromName("int"),
                            new IdentifierExpression("id")
                        )
                    )
                )
            );

            return new FunctionDeclaration(
                "getStateInstance",
                AccessSpecifier.Public,
                TypeIdentifier.FromName("State*"),
                new ArgumentDeclarationList(new ArgumentDeclaration("id", TypeIdentifier.FromName("StateID"))),
                statements);
        }

        /// <summary>
        /// Crea la funcio 'getOwner'.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio de la funcio.</returns>
        /// 
        private FunctionDeclaration MakeGetOwnerFunction(Machine machine) {

            StatementList statements = new StatementList(
                new ReturnStatement(
                    new IdentifierExpression("owner")));

            return new FunctionDeclaration(
                "getOwner",
                AccessSpecifier.Public,
                TypeIdentifier.FromName(String.Format("{0}*", ownerClassName)),
                null,
                statements);
        }

        /// <summary>
        /// Crea la declaracio del metode 'start'
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private FunctionDeclaration MakeStartFunction(Machine machine) {

            StatementList bodyStatements = new StatementList();
            if (machine.InitializeAction != null) {
                var statements = MakeActionStatements(machine.InitializeAction);
                if (statements != null)
                    bodyStatements.AddRange(statements);
            }
            bodyStatements.Add(
                new InvokeStatement(
                    new InvokeExpression(
                        new IdentifierExpression("initialize"),
                        new InvokeExpression(
                            new IdentifierExpression("getStateInstance"),
                            new IdentifierExpression(
                                String.Format("StateID::{0}", machine.Start.FullName))))));

            return new FunctionDeclaration(
                "start",
                AccessSpecifier.Public,
                TypeIdentifier.FromName("void"),
                null,
                bodyStatements);
        }

        /// <summary>
        /// Crea la declaracio del metode 'end'.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private FunctionDeclaration MakeEndFunction(Machine machine) {

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
        private FunctionDeclaration MakeTransitionFunction(string transitionName) {

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
        /// Crea les instruccions coresponent a una accio.
        /// </summary>
        /// <param name="action">La accio.</param>
        /// <returns>Llista d'instruccions.</returns>
        /// 
        private StatementList MakeActionStatements(Model.Action action) {

            StatementList stmtList = null;

            foreach (var activity in action.Activities) {
                if (activity is RunActivity callActivity) {
                    Statement stmt = new InvokeStatement(
                        new InvokeExpression(
                            new IdentifierExpression(
                                String.Format("owner->do{0}", callActivity.ProcessName))));
                    if (stmtList == null)
                        stmtList = new StatementList();

                    stmtList.Add(stmt);
                }
            }

            return stmtList;
        }
    }
}
