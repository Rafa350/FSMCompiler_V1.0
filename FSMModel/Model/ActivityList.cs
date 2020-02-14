namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System.Collections.Generic;

    public sealed class ActivityList : List<Activity> {

        public ActivityList() {

        }

        public ActivityList(IEnumerable<Activity> activities) :
            base(activities) {

        }
    }
}
