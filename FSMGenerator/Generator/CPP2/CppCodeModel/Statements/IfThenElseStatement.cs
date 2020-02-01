namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Statements {

    using System;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel;

    public sealed class IfThenElseStatement: StatementBase {

        private readonly ExpressionBase conditionExpression;
        private readonly Block trueBlock;
        private readonly Block falseBlock;

        public IfThenElseStatement(ExpressionBase conditionExpression, Block trueBlock, Block falseBlock) {

            if (conditionExpression == null)
                throw new ArgumentNullException("conditionExpression");

            if (trueBlock == null)
                throw new ArgumentNullException("trueBlock");

            this.conditionExpression = conditionExpression;
            this.trueBlock = trueBlock;
            this.falseBlock = falseBlock;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public ExpressionBase ConditionExpression {
            get {
                return conditionExpression;
            }
        }

        public Block TrueBlock {
            get {
                return trueBlock;
            }
        }

        public Block FalseBlock {
            get {
                return falseBlock;
            }
        }
    }
}
