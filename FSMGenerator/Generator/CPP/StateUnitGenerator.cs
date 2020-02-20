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

            TypeIdentifier voidTypeIdentifier = TypeIdentifier.FromName("void");
            TypeIdentifier contextTypeIdentifier = TypeIdentifier.FromName(options.ContextClassName);
            TypeIdentifier contextTypePtrIdentifier = TypeIdentifier.FromName(String.Format("{0}*", options.ContextClassName));

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
            ub.BeginClass(options.StateClassName, options.StateBaseClassName, AccessSpecifier.Public);

            // Declara el constructor
            //
            ub.AddMemberDeclaration(new ConstructorDeclaration {
                Access = AccessSpecifier.Protected,
                Arguments = new ArgumentDeclarationList {
                    new ArgumentDeclaration("context", contextTypePtrIdentifier)
                },
                Initializers = new ConstructorInitializerList {
                    new ConstructorInitializer(
                        options.StateBaseClassName,
                        new IdentifierExpression("context"))
                }
            });

            // Declara la functio 'enter'
            //
            ub.AddMemberDeclaration(new FunctionDeclaration {
                Access = AccessSpecifier.Public,
                Implementation = ImplementationSpecifier.Virtual,
                ReturnType = voidTypeIdentifier,
                Name = "enter"
            });

            // Declara la fuincio 'exit'
            //
            ub.AddMemberDeclaration(new FunctionDeclaration {
                Access = AccessSpecifier.Public,
                Implementation = ImplementationSpecifier.Virtual,
                ReturnType = voidTypeIdentifier,
                Name = "exit"
            });

            foreach (var transitionName in machine.GetTransitionNames()) {
                ub.AddMemberDeclaration(new FunctionDeclaration {
                    Access = AccessSpecifier.Public,
                    Implementation = ImplementationSpecifier.Virtual,
                    ReturnType = voidTypeIdentifier,
                    Name = String.Format("transition_{0}", transitionName)
                });
            }

            // Finalitza la clase 
            //
            ub.EndClass();

            // Crea les clases d'estat derivades
            //
            foreach (var state in machine.States) {

                ub.BeginClass(String.Format("{0}", state.Name), options.StateClassName, AccessSpecifier.Public);

                // Declara el constructor.
                //
                ub.AddMemberDeclaration(new ConstructorDeclaration {
                    Access = AccessSpecifier.Public,
                    Arguments = new ArgumentDeclarationList {
                        new ArgumentDeclaration("context", contextTypePtrIdentifier)
                    },
                    Initializers = new ConstructorInitializerList {
                        new ConstructorInitializer(
                            options.StateClassName,
                            new IdentifierExpression("context"))
                    }
                });

                // Declara la fuincio 'enter'.
                //
                if (state.EnterAction != null)
                    ub.AddMemberDeclaration(MakeOnEnterFunction(state, options.ContextClassName));

                // Declara la funcio 'exit'
                //
                if (state.ExitAction != null)
                    ub.AddMemberDeclaration(MakeOnExitFunction(state, options.ContextClassName));

                // Declara les funcions de transicio
                //
                foreach (var transitionName in state.GetTransitionNames())
                    ub.AddMemberDeclaration(MakeOnTransitionFunction(state, transitionName, options.ContextClassName));

                ub.EndClass();
            }

            return ub.ToUnit();
        }

        /// <summary>
        /// Genera la funcio 'enter'
        /// </summary>
        /// <param name="state">L'estat.</param>
        /// <returns>La funcio.</returns>
        /// 
        private static FunctionDeclaration MakeOnEnterFunction(State state, string contextClassName) {

            BlockStatement body = new BlockStatement();
            body.Statements.Add(
                new InlineStatement(
                    String.Format("{0}* ctx = static_cast<{0}*>(getContext())", contextClassName)));
            body.Statements.AddRange(
                MakeActionStatements(state.EnterAction));

            return new FunctionDeclaration {
                Access = AccessSpecifier.Public,
                Implementation = ImplementationSpecifier.Override,
                ReturnType = TypeIdentifier.FromName("void"),
                Name = "enter",
                Body = body
            };
        }

        /// <summary>
        /// Genera la funcio 'exit'
        /// </summary>
        /// <param name="state">L'estat.</param>
        /// <returns>La funcio.</returns>
        /// 
        private static FunctionDeclaration MakeOnExitFunction(State state, string contextClassName) {

            BlockStatement body = new BlockStatement();

            body.Statements.Add(
                new InlineStatement(
                    String.Format("{0}* ctx = static_cast<{0}*>(getContext())", contextClassName)));
            body.Statements.AddRange(
                MakeActionStatements(state.ExitAction));

            return new FunctionDeclaration {
                Access = AccessSpecifier.Public,
                Implementation = ImplementationSpecifier.Override,
                ReturnType = TypeIdentifier.FromName("void"),
                Name = "exit",
                Body = body
            };
        }

        /// <summary>
        /// Construeix la funcio de transicio.
        /// </summary>
        /// <param name="state">El estat.</param>
        /// <param name="transitionName">El nom de la transicio.</param>
        /// <returns>La funcio.</returns>
        /// 
        private static FunctionDeclaration MakeOnTransitionFunction(State state, string transitionName, string contextClassName) {

            StatementList bodyStatements = new StatementList();

            // Intruccio per recuperar el context.
            //
            bodyStatements.Add(
                new InlineStatement(
                    String.Format("{0}* ctx = static_cast<{0}*>(getContext())", contextClassName)));

            foreach (Transition transition in state.Transitions) {
                if (transition.Name == transitionName) {

                    StatementList trueBodyStatements = new StatementList();

                    trueBodyStatements.Add(new FunctionCallStatement(
                        new FunctionCallExpression(
                            new IdentifierExpression("ctx->clearState"))));

                    // Accio de transicio.
                    //
                    if (transition.Action != null)
                        trueBodyStatements.AddRange(MakeActionStatements(transition.Action));

                    trueBodyStatements.Add(new FunctionCallStatement(
                        new FunctionCallExpression(
                            new IdentifierExpression("ctx->setState"),
                            new FunctionCallExpression(
                                new IdentifierExpression("ctx->getStateInstance"),
                                new IdentifierExpression(
                                    String.Format("Context::StateID::{0}", transition.NextState.Name))))));

                    Expression conditionExpr = new InlineExpression(transition.Guard == null ? "true" : transition.Guard.Expression);
                    bodyStatements.Add(new IfThenElseStatement(
                        conditionExpr,
                        new BlockStatement { Statements = trueBodyStatements },
                        null));
                }
            }

            return new FunctionDeclaration {
                Access = AccessSpecifier.Public,
                Implementation = ImplementationSpecifier.Override,
                ReturnType = TypeIdentifier.FromName("void"),
                Name = String.Format("transition_{0}", transitionName),
                Body = new BlockStatement(bodyStatements),
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
                                new IdentifierExpression(String.Format("ctx->do{0}", callActivity.ProcessName))));
                        statements.Add(statement);
                    }
                }
                return statements;
            }
        }
    }
}
