﻿namespace MicroCompiler.CodeModel {

    using System;

    /// <summary>
    /// Clase que representa un bloc de codi.
    /// </summary>
    /// 
    public sealed class Block : IVisitable {

        /// <summary>
        /// Constructor per defecte.
        /// </summary>
        /// 
        public Block() {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statements">Llista d'instruccions.</param>
        /// 
        public Block(StatementList statements) {

            Statements = statements;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Obte o asigna la llista d'instruccions.
        /// </summary>
        /// 
        public StatementList Statements { get; set; }
    }
}
