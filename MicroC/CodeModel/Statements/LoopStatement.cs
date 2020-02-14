namespace MicroCompiler.CodeModel.Statements {

    using System;

    public enum ConditionPosition {
        PreLoop,
        PostLoop
    }

    public sealed class LoopStatement : Statement {

        private readonly ConditionPosition conditionPosition;
        private readonly Expression conditionExpression;
        private readonly Block body;

        public LoopStatement(ConditionPosition conditionPosition, Expression conditionExpression, Block body) {

            this.conditionPosition = conditionPosition;
            this.conditionExpression = conditionExpression ?? throw new ArgumentNullException(nameof(conditionExpression));
            this.body = body;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public ConditionPosition ConditionPosition => conditionPosition;

        public Expression ConditionExpression => conditionExpression;

        public Block Body => body;

    }
}
