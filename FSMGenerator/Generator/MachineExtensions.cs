namespace MikroPicDesigns.FSMCompiler.v1.Generator {

    using System.Collections.Generic;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Activities;

    public static class MachineExtensions {

        public static IEnumerable<string> GetStateNames(this Machine machine) {

            List<string> names = new List<string>();

            foreach (var state in machine.States)
                names.Add(state.Name);

            return names;
        }

        public static IEnumerable<string> GetTransitionNames(this Machine machine) {

            List<string> names = new List<string>();

            foreach (var state in machine.States) {
                foreach (string name in state.GetTransitionNames())
                    if (!names.Contains(name))
                        names.Add(name);
            }

            return names;
        }

        public static IEnumerable<string> GetActivityNames(this Machine machine) {

            List<string> names = new List<string>();

            void PopulateList(Action action) {

                foreach (var activity in action.Activities) {
                    if (activity is RunActivity callActivity) {
                        string name = callActivity.ProcessName;
                        if (!names.Contains(name)) {
                            names.Add(name);
                        }
                    }
                }
            }

            foreach (var state in machine.States) {
                if (state.HasTransitions) {
                    foreach (var transition in state.Transitions)
                        if (transition.Action != null)
                            PopulateList(transition.Action);
                }
                if (state.EnterAction != null)
                    PopulateList(state.EnterAction);

                if (state.ExitAction != null)
                    PopulateList(state.ExitAction);
            }

            return names;
        }
    }
}
