namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Expressions {

    using System;

    public enum BinaryOpCode {
        Add,
        Sub,
        Mul,
        Mod,
        Div,
        And,
        Or,
        Xor,
        LeftShift,
        RightShift,
        LogicalAnd,
        LogicalOr,
        Equal,
        NoEqual,
        Less,
        LessOrEqual,
        Greather,
        GreatherOrEqual,
    }
    
    public sealed class BinaryExpression: ExpressionBase {

        private readonly BinaryOpCode opCode;
        private readonly ExpressionBase leftExpression;
        private readonly ExpressionBase rightExpression;

        public BinaryExpression(BinaryOpCode opCode, ExpressionBase leftExpression, ExpressionBase rightExpression) {

            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression));

            if (rightExpression == null)
                throw new ArgumentNullException(nameof(rightExpression));

            this.opCode = opCode;
            this.leftExpression = leftExpression;
            this.rightExpression = rightExpression;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public override string ToString() {

            string symbol;
            switch (opCode) {
                case BinaryOpCode.Add:
                    symbol = "+";
                    break;

                case BinaryOpCode.Sub:
                    symbol = "-";
                    break;
                
                case BinaryOpCode.Mul:
                    symbol = "*";
                    break;
                
                case BinaryOpCode.Div:
                    symbol = "/";
                    break;

                case BinaryOpCode.Mod:
                    symbol = "%";
                    break;
                
                default:
                    throw new Exception("OpCode no reconocido.");
            }
            return String.Format("binary expression '{0}'", symbol);
        }

        public BinaryOpCode OpCode {
            get {
                return opCode;
            }
        }

        public ExpressionBase LeftExpression {
            get {
                return leftExpression;
            }
        }

        public ExpressionBase RightExpression {
            get {
                return rightExpression;
            }
        }
    }
}
