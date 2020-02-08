namespace MicroCompiler.CodeModel.Expressions {

    using System;

    /// <summary>
    /// Clase que representa una expressio condicional.
    /// </summary>
    /// 
    public sealed class ConditionalExpression : ExpressionBase {

        private readonly ExpressionBase conditionExpression;
        private readonly ExpressionBase trueExpression;
        private readonly ExpressionBase falseExpression;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conditionExpression">Expressio de la condicio.</param>
        /// <param name="trueExpression">Expressio de la branca true.</param>
        /// <param name="falseExpression">Expressio de la branca false.</param>
        /// 
        public ConditionalExpression(ExpressionBase conditionExpression, ExpressionBase trueExpression, ExpressionBase falseExpression) {

            if (conditionExpression == null)
                throw new ArgumentNullException(nameof(conditionExpression));

            if (trueExpression == null)
                throw new ArgumentNullException(nameof(trueExpression));

            if (falseExpression == null)
                throw new ArgumentNullException(nameof(falseExpression));

            this.conditionExpression = conditionExpression;
            this.trueExpression = trueExpression;
            this.falseExpression = falseExpression;
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
        public ExpressionBase ConditionExpression => conditionExpression;

        /// <summary>
        /// Obte l'expressio de la branca true.
        /// </summary>
        /// 
        public ExpressionBase TrueExpression => trueExpression;

        /// <summary>
        /// Obte l'expressio de la branca false.
        /// </summary>
        /// 
        public ExpressionBase FalseExpression => falseExpression;
    }
}
