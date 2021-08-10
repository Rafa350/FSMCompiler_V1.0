using System.Collections.Generic;

namespace MikroPicDesigns.FSMCompiler.v1.Model {

    public sealed class ActivityList : List<Activity> {

        public ActivityList() {

        }

        public ActivityList(IEnumerable<Activity> activities) :
            base(activities) {

        }
    }
}
