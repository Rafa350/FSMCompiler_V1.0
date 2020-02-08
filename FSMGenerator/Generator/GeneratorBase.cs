namespace MikroPicDesigns.FSMCompiler.v1.Generator {

    using System;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    public abstract class GeneratorBase : IGenerator {

        public virtual void Generate(Machine machine) {

            throw new NotImplementedException();
        }
    }
}
