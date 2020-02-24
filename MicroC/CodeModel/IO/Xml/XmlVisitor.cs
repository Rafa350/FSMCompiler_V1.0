namespace MicroCompiler.CodeModel.IO.Xml {

    using System;
    using System.Xml;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;

    internal sealed class XmlVisitor : XmlVisitorBase {

        public XmlVisitor(XmlWriter wr)
            : base(wr) {
        }

        public override void Visit(UnitDeclaration decl) {

            StartElement("unitDeclaration");
            base.Visit(decl);
            EndElement();
        }

        public override void Visit(NamespaceDeclaration decl) {

            StartElement("namespaceDeclaration");
            if (!String.IsNullOrEmpty(decl.Name)) {
                Attribute("name", decl.Name);
            }

            base.Visit(decl);
            EndElement();
        }

        public override void Visit(ClassDeclaration decl) {

            StartElement("classDeclaration");
            Attribute("name", decl.Name);
            //Attribute("access", obj.Access.ToString().ToLower());
            //Attribute("implementation", obj.Implementation.ToString().ToLower());
            base.Visit(decl);
            EndElement();
        }

        public override void Visit(FunctionDeclaration decl) {

            StartElement("functionDeclaration");
            Attribute("name", decl.Name);
            Attribute("type", decl.ReturnType.Name);

            if (decl.Arguments != null) {
                StartElement("parameterDeclarations");
                foreach (var argument in decl.Arguments) {
                    argument.AcceptVisitor(this);
                }

                EndElement();
            }

            if (decl.Body != null) {
                decl.Body.AcceptVisitor(this);
            }

            EndElement();
        }

        public override void Visit(ArgumentDeclaration decl) {

            StartElement("parameterDeclaration");
            Attribute("name", decl.Name);
            Attribute("type", decl.ValueType.Name);
            EndElement();
        }

        public override void Visit(VariableDeclaration decl) {

            StartElement("memberVariableDeclaration");
            Attribute("name", decl.Name);
            Attribute("type", decl.ValueType.Name);
            Attribute("access", decl.Access.ToString().ToLower());
            //Attribute("implementation", obj.Implementation.ToString().ToLower());
            //Attribute("isReadonly", obj.IsReadonly);
            EndElement();
        }

        public override void Visit(BlockStatement stmt) {

            StartElement("block");
            if (stmt.Statements != null) {
                StartElement("statements");
                foreach (var statement in stmt.Statements) {
                    statement.AcceptVisitor(this);
                }

                EndElement();
            }
            EndElement();
        }

        public override void Visit(ReturnStatement stmt) {

            StartElement("returnStatement");
            if (stmt.ValueExp != null) {
                stmt.ValueExp.AcceptVisitor(this);
            }

            EndElement();
        }

        public override void Visit(AssignStatement stmt) {

            StartElement("assignStatement");
            Attribute("identifier", stmt.Name);
            stmt.ValueExp.AcceptVisitor(this);
            EndElement();
        }

        public override void Visit(InvokeStatement stmt) {

            StartElement("callStatement");
            stmt.InvokeExp.AcceptVisitor(this);
            EndElement();
        }

        public override void Visit(IfThenElseStatement stmt) {

            StartElement("conditionalStatement");
            stmt.ConditionExp.AcceptVisitor(this);
            stmt.TrueStmt.AcceptVisitor(this);
            if (stmt.FalseStmt != null) {
                stmt.FalseStmt.AcceptVisitor(this);
            }

            EndElement();
        }

        public override void Visit(LoopStatement stmt) {

            StartElement("loopStatement");
            Attribute("conditionPosition", stmt.ConditionPos);
            stmt.ConditionExp.AcceptVisitor(this);
            if (stmt.Stmt != null) {
                stmt.Stmt.AcceptVisitor(this);
            }

            EndElement();
        }

        public override void Visit(UnaryExpression exp) {

            StartElement("unaryExpression");
            Attribute("operand", exp.Operation);
            exp.Expression.AcceptVisitor(this);
            EndElement();
        }

        public override void Visit(BinaryExpression exp) {

            StartElement("binaryExpression");
            Attribute("operand", exp.Operation);
            exp.LeftExpression.AcceptVisitor(this);
            exp.RightExpression.AcceptVisitor(this);
            EndElement();
        }

        public override void Visit(ConditionalExpression exp) {

            StartElement("conditionalExpression");
            exp.ConditionExp.AcceptVisitor(this);
            exp.TrueExp.AcceptVisitor(this);
            exp.FalseExp.AcceptVisitor(this);
            EndElement();
        }

        public override void Visit(IdentifierExpression exp) {

            StartElement("identifier");
            Attribute("name", exp.Name);
            EndElement();
        }

        public override void Visit(LiteralExpression exp) {

            StartElement("literal");
            Attribute("value", exp.Value);
            EndElement();
        }

        public override void Visit(InvokeExpression exp) {

            StartElement("callExpression");
            exp.AddressEpr.AcceptVisitor(this);
            if (exp.Arguments != null) {
                StartElement("parameters");
                foreach (var argument in exp.Arguments) {
                    argument.AcceptVisitor(this);
                }

                EndElement();
            }
            EndElement();
        }
    }
}
