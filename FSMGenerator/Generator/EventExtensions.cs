namespace MikroPicDesigns.FSMCompiler.v1.Generator {

    using System;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    public static class EventExtensions {

        public static string GetFullName(this Event ev) {

            if ((ev.Arguments == null) || String.IsNullOrEmpty(ev.Arguments.Expression))
                return ev.Name;
            else
                return String.Format("{0}({1})", ev.Name, ev.Arguments.Expression);
        }
    }
}
