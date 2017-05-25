namespace MikroPicDesigns.FSMCompiler.v1.Generator {

    using MikroPicDesigns.FSMCompiler.v1.Model;

    public interface IGenerator {

        void Generate(Machine machine);
    }
}
