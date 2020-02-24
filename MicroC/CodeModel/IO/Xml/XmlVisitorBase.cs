namespace MicroCompiler.CodeModel.IO.Xml {

    using System;
    using System.Globalization;
    using System.Xml;
    using MicroCompiler.CodeModel;
    using MicroCompiler.CodeModel.Expressions;
    using MicroCompiler.CodeModel.Statements;

    internal abstract class XmlVisitorBase : DefaultVisitor {

        private readonly XmlWriter wr;
        private readonly CultureInfo ci;

        public XmlVisitorBase(XmlWriter wr) {

            this.wr = wr;
            this.ci = CultureInfo.GetCultureInfo("en-US");
        }

        protected void StartElement(string name) {

            wr.WriteStartElement(name);
        }

        protected void EndElement() {

            wr.WriteEndElement();
        }

        protected void Attribute(string name, object value) {

            if (value is bool)
                wr.WriteAttributeString(name, Convert((bool)value));

            else
                wr.WriteElementString(name, String.Format(ci, "{0}", value));
        }

        protected void Attribute(string name, string value) {

            wr.WriteAttributeString(name, value);
        }

        protected void Attribute(string name, ConditionPosition value) {

            wr.WriteAttributeString(name, Convert(value));
        }

        protected void Attribute(string name, UnaryOperation value) {

            wr.WriteAttributeString(name, Convert(value));
        }

        protected void Attribute(string name, BinaryOperation value) {

            wr.WriteAttributeString(name, Convert(value));
        }

        private static string Convert(bool value) {

            return value ? "true" : "false";
        }

        private static string Convert(ConditionPosition conditionPosition) {

            if (conditionPosition == ConditionPosition.PostLoop)
                return "postLoop";

            else
                return "preLoop";
        }

        private static string Convert(UnaryOperation opCode) {

            switch (opCode) {
                case UnaryOperation.Not:
                    return "not";

                default:
                    throw new Exception("OpCode invalido.");
            }
        }

        private static string Convert(BinaryOperation opCode) {

            switch (opCode) {
                case BinaryOperation.Add:
                    return "add";

                case BinaryOperation.Sub:
                    return "sub";

                case BinaryOperation.Mul:
                    return "mult";

                case BinaryOperation.Div:
                    return "div";

                case BinaryOperation.Mod:
                    return "mod";

                case BinaryOperation.And:
                    return "and";

                case BinaryOperation.Or:
                    return "or";

                case BinaryOperation.Xor:
                    return "xor";

                case BinaryOperation.LogicalAnd:
                    return "logicalAnd";

                case BinaryOperation.LogicalOr:
                    return "logicalOr";

                case BinaryOperation.Equal:
                    return "equal";

                case BinaryOperation.NoEqual:
                    return "noEqual";

                case BinaryOperation.Less:
                    return "less";

                case BinaryOperation.LessOrEqual:
                    return "lessOrEqual";

                case BinaryOperation.Greather:
                    return "greather";

                case BinaryOperation.GreatherOrEqual:
                    return "greatherOrEqual";

                default:
                    throw new Exception("OpCode invalido.");
            }
        }
    }
}
