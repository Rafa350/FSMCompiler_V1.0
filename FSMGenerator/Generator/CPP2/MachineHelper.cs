namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2 {

    using System.Collections.Generic;
    using MikroPicDesigns.FSMCompiler.v1.Model;
    using MikroPicDesigns.FSMCompiler.v1.Model.Commands;

    public static class MachineHelper {

        public static IEnumerable<string> GetTransitionNames(this Machine machine) {

            List<string> names = new List<string>();
            
            foreach (var state in machine.States) {
                foreach(string name in state.GetTransitionNames()) {
                    if (!names.Contains(name))
                        names.Add(name);
                }
            }

            return names;
        }

        public static IEnumerable<string> GetCommandNames(this Machine machine) {

            List<string> names = new List<string>();

            void PopulateList(Action action) {

                foreach (var command in action.Commands) {
                    if (command is MachineCommand machineCommand) {
                        string name = machineCommand.Text;
                        if (!names.Contains(name))
                            names.Add(name);
                    }
                }
            }

            foreach (var state in machine.States) {
                if (state.HasTransitions) {
                    foreach (var transition in state.Transitions) {
                        if (transition.Action != null)
                            PopulateList(transition.Action);
                    }
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
