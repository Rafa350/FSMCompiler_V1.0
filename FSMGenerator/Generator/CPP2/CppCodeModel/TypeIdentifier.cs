namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel {

    using System.Collections.Generic;

    public sealed class TypeIdentifier {

        private static Dictionary<string, TypeIdentifier> cache = new Dictionary<string, TypeIdentifier>();
        private readonly string name;

        private TypeIdentifier(string name) {

            this.name = name;
        }

        public static TypeIdentifier FromName(string name) {

            TypeIdentifier t;

            if (!cache.TryGetValue(name, out t)) {
                t = new TypeIdentifier(name);
                cache.Add(name, t);
            }

            return t;
        }

        /// <summary>
        /// Enumera el nom dels tipus registrats.
        /// </summary>
        /// 
        public static IEnumerable<string> Names {
            get { return cache.Keys; }
        }

        /// <summary>
        /// Enumera els tipus registrats.
        /// </summary>
        /// 
        public static IEnumerable<TypeIdentifier> TypeIdentifiers {
            get { return cache.Values; }
        }

        /// <summary>
        /// Obte el nom.
        /// 
        /// </summary>
        public string Name => name;
    }
}