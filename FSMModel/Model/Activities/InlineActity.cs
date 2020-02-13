namespace MikroPicDesigns.FSMCompiler.v1.Model.Activities {

    using System;

    public sealed class InlineActity : Activity {

        private readonly string text;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Text</param>
        /// 
        public InlineActity(string text) {

            if (String.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            this.text = text;
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

        public string Text => text;
    }
}
