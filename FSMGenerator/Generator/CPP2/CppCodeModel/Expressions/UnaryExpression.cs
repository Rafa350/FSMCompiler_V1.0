namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Expressions {

    using System;

    public enum UnaryOpCode {

        Minus,
        Not,
        PostInc,
        PostDec,
        PreInc,
        PreDec
    }

    public class UnaryExpression : ExpressionBase {

        private readonly UnaryOpCode opCode;
        private readonly ExpressionBase expression;

        public UnaryExpression(UnaryOpCode opCode, ExpressionBase expression) {

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            this.opCode = opCode;
            this.expression = expression;
        }

        public override void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public UnaryOpCode OpCode {
            get {
                return opCode;
            }
        }

        public ExpressionBase Expression {
            get {
                return expression;
            }
        }
    }
}

