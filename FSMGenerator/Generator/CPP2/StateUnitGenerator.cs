namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
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
            UnitMemberDeclarationList declList = new UnitMemberDeclarationList();
            declList.Add(MakeStateClass(machine));

            // Crea les clases dels estats derivats.
            //
            foreach (State state in machine.States)
                declList.Add(MakeDerivedStateClass(state));

            // Crea la unitat de compilacio.
            //
            UnitMemberDeclarationList unitMemberDeclList = new UnitMemberDeclarationList();
            if (String.IsNullOrEmpty(options.NsName))
                unitMemberDeclList.AddRange(declList);
            else
                unitMemberDeclList.Add(new NamespaceDeclaration(options.NsName, declList));
            return new UnitDeclaration(unitMemberDeclList);
        }

        /// <summary>
        /// Construeix la declaracio de la clase base dels estats.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio de la clase.</returns>
        /// 
        private static ClassDeclaration MakeStateClass(Machine machine) {

            ConstructorDeclarationList constructorList = new ConstructorDeclarationList {
                new ConstructorDeclaration {
                    Access = AccessMode.Protected
                }
            };

            MemberFunctionDeclarationList functionList = new MemberFunctionDeclarationList();
            foreach (var transitionName in machine.GetTransitionNames()) {
                MemberFunctionDeclaration function = new MemberFunctionDeclaration {
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
                };

                functionList.Add(function);
            }

            return new ClassDeclaration {
                Name = options.StateClassName,
                BaseName = options.StateBaseClassName,
                BaseAccess = AccessMode.Public,
                Constructors = constructorList,
                Functions = functionList
            };
        }

        /// <summary>
        /// Conmstrueix la clase d'un estat concret.
        /// </summary>
        /// <param name="state">El estat.</param>
        /// <returns>La declaracio de la clase.</returns>
        /// 
        private static ClassDeclaration MakeDerivedStateClass(State state) {

            ConstructorDeclarationList constructors = new ConstructorDeclarationList {
                new ConstructorDeclaration {
                    Access = AccessMode.Protected
                }
            };

            MemberFunctionDeclarationList functions = new MemberFunctionDeclarationList();
            functions.Add(MakeGetInstanceFunction(state));
            foreach (var transitionName in state.GetTransitionNames())
                functions.Add(MakeOnTransitionFunction(state, transitionName));

            MemberVariableDeclarationList variables = new MemberVariableDeclarationList {
                new MemberVariableDeclaration {
                    Access = AccessMode.Private,
                    Name = "instance",
                    ValueType = TypeIdentifier.FromName(String.Format("{0}*", state.Name)),
                    Mode = MemberVariableMode.Static,
                    Initializer = new LiteralExpression("nullptr")
                }
            };

            return new ClassDeclaration {
                Name = String.Format("{0}", state.Name),
                BaseName = options.StateClassName,
                BaseAccess = AccessMode.Public,
                Constructors = constructors,
                Functions = functions,
                Variables = variables
            };
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
        private static MemberFunctionDeclaration MakeOnTransitionFunction(State state, string transitionName) {

            StatementList bodyStatements = new StatementList();

            foreach (Transition transition in state.Transitions) {
                if (transition.Name == transitionName) {

                    StatementList trueBodyStatements = new StatementList();

                    // Accio 'Exit'
                    //
                    if (transition.NextState != state)
                        if (state.ExitAction != null)
                            trueBodyStatements.AddRange(MakeActionStatements(state.ExitAction));

                    // Accio de transicio.
                    //
                    if (transition.Action != null)
                        trueBodyStatements.AddRange(MakeActionStatements(transition.Action));

                    // Accio 'Enter'
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
                        ValueType = TypeIdentifier.FromName(String.Format("{0}*", options.ContextClassName))
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
