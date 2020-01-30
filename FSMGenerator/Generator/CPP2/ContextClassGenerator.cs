namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.Text;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    /// <summary>
    /// Genera la clase de context.
    /// </summary>
    public sealed class ContextClassGenerator {

        private readonly string nsName;
        private readonly string stateClassName;
        private readonly string contextClassName;
        private readonly string contextBaseClassName;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Opcions.</param>
        /// 
        public ContextClassGenerator(CPPGeneratorOptions options) {

            nsName = options.NsName;
            stateClassName = options.StateClassName;
            contextClassName = options.ContextClassName;
            contextBaseClassName = options.ContextBaseClassName;
        }

        /// <summary>
        /// Genera la unitat de compilacio.
        /// </summary>
        /// <param name="machine">La maquina</param>
        /// <returns>La unitat de compilacio.</returns>
        /// 
        public UnitDeclaration Generate(Machine machine) {

            return MakeUnitDeclaration(machine);
        }

        /// <summary>
        /// Crea la declaracio de la unitat de compilacio.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La unitat de compilacio.</returns>
        /// 
        private UnitDeclaration MakeUnitDeclaration(Machine machine) {

            ClassDeclaration clsDecl = MakeClassDeclaration(machine);

            UnitDeclaration unitDecl = new UnitDeclaration();

            if (String.IsNullOrEmpty(nsName))
                unitDecl.AddMember(clsDecl);

            else {
                NamespaceDeclaration nsDecl = new NamespaceDeclaration();
                nsDecl.Name = nsName;
                nsDecl.AddMember(clsDecl);
                unitDecl.AddMember(nsDecl);
            }

            return unitDecl;
        }

        /// <summary>
        /// Crea la declaracio de la clase.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio de la clase.</returns>
        /// 
        private ClassDeclaration MakeClassDeclaration(Machine machine) {

            ClassDeclaration clsDecl = new ClassDeclaration();
            clsDecl.Name = contextClassName;
            clsDecl.BaseName = contextBaseClassName;
            clsDecl.BaseAccess = AccessSpecifier.Public;
            clsDecl.AddConstructor(MakeConstructor());
            clsDecl.AddMethod(MakeStartMethod(machine));
            clsDecl.AddMethod(MakeTerminateMethod(machine));
            foreach (var transitionName in machine.GetTransitionNames())
                clsDecl.AddMethod(MakeTransitionMethod(transitionName));
            foreach (var activityName in machine.GetActivityNames())
                clsDecl.AddMethod(MakeActivityMethod(activityName));

            return clsDecl;
        }

        /// <summary>
        /// Crea la declaracio del constructor.
        /// </summary>
        /// <returns></returns>
        private ConstructorDeclaration MakeConstructor() {

            return new ConstructorDeclaration(AccessSpecifier.Public);
        }

        /// <summary>
        /// Crea la declaracio del metode 'start'
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private MethodDeclaration MakeStartMethod(Machine machine) {

            StringBuilder sb = new StringBuilder();

            if (machine.InitializeAction != null) {
                sb.AppendLine("// Machine initialization actions.");
                sb.AppendLine("//");
                sb.Append(MakeActionBody(machine.InitializeAction));
                sb.AppendLine();
            }

            if (machine.Start.EnterAction != null) {
                sb.AppendLine("// Enter state actions.");
                sb.AppendLine("//");
                sb.Append(MakeActionBody(machine.Start.EnterAction));
                sb.AppendLine();
            }

            sb.AppendLine("// Select initial state.");
            sb.AppendLine("//");
            sb.AppendFormat("setState({0}::getInstance());", machine.Start.FullName);

            return new MethodDeclaration(
                "start",
                "void",
                AccessSpecifier.Public,
                null,
                sb.ToString());
        }

        /// <summary>
        /// Crea la declaracio del metode 'end'.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private MethodDeclaration MakeTerminateMethod(Machine machine) {

            return new MethodDeclaration(
                "end",
                "void",
                AccessSpecifier.Public,
                null,
                null);
        }

        /// <summary>
        /// Crea la declaracio d'un metode de transicio.
        /// </summary>
        /// <param name="transitionName">Nom de la transicio.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private MethodDeclaration MakeTransitionMethod(string transitionName) {

            string name = String.Format("on{0}", transitionName);
            string body = String.Format("static_cast<{1}*>(getState())->on{0}(this);", transitionName, stateClassName);

            return new MethodDeclaration(
                name,
                "void",
                AccessSpecifier.Public,
                null,
                body);
        }

        /// <summary>
        /// Crea la declaracio d'un metode d'activitat.
        /// </summary>
        /// <param name="activityName">El nom de l'activitat.</param>
        /// <returns>La declaracio del metode.</returns>
        /// 
        private MethodDeclaration MakeActivityMethod(string activityName) {

            string name = String.Format("do{0}", activityName);

            return new MethodDeclaration(
                name,
                "void",
                AccessSpecifier.Public,
                null,
                null);
        }

        private string MakeActionBody(Model.Action action) {

            StringBuilder sb = new StringBuilder();

            foreach (var activity in action.Activities) {
                if (activity is CallActivity callActivity)
                    sb.AppendLine(String.Format("do{0}();", callActivity.MethodName));
            }

            return sb.ToString();
        }
    }
}
