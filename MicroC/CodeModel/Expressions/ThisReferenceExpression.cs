namespace MicroCompiler.CodeModel.Expressions {

    using System;
    
    public sealed class ThisReferenceExpression: Expression {

        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }
    }
}
