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

    /// <summary>
    /// Clase que representa una expressio binaria.
    /// </summary>
    /// 
    public sealed class BinaryExpression : ExpressionBase {

        private readonly BinaryOpCode opCode;
        private readonly ExpressionBase leftExpression;
        private readonly ExpressionBase rightExpression;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="opCode">Codi d'operacio.</param>
        /// <param name="leftExpression">Expressio de la branca esquerra.</param>
        /// <param name="rightExpression">Expressio de la branca dreta.</param>
        /// 
        public BinaryExpression(BinaryOpCode opCode, ExpressionBase leftExpression, ExpressionBase rightExpression) {

            if (leftExpression == null)
                throw new ArgumentNullException(nameof(leftExpression));

            if (rightExpression == null)
                throw new ArgumentNullException(nameof(rightExpression));

            this.opCode = opCode;
            this.leftExpression = leftExpression;
            this.rightExpression = rightExpression;
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
        /// Obte el codi d'operacio.
        /// </summary>
        /// 
        public BinaryOpCode OpCode => opCode;

        /// <summary>
        /// Obte l'expressio de la branca esquerra.
        /// </summary>
        /// 
        public ExpressionBase LeftExpression => leftExpression;

        /// <summary>
        /// Obte l'expressio de la branca dreta.
        /// </summary>
        /// 
        public ExpressionBase RightExpression => rightExpression;
    }
}
