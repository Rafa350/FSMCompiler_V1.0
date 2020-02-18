namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP {

    using System;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    public static class StateUnitGenerator {

        public enum Variant {
            Header,
            Code
        }

        public static UnitDeclaration Generate(Machine machine, CPPGeneratorOptions options, Variant variant) {

            UnitBuilder ub = new UnitBuilder();

            // Obra un espai de noms si cal.
            //
            if (!String.IsNullOrEmpty(options.NsName))
                ub.BeginNamespace(options.NsName);

            // Declara la clase de context
            //
            if (variant == Variant.Header)
                ub.AddForwardClassDeclaration(options.ContextClassName);

            // Declara la clase d'estat base
            //
            ub.BeginClass(options.StateClassName, options.StateBaseClassName, AccessMode.Public);

            ub.AddConstructorDeclaration(new ConstructorDeclaration {
                Access = AccessMode.Protected
            });

            foreach (var transitionName in machine.GetTransitionNames()) {
                ub.AddMemberFunctionDeclaration(new MemberFunctionDeclaration {
                    Access = AccessMode.Public,
                    Mode = MemberFunctionMode.Virtual,
                    ReturnType = TypeIdentifier.FromName("void"),
                    Name = String.Format("on{0}", transitionName),
                    Arguments = new ArgumentDeclarationList {
                        new ArgumentDeclaration {
                            Name = "context",
                            ValueType = TypeIdentifier.FromName(String.Format("{0}*", options.ContextClassName))
                        }
                    }
                });
            }

            ub.EndClass();

            // Crea les clases d'estat derivades
            //
            foreach (var state in machine.States) {
                ub.BeginClass(String.Format("{0}", state.Name), options.StateClassName, AccessMode.Public);

                ub.AddConstructorDeclaration(new ConstructorDeclaration {
                    Access = AccessMode.Protected
                });

                ub.AddMemberFunctionDeclaration(MakeGetInstanceFunction(state));

                foreach (var transitionName in state.GetTransitionNames())
                    ub.AddMemberFunctionDeclaration(MakeOnTransitionFunction(state, transitionName, options.ContextClassName));

                ub.AddMemberVariableDeclaration(new MemberVariableDeclaration {
                    Access = AccessMode.Private,
                    Name = "instance",
                    ValueType = TypeIdentifier.FromName(String.Format("{0}*", state.Name)),
                    Mode = MemberVariableMode.Static,
                    Initializer = new LiteralExpression("nullptr")
                });

                ub.EndClass();
            }

            return ub.ToUnit();
        }

        /// <summary>
        /// Construeix la funcio 'getInstance'
        /// </summary>
        /// <param name="state">El estat.</param>
        /// <returns>La funcio.</returns>
        /// 
        private static MemberFunctionDeclaration MakeGetInstanceFunction(State state) {

            return new MemberFunctionDeclaration {
                Name = "getInstance",
                Access = AccessMode.Public,
                Mode = MemberFunctionMode.Static,
                ReturnType = TypeIdentifier.FromName(String.Format("{0}*", state.Name)),
                Body = new BlockStatement(new StatementList {
                        new InlineStatement(String.Format("if (instance == nullptr) instance = new {0}()", state.Name)),
                        new ReturnStatement(
                            new IdentifierExpression("instance"))
                    })
            };
        }

        /// <summary>
        /// Construeix la funcio de les transicions.
        /// </summary>
        /// <param name="state">El estat.</param>
        /// <param name="transitionName">El nom de la transicio.</param>
        /// <returns>La funcio.</returns>
        /// 
        private static MemberFunctionDeclaration MakeOnTransitionFunction(State state, string transitionName, string contextClassName) {

            StatementList bodyStatements = new StatementList();

            foreach (Transition transition in state.Transitions) {
                if (transition.Name == transitionName) {

                    StatementList trueBodyStatements = new StatementList();

                    // Accio 'Exit' del estat actual.
                    //
                    if (transition.NextState != state)
                        if (state.ExitAction != null)
                            trueBodyStatements.AddRange(MakeActionStatements(state.ExitAction));

                    // Accio de transicio.
                    //
                    if (transition.Action != null)
                        trueBodyStatements.AddRange(MakeActionStatements(transition.Action));

                    // Accio 'Enter' del nou estat.
                    //
                    if (transition.NextState != state)
                        if (transition.NextState.EnterAction != null)
                            trueBodyStatements.AddRange(MakeActionStatements(transition.NextState.EnterAction));

                    if (transition.NextState != null) {
                        trueBodyStatements.Add(new FunctionCallStatement(
                            new FunctionCallExpression(
                                new IdentifierExpression("context->setState"),
                                new FunctionCallExpression(
                                    new IdentifierExpression(String.Format("{0}::getInstance", transition.NextState.Name))))));
                    }

                    Expression conditionExpr = new InlineExpression(transition.Guard == null ? "true" : transition.Guard.Expression);
                    bodyStatements.Add(new IfThenElseStatement(
                        conditionExpr,
                        new BlockStatement { Statements = trueBodyStatements },
                        null));
                }
            }

            return new MemberFunctionDeclaration {
                Access = AccessMode.Public,
                Mode = MemberFunctionMode.Override,
                ReturnType = TypeIdentifier.FromName("void"),
                Name = String.Format("on{0}", transitionName),
                Body = new BlockStatement(bodyStatements),
                Arguments = new ArgumentDeclarationList {
                    new ArgumentDeclaration {
                        Name = "context",
                        ValueType = TypeIdentifier.FromName(String.Format("{0}*", contextClassName))
                    }
                }
            };
        }

        /// <summary>
        /// Construeix les instruccions coresponents a una accio.
        /// </summary>
        /// <param name="action">La accio.</param>
        /// <returns>La llista d'instruccions.</returns>
        /// 
        private static StatementList MakeActionStatements(Model.Action action) {

            if (action.Activities == null)
                return null;

            else {
                StatementList statements = new StatementList();
                foreach (var activity in action.Activities) {
                    if (activity is RunActivity callActivity) {
                        Statement statement = new FunctionCallStatement(
                            new FunctionCallExpression(
                                new IdentifierExpression(String.Format("context->do{0}", callActivity.ProcessName))));
                        statements.Add(statement);
                    }
                }
                return statements;
            }
        }
    }
}
