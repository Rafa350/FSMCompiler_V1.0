namespace MicroCompiler.CodeModel.Statements {

    using System;
    using MicroCompiler.CodeModel;

    public sealed class IfThenElseStatement : Statement {

        private readonly Expression conditionExpression;
        private readonly Block trueBlock;
        private readonly Block falseBlock;

        public IfThenElseStatement(Expression conditionExpression, Block trueBlock, Block falseBlock) {

            this.conditionExpression = conditionExpression ?? throw new ArgumentNullException(nameof(conditionExpression));
            this.trueBlock = trueBlock ?? throw new ArgumentNullException(nameof(trueBlock));
            this.falseBlock = falseBlock;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public Expression ConditionExpression => conditionExpression;

        public Block TrueBlock => trueBlock;

        public Block FalseBlock => falseBlock;
    }
}
