namespace MicroCompiler.CodeModel.Expressions {

    using System;

    /// <summary>
    /// Clase que representa una expressio condicional.
    /// </summary>
    /// 
    public sealed class ConditionalExpression : Expression {

        private readonly Expression conditionExp;
        private readonly Expression trueExp;
        private readonly Expression falseExp;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conditionExp">Expressio de la condicio.</param>
        /// <param name="trueExp">Expressio de la branca true.</param>
        /// <param name="falseExp">Expressio de la branca false.</param>
        /// 
        public ConditionalExpression(Expression conditionExp, Expression trueExp, Expression falseExp) {

            this.conditionExp = conditionExp ?? throw new ArgumentNullException(nameof(conditionExp));
            this.trueExp = trueExp ?? throw new ArgumentNullException(nameof(trueExp));
            this.falseExp = falseExp ?? throw new ArgumentNullException(nameof(falseExp));
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
        /// Obte l'expressio de la condicio.
        /// </summary>
        /// 
        public Expression ConditionExp => conditionExp;

        /// <summary>
        /// Obte l'expressio de la branca true.
        /// </summary>
        /// 
        public Expression TrueExp => trueExp;

        /// <summary>
        /// Obte l'expressio de la branca false.
        /// </summary>
        /// 
        public Expression FalseExp => falseExp;
    }
}
