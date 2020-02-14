﻿namespace MicroCompiler.CodeModel.Statements {

    using System;
    using System.Collections.Generic;

    public sealed class SwitchStatement : Statement {

        private Expression expression;
        private Block defaultBody;
        private List<SwitchCaseStatement> switchCaseList;

        public override void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public void AddSwitchCase(SwitchCaseStatement switchCase) {

            if (switchCase == null)
                throw new ArgumentNullException(nameof(switchCase));

            if (switchCaseList == null)
                switchCaseList = new List<SwitchCaseStatement>();

            switchCaseList.Add(switchCase);
        }

        /// <summary>
        /// Obrte o asigna l'expressio.
        /// </summary>
        /// 
        public Expression Expression {
            get { return expression; }
            set { expression = value; }
        }

        public Block DefaultBody {
            get { return defaultBody; }
            set { defaultBody = value; }
        }

        public IEnumerable<SwitchCaseStatement> SwitchCases => switchCaseList;
    }
}
