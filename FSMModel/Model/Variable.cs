using System;

namespace MikroPicDesigns.FSMCompiler.v1.Model {

    public sealed class Variable: IVisitable {

        private readonly string _name;
        private readonly string _type;
        private readonly string _value;

        public Variable(string name, string type, string value = null) {

            _name = name ?? throw new ArgumentNullException(nameof(name));
            _type = type ?? throw new AccessViolationException(nameof(type));
            _value = value;
        }

        public void AcceptVisitor(IVisitor visitor) {

        }

        public string Name =>
            _name;

        public string Type =>
            _type;

        public string Value =>
            _value;
    }
}
