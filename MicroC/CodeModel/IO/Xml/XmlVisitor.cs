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

        public override void Visit(UnitDeclaration obj) {

            StartElement("unitDeclaration");
            base.Visit(obj);
            EndElement();
        }

        public override void Visit(NamespaceDeclaration obj) {

            StartElement("namespaceDeclaration");
            if (!String.IsNullOrEmpty(obj.Name)) {
                Attribute("name", obj.Name);
            }

            base.Visit(obj);
            EndElement();
        }

        public override void Visit(ClassDeclaration obj) {

            StartElement("classDeclaration");
            Attribute("name", obj.Name);
            //Attribute("access", obj.Access.ToString().ToLower());
            //Attribute("implementation", obj.Implementation.ToString().ToLower());
            base.Visit(obj);
            EndElement();
        }

        public override void Visit(MemberFunctionDeclaration obj) {

            StartElement("functionDeclaration");
            Attribute("name", obj.Name);
            Attribute("type", obj.ReturnType.Name);

            if (obj.Arguments != null) {
                StartElement("parameterDeclarations");
                foreach (var argument in obj.Arguments) {
                    argument.AcceptVisitor(this);
                }

                EndElement();
            }

            if (obj.Body != null) {
                obj.Body.AcceptVisitor(this);
            }

            EndElement();
        }

        public override void Visit(ArgumentDeclaration obj) {

            StartElement("parameterDeclaration");
            Attribute("name", obj.Name);
            Attribute("type", obj.ValueType.Name);
            EndElement();
        }

        public override void Visit(MemberVariableDeclaration obj) {

            StartElement("memberVariableDeclaration");
            Attribute("name", obj.Name);
            Attribute("type", obj.ValueType.Name);
            Attribute("access", obj.Access.ToString().ToLower());
            //Attribute("implementation", obj.Implementation.ToString().ToLower());
            //Attribute("isReadonly", obj.IsReadonly);
            EndElement();
        }

        public override void Visit(BlockStatement obj) {

            StartElement("block");
            if (obj.Statements != null) {
                StartElement("statements");
                foreach (var statement in obj.Statements) {
                    statement.AcceptVisitor(this);
                }

                EndElement();
            }
            EndElement();
        }

        public override void Visit(ReturnStatement obj) {

            StartElement("returnStatement");
            if (obj.Expression != null) {
                obj.Expression.AcceptVisitor(this);
            }

            EndElement();
        }

        public override void Visit(AssignStatement obj) {

            StartElement("assignStatement");
            Attribute("identifier", obj.Name);
            obj.Expression.AcceptVisitor(this);
            EndElement();
        }

        public override void Visit(FunctionCallStatement obj) {

            StartElement("callStatement");
            obj.Expression.AcceptVisitor(this);
            EndElement();
        }

        public override void Visit(IfThenElseStatement obj) {

            StartElement("conditionalStatement");
            obj.ConditionExpression.AcceptVisitor(this);
            obj.TrueStmt.AcceptVisitor(this);
            if (obj.FalseStmt != null) {
                obj.FalseStmt.AcceptVisitor(this);
            }

            EndElement();
        }

        public override void Visit(LoopStatement obj) {

            StartElement("loopStatement");
            Attribute("conditionPosition", obj.ConditionPosition);
            obj.ConditionExpression.AcceptVisitor(this);
            if (obj.Body != null) {
                obj.Body.AcceptVisitor(this);
            }

            EndElement();
        }

        public override void Visit(UnaryExpression obj) {

            StartElement("unaryExpression");
            Attribute("operand", obj.OpCode);
            obj.Expression.AcceptVisitor(this);
            EndElement();
        }

        public override void Visit(BinaryExpression obj) {

            StartElement("binaryExpression");
            Attribute("operand", obj.OpCode);
            obj.LeftExpression.AcceptVisitor(this);
            obj.RightExpression.AcceptVisitor(this);
            EndElement();
        }

        public override void Visit(ConditionalExpression obj) {

            StartElement("conditionalExpression");
            obj.ConditionExpression.AcceptVisitor(this);
            obj.TrueExpression.AcceptVisitor(this);
            obj.FalseExpression.AcceptVisitor(this);
            EndElement();
        }

        public override void Visit(IdentifierExpression obj) {

            StartElement("identifier");
            Attribute("name", obj.Name);
            EndElement();
        }

        public override void Visit(LiteralExpression obj) {

            StartElement("literal");
            Attribute("value", obj.Value);
            EndElement();
        }

        public override void Visit(FunctionCallExpression obj) {

            StartElement("callExpression");
            obj.Function.AcceptVisitor(this);
            if (obj.Arguments != null) {
                StartElement("parameters");
                foreach (var argument in obj.Arguments) {
                    argument.AcceptVisitor(this);
                }

                EndElement();
            }
            EndElement();
        }
    }
}
