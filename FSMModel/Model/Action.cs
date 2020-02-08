namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;
    using System.Collections.Generic;

    public sealed class Action : IVisitable {

        private List<Activity> activityList;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// 
        public Action() {
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        /// <summary>
        /// Afegeix una comanda.
        /// </summary>
        /// <param name="activity">La activitat.</param>
        /// 
        public void AddActivity(Activity activity) {

            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            if (activityList == null)
                activityList = new List<Activity>();

            activityList.Add(activity);
        }

        /// <summary>
        /// Enumera les comandes.
        /// </summary>
        /// 
        public IEnumerable<Activity> Activities {
            get {
                return activityList;
            }
        }
    }
}
