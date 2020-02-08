namespace MicroCompiler.CodeModel.Statements {

    using System;

    public enum ConditionPosition {
        PreLoop,
        PostLoop
    }

    public sealed class LoopStatement : StatementBase {

        private readonly ConditionPosition conditionPosition;
        private readonly ExpressionBase conditionExpression;
        private readonly Block body;

        public LoopStatement(ConditionPosition conditionPosition, ExpressionBase conditionExpression, Block body) {

            if (conditionExpression == null)
                throw new ArgumentNullException(nameof(conditionExpression));

            this.conditionPosition = conditionPosition;
            this.conditionExpression = conditionExpression;
            this.body = body;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public ConditionPosition ConditionPosition {
            get {
                return conditionPosition;
            }
        }

        public ExpressionBase ConditionExpression {
            get {
                return conditionExpression;
            }
        }

        public Block Body {
            get {
                return body;
            }
        }
    }
}
