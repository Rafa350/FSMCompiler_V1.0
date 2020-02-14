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
            List<IUnitMember> memberList = new List<IUnitMember>();
            if (String.IsNullOrEmpty(options.NsName))
                memberList.Add(classDecl);

            else {
                NamespaceDeclaration namespaceDecl = new NamespaceDeclaration {
                    Name = options.NsName
                };
                namespaceDecl.AddMember(classDecl);
                memberList.Add(namespaceDecl);
            }

            return new UnitDeclaration(memberList);
        }

        /// <summary>
        /// Crea la declaracio de la clase de context.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio de la clase.</returns>
        /// 
        private static ClassDeclaration MakeClassDeclaration(Machine machine) {

            // Crea el constructor
            //
            ConstructorDeclaration constructorDecl = new ConstructorDeclaration {
                Access = AccessMode.Public
            };

            // Crea les funcions membre
            //
            MemberFunctionDeclarationList memberFunctionDeclList = new MemberFunctionDeclarationList();
            memberFunctionDeclList.Add(MakeStartFunction(machine));
            memberFunctionDeclList.Add(MakeEndFunction(machine));
            foreach (var transitionName in machine.GetTransitionNames())
                memberFunctionDeclList.Add(MakeTransitionFunction(transitionName));
            foreach (var activityName in machine.GetActivityNames())
                memberFunctionDeclList.Add(MakeActivityFunction(activityName));

            // Crea la clase
            //
            return new ClassDeclaration {
                Name = options.ContextClassName,
                BaseName = options.ContextBaseClassName,
                BaseAccess = AccessMode.Public,
                Constructors = new ConstructorDeclarationList {
                    constructorDecl
                },
                Functions = memberFunctionDeclList
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
            if (machine.InitializeAction != null)
                bodyStatements.AddRange(MakeActionStatements(machine.InitializeAction));
            if (machine.Start.EnterAction != null)
                bodyStatements.AddRange(MakeActionStatements(machine.Start.EnterAction));
            bodyStatements.Add(
                new InlineStatement(
                    String.Format("setState({0}::getInstance())", machine.Start.FullName)));

            return new MemberFunctionDeclaration {
                Name = "start",
                ReturnType = TypeIdentifier.FromName("void"),
                Access = AccessMode.Public,
                Body = new Block {
                    Statements = bodyStatements
                }
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
        private static MemberFunctionDeclaration MakeTransitionFunction(string transitionName) {

            return new MemberFunctionDeclaration {
                Name = String.Format("on{0}", transitionName),
                ReturnType = TypeIdentifier.FromName("void"),
                Access = AccessMode.Public,
                Body = new Block {
                    Statements = new StatementList {
                        new InlineStatement(
                            String.Format("static_cast<{0}*>(getState())->on{1}(this)", options.StateClassName, transitionName))
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
        /// Crea el programa coresponent a una accio.
        /// </summary>
        /// <param name="action">La accio.</param>
        /// <returns>El programa.</returns>
        /// 
        private static IEnumerable<Statement> MakeActionStatements(Model.Action action) {

            List<Statement> stmtList = null;

            foreach (var activity in action.Activities) {
                if (activity is RunActivity callActivity) {
                    Statement stmt = new FunctionCallStatement(
                        new FunctionCallExpression(
                            new IdentifierExpression(
                                String.Format("do{0}", callActivity.ProcessName))));
                    if (stmtList == null)
                        stmtList = new List<Statement>();

                    stmtList.Add(stmt);
                }
            }

            return stmtList;
        }
    }
}
