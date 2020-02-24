namespace MicroCompiler.CodeModel.Expressions {

    using System;

    public sealed class CastExpression: Expression {

        private readonly TypeIdentifier typeId;
        private readonly Expression expression;

        public CastExpression(TypeIdentifier typeId, Expression expression) {

            this.typeId = typeId;
            this.expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public TypeIdentifier TypeId => typeId;

        public Expression Expression => expression;
    }
}
