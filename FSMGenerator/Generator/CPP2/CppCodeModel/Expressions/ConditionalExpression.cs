using System;


namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Expressions {
    
    public sealed class ConditionalExpression: ExpressionBase {

        private readonly ExpressionBase conditionExpression;
        private readonly ExpressionBase trueExpression;
        private readonly ExpressionBase falseExpression;

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

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public override string ToString() {

            return "conditional expression";
        }

        public ExpressionBase ConditionExpression {
            get {
                return conditionExpression;
            }
        }

        public ExpressionBase TrueExpression {
            get {
                return trueExpression;
            }
        }

        public ExpressionBase FalseExpression {
            get {
                return falseExpression;
            }
        }
    }
}
