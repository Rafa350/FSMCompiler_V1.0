namespace MicroCompiler.CodeModel {

    using System.Collections.Generic;

    public sealed class ConstructorInitializerList: List<ConstructorInitializer> {

        public ConstructorInitializerList() {
        }

        public ConstructorInitializerList(IEnumerable<ConstructorInitializer> initializers) :
            base(initializers) {
        }

        public ConstructorInitializerList(params ConstructorInitializer[] initializers) :
            base(initializers) {
        }
    }
}
