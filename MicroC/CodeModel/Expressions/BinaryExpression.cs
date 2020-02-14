namespace MicroCompiler.CodeModel.Expressions {

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
    public sealed class BinaryExpression : Expression {

        private readonly BinaryOpCode opCode;
        private readonly Expression leftExpression;
        private readonly Expression rightExpression;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="opCode">Codi d'operacio.</param>
        /// <param name="leftExpression">Expressio de la branca esquerra.</param>
        /// <param name="rightExpression">Expressio de la branca dreta.</param>
        /// 
        public BinaryExpression(BinaryOpCode opCode, Expression leftExpression, Expression rightExpression) {

            this.opCode = opCode;
            this.leftExpression = leftExpression ?? throw new ArgumentNullException(nameof(leftExpression));
            this.rightExpression = rightExpression ?? throw new ArgumentNullException(nameof(rightExpression));
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
        public Expression LeftExpression => leftExpression;

        /// <summary>
        /// Obte l'expressio de la branca dreta.
        /// </summary>
        /// 
        public Expression RightExpression => rightExpression;
    }
}
