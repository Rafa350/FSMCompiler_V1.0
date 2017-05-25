namespace MikroPicDesigns.FSMCompiler.v1.Model {

    using System;

    public sealed class Event: IVisitable {

        private readonly string name;

        public Event(string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.name = name;
        }

        public void AcceptVisitor(IVisitor visitor) {

            visitor.Visit(this);
        }

        public string Name {
            get {
                return name;
            }
        }
    }
}
