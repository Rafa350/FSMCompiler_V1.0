using System;
using System.Collections.Generic;

namespace MikroPicDesigns.FSMCompiler.v1.Model {

    public sealed class Action : IVisitable {

        private ActivityList _activityList;

        /// <summary>
        /// Constructor de l'objecte.
        /// </summary>
        /// 
        public Action() {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="activities">Les activitats a afeigir.</param>
        /// 
        public Action(ActivityList activities) {

            _activityList = activities ?? throw new ArgumentNullException(nameof(activities));
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="activities">Les activitats a afeigit.</param>
        /// 
        public Action(IEnumerable<Activity> activities) :
            this(new ActivityList(activities)) {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="activities">Les activitats a afeigir.</param>
        /// 
        public Action(params Activity[] activities) :
            this(new ActivityList(activities)) {

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

            if (_activityList == null)
                _activityList = new ActivityList();

            _activityList.Add(activity);
        }

        /// <summary>
        /// Indica si te activitats.
        /// </summary>
        /// 
        public bool HasActivities => 
            _activityList != null;

        /// <summary>
        /// Enumera les comandes.
        /// </summary>
        /// 
        public IEnumerable<Activity> Activities => 
            _activityList;
    }
}
