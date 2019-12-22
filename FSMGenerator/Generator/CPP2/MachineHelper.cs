namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System.Collections.Generic;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    public static class MachineHelper {

        public static IEnumerable<string> GetTransitionNames(this Machine machine) {

            List<string> names = new List<string>();
            
            foreach (State state in machine.States) {
                foreach(string name in state.GetTransitionNames()) {
                    if (!names.Contains(name))
                        names.Add(name);
                }
            }

            return names;
        }
    }
}
