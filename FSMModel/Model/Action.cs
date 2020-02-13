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
        /// Constructor.
        /// </summary>
        /// <param name="activities">Les activitats a afeigit.</param>
        /// 
        public Action(IEnumerable<Activity> activities) {

            if (activities == null)
                throw new ArgumentNullException(nameof(activities));

            activityList = new List<Activity>(activities);
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="activities">Les activitats a afeigir.</param>
        /// 
        public Action(params Activity[] activities) :
            this((IEnumerable<Activity>) activities) {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="activities">Les activitats a afeigir.</param>
        /// 
        public Action(List<Activity> activities) {

            if (activities == null)
                throw new ArgumentNullException(nameof(activities));

            activityList = activities;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

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
        /// Indica si te activitats.
        /// </summary>
        /// 
        public bool HasActivities => activityList != null;

        /// <summary>
        /// Enumera les comandes.
        /// </summary>
        /// 
        public IEnumerable<Activity> Activities => activityList;
    }
}
