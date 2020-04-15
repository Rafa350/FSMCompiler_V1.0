namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP {

    using System;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    public class StateUnitGenerator {

        public enum Variant {
            Header,
            Code
        }

        private readonly string nsName;
        private readonly string ownerClassName;
        private readonly string contextClassName;
        private readonly string stateClassName;
        private readonly string stateBaseClassName;

        public StateUnitGenerator(CPPGeneratorOptions options) {

            nsName = options.NsName;
            ownerClassName = options.OwnerClassName;
            contextClassName = options.ContextClassName;
            stateClassName = options.StateClassName;
            stateBaseClassName = options.StateBaseClassName;
        }

        public UnitDeclaration Generate(Machine machine, Variant variant) {

            UnitBuilder ub = new UnitBuilder();

            TypeIdentifier voidTypeIdentifier = TypeIdentifier.FromName("void");
            TypeIdentifier contextTypePtrIdentifier = TypeIdentifier.FromName(String.Format("{0}*", contextClassName));

            // Obra un espai de noms si cal.
            //
            if (!String.IsNullOrEmpty(nsName))
                ub.BeginNamespace(nsName);

            // Declara la clase de context
            //
            if (variant == Variant.Header)
                ub.AddForwardClassDeclaration(contextClassName);

            // Declara la clase d'estat base
            //
            ub.BeginClass(stateClassName, stateBaseClassName, AccessSpecifier.Public);

            // Declara el constructor
            //
            ub.AddMemberDeclaration(new ConstructorDeclaration {
                Access = AccessSpecifier.Protected,
                Arguments = new ArgumentDeclarationList {
                    new ArgumentDeclaration("context", contextTypePtrIdentifier)
                },
                Initializers = new ConstructorInitializerList {
                    new ConstructorInitializer(
                        stateBaseClassName,
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

                ub.BeginClass(String.Format("{0}", state.Name), stateClassName, AccessSpecifier.Public);

                // Declara el constructor.
                //
                ub.AddMemberDeclaration(new ConstructorDeclaration {
                    Access = AccessSpecifier.Public,
                    Arguments = new ArgumentDeclarationList {
                        new ArgumentDeclaration("context", contextTypePtrIdentifier)
                    },
                    Initializers = new ConstructorInitializerList {
                        new ConstructorInitializer(
                            stateClassName,
                            new IdentifierExpression("context"))
                    }
                });

                // Declara la fuincio 'enter'.
                //
                if (state.EnterAction != null)
                    ub.AddMemberDeclaration(MakeOnEnterFunction(state, contextClassName, ownerClassName));

                // Declara la funcio 'exit'
                //
                if (state.ExitAction != null)
                    ub.AddMemberDeclaration(MakeOnExitFunction(state, contextClassName, ownerClassName));

                // Declara les funcions de transicio
                //
                foreach (var transitionName in state.GetTransitionNames())
                    ub.AddMemberDeclaration(MakeOnTransitionFunction(state, transitionName, contextClassName, ownerClassName));

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
        private FunctionDeclaration MakeOnEnterFunction(State state, string contextClassName, string ownerClassName) {

            BlockStatement body = new BlockStatement();
            body.Statements.Add(
                new InlineStatement(
                    String.Format("{0}* ctx = static_cast<{0}*>(getContext())", contextClassName)));
            body.Statements.Add(
                new InlineStatement(
                    String.Format("{0}* owner = ctx->getOwner()", ownerClassName)));
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
        private FunctionDeclaration MakeOnExitFunction(State state, string contextClassName, string ownerClassName) {

            BlockStatement body = new BlockStatement();

            body.Statements.Add(
                new InlineStatement(
                    String.Format("{0}* ctx = static_cast<{0}*>(getContext())", contextClassName)));
            body.Statements.Add(
                new InlineStatement(
                    String.Format("{0}* owner = ctx->getOwner()", ownerClassName)));
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
        private FunctionDeclaration MakeOnTransitionFunction(State state, string transitionName, string contextClassName, string ownerClassName) {

            StatementList bodyStatements = new StatementList();

            // Intruccio per recuperar el context.
            //
            bodyStatements.Add(
                new InlineStatement(
                    String.Format("{0}* ctx = static_cast<{0}*>(getContext())", contextClassName)));
            bodyStatements.Add(
                new InlineStatement(
                    String.Format("{0}* owner = ctx->getOwner()", ownerClassName)));

            foreach (Transition transition in state.Transitions) {
                if (transition.TransitionEvent.Name == transitionName) {

                    StatementList trueBodyStatements = new StatementList();

                    trueBodyStatements.Add(new InvokeStatement(
                        new InvokeExpression(
                            new IdentifierExpression("ctx->beginTransition"))));

                    // Accio de transicio.
                    //
                    if (transition.Action != null)
                        trueBodyStatements.AddRange(MakeActionStatements(transition.Action));

                    trueBodyStatements.Add(new InvokeStatement(
                        new InvokeExpression(
                            new IdentifierExpression("ctx->endTransition"),
                            new InvokeExpression(
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
        private StatementList MakeActionStatements(Model.Action action) {

            if (action.Activities == null)
                return null;

            else {
                StatementList statements = new StatementList();
                foreach (var activity in action.Activities) {
                    if (activity is RunActivity callActivity) {
                        Statement statement = new InvokeStatement(
                            new InvokeExpression(
                                new IdentifierExpression(String.Format("owner->do{0}", callActivity.ProcessName))));
                        statements.Add(statement);
                    }
                }
                return statements;
            }
        }
    }
}
