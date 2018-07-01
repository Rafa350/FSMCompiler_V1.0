namespace MikroPicDesigns.FSMCompiler.v1.Generator {

    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public sealed class GeneratorParameters {

        private readonly Dictionary<string, string> items = new Dictionary<string, string>();

        public void Add(string text) {

            if (String.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            string[] s = text.Split(new char[] { '=' });
            Add(s[0], s[1]);
        }

        public void Add(string name, string value) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            items.Add(name, value);
        }

        public void Populate(object dataObject) {

            if (dataObject == null)
                throw new ArgumentNullException("dataObject");

            Type dataObjectType = dataObject.GetType();
            foreach (KeyValuePair<string, string> kv in items) {
                PropertyInfo propInfo = dataObjectType.GetProperty(kv.Key, BindingFlags.Instance | BindingFlags.Public);
                if (propInfo == null)
                    throw new InvalidOperationException(
                        String.Format("No es posible asignar el parametro '{0}'.", kv.Key));
                object value = Convert.ChangeType(kv.Value, propInfo.PropertyType);
                propInfo.SetValue(dataObject, value, null);
            }
        }

        public IEnumerable<string> Names {
            get {
                return items.Keys;
            }
        }

        public string this[string name] {
            get {
                return items[name];
            }
        }
    }
}
