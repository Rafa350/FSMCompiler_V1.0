namespace MicroCompiler.CodeModel {

    public interface IClassMember : IVisitable {

        string Name { get; set; }
        AccessMode Access { get; set; }
    }
}
