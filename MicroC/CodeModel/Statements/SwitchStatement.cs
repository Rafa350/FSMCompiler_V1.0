namespace MicroCompiler.CodeModel.Statements {

    using System;
    using System.Collections.Generic;

    public sealed class SwitchStatement : Statement {

        private List<CaseStatement> cases;

        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        public SwitchStatement() {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="expression">Expressio pel calcul del valor de selccio.</param>
        /// <param name="cases">Llista de casos</param>
        /// <param name="defaultCaseStmt">Instruccio per defecte.</param>
        /// 
        public SwitchStatement(Expression expression, List<CaseStatement> cases, Statement defaultCaseStmt = null) {

            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            this.cases = cases ?? throw new ArgumentNullException(nameof(cases)); ;
            DefaultCaseStmt = defaultCaseStmt;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Obrte o asigna l'expressio.
        /// </summary>
        /// 
        public Expression Expression { get; set; }

        /// <summary>
        /// Obte o asigna la instruccio del cas per defecte.
        /// </summary>
        /// 
        public Statement DefaultCaseStmt { get; set; }

        /// <summary>
        /// Indica si conte cases.
        /// </summary>
        /// 
        public bool HasCases => (cases != null) && (cases.Count > 0);

        /// <summary>
        /// Obte o asigna la llista de casos.
        /// </summary>
        /// 
        public List<CaseStatement> Cases {
            get {
                if (cases == null)
                    cases = new List<CaseStatement>();
                return cases;
            }
            set {
                cases = value;
            }
        }
    }
}
