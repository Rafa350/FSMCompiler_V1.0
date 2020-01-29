namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    public interface IClassMember: IVisitable {
    
        string Name { get; }
        string TypeName { get;  }
        AccessSpecifier Access { get; }
    }
}
