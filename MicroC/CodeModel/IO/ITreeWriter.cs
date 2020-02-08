namespace MicroCompiler.CodeModel.IO {

    using System.IO;

    public interface IUnitWriter {

        void Write(Stream stream, UnitDeclaration tree);
    }
}
