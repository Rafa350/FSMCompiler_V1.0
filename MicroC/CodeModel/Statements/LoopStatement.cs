namespace MicroCompiler.CodeModel.Statements {

    using System;

    public enum ConditionPosition {
        PreLoop,
        PostLoop
    }

    public sealed class LoopStatement : Statement {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        public LoopStatement() {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="conditionPos">Situacio de la condicio.</param>
        /// <param name="conditionExp">Expressio de la condicio.</param>
        /// <param name="stmt">Instruccio.</param>
        /// 
        public LoopStatement(ConditionPosition conditionPos, Expression conditionExp, Statement stmt) {

            ConditionPos = conditionPos;
            ConditionExp = conditionExp ?? throw new ArgumentNullException(nameof(conditionExp));
            Stmt = stmt;
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

        public ConditionPosition ConditionPos {get;set;}

        public Expression ConditionExp { get; set; }

        public Statement Stmt { get; set; }
    }
}
