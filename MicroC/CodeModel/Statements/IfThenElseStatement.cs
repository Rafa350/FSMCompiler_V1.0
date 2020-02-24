namespace MicroCompiler.CodeModel.Statements {

    using System;
    using MicroCompiler.CodeModel;

    public sealed class IfThenElseStatement : Statement {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public IfThenElseStatement() {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="conditionExp">Expressio de la condicio.</param>
        /// <param name="trueStmt">Instruccio en cas true.</param>
        /// <param name="falseStmt">Instruccio en cas false.</param>
        /// 
        public IfThenElseStatement(Expression conditionExp, Statement trueStmt, Statement falseStmt) {

            ConditionExp = conditionExp ?? throw new ArgumentNullException(nameof(conditionExp));
            TrueStmt = trueStmt ?? throw new ArgumentNullException(nameof(trueStmt));
            FalseStmt = falseStmt;
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
        /// Obte o asigna l'espressio de la condicio.
        /// </summary>
        /// 
        public Expression ConditionExp { get; set; }

        /// <summary>
        /// Obte o asigna les instruccion del cas true.
        /// </summary>
        /// 
        public Statement TrueStmt { get; set; }

        /// <summary>
        /// Obte o asigna les instruccions del cass false.
        /// </summary>
        /// 
        public Statement FalseStmt { get; set; }
    }
}
