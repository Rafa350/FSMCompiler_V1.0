namespace MikroPicDesigns.FSMCompiler.v1.Model.Activities {

    using System;

    public sealed class ThrowActivity : Activity {

        private readonly string transitionName;

        /// <summary>
        /// Contructror
        /// </summary>
        /// <param name="transitionName">Nom de la transicio.</param>
        /// 
        public ThrowActivity(string transitionName) {

            if (String.IsNullOrEmpty(transitionName))
                throw new ArgumentNullException(nameof(transitionName));

            this.transitionName = transitionName;
        }

        /// <summary>
        /// Accepota un visitador.
        /// </summary>
        /// <param name="visitor">E visitador.</param>
        /// 
        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte el nom de la transicio.
        /// </summary>
        /// 
        public string Text => transitionName;
    }
}
