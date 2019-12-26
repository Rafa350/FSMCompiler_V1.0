namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System;
    using System.Collections.Generic;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    [Obsolete]
    public static class StateHelper {

        public static IEnumerable<string> GetTransitionNames(this State state) {

            List<string> names = new List<string>();

            foreach (Transition transition in state.Transitions) {
                string name = transition.Name;
                if (!names.Contains(name))
                    names.Add(name);
            }

            return names;
        }
    }
}
