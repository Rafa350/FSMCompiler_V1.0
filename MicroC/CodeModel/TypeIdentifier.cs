namespace MicroCompiler.CodeModel {

    using System.Collections.Generic;

    public sealed class TypeIdentifier {

        private static Dictionary<string, TypeIdentifier> cache;
        private readonly string name;

        private TypeIdentifier(string name) {

            this.name = name;
        }

        public static TypeIdentifier FromName(string name) {

            TypeIdentifier t;

            if ((cache == null) || (!cache.TryGetValue(name, out t))) {

                t = new TypeIdentifier(name);

                if (cache == null)
                    cache = new Dictionary<string, TypeIdentifier>();
                cache.Add(name, t);
            }

            return t;
        }

        /// <summary>
        /// Enumera el nom dels tipus registrats.
        /// </summary>
        /// 
        public static IEnumerable<string> Names {
            get { return cache == null ? null : cache.Keys; }
        }

        /// <summary>
        /// Enumera els tipus registrats.
        /// </summary>
        /// 
        public static IEnumerable<TypeIdentifier> TypeIdentifiers {
            get { return cache == null ? null : cache.Values; }
        }

        /// <summary>
        /// Obte el nom.
        /// 
        /// </summary>
        public string Name => name;
    }
}