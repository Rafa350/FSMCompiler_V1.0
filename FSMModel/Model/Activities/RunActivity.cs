namespace MikroPicDesigns.FSMCompiler.v1.Model.Activities {

    using System;

    public sealed class RunActivity : Activity {

        private readonly string processName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processName">El nom del process.</param>
        /// 
        public RunActivity(string processName) {

            if (String.IsNullOrEmpty(processName))
                throw new ArgumentNullException(nameof(processName));

            this.processName = processName;
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
        /// Obte el nom del proces.
        /// </summary>
        /// 
        public string ProcessName => processName;
    }
}
