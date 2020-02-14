namespace MicroCompiler.CodeModel.Expressions {

    using System;

    /// <summary>
    /// Clase que representa una expressio condicional.
    /// </summary>
    /// 
    public sealed class ConditionalExpression : Expression {

        private readonly Expression conditionExpression;
        private readonly Expression trueExpression;
        private readonly Expression falseExpression;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conditionExpression">Expressio de la condicio.</param>
        /// <param name="trueExpression">Expressio de la branca true.</param>
        /// <param name="falseExpression">Expressio de la branca false.</param>
        /// 
        public ConditionalExpression(Expression conditionExpression, Expression trueExpression, Expression falseExpression) {

            this.conditionExpression = conditionExpression ?? throw new ArgumentNullException(nameof(conditionExpression));
            this.trueExpression = trueExpression ?? throw new ArgumentNullException(nameof(trueExpression));
            this.falseExpression = falseExpression ?? throw new ArgumentNullException(nameof(falseExpression));
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
        public Expression ConditionExpression => conditionExpression;

        /// <summary>
        /// Obte l'expressio de la branca true.
        /// </summary>
        /// 
        public Expression TrueExpression => trueExpression;

        /// <summary>
        /// Obte l'expressio de la branca false.
        /// </summary>
        /// 
        public Expression FalseExpression => falseExpression;
    }
}
