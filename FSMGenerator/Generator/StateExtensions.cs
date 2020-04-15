namespace MikroPicDesigns.FSMCompiler.v1.Generator {

    using System;
    using System.Collections.Generic;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    [Obsolete]
    public static class StateExtensions {

        public static IEnumerable<string> GetTransitionNames(this State state) {

            List<string> names = new List<string>();

            if (state.HasTransitions)
                foreach (Transition transition in state.Transitions) {
                    string name = transition.TransitionEvent.GetFullName();
                    if (!names.Contains(name))
                        names.Add(name);
                }

            return names;
        }
    }
}
