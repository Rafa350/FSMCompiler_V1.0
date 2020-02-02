namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.IO {

    using System.IO;
    
    public interface IUnitWriter {

        void Write(Stream stream, UnitDeclaration tree);
    }
}
