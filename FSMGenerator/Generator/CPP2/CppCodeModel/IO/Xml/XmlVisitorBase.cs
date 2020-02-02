namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.IO.Xml {

    using System;
    using System.Globalization;
    using System.Xml;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Expressions;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2.CppCodeModel.Statements;

    internal abstract class XmlVisitorBase: DefaultVisitor {

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
                wr.WriteAttributeString(name, Convert((bool) value));
            else
                wr.WriteElementString(name, String.Format(ci, "{0}", value));
        }

        protected void Attribute(string name, string value) {

            wr.WriteAttributeString(name, value);
        }

        protected void Attribute(string name, ConditionPosition value) {

            wr.WriteAttributeString(name, Convert(value));
        }

        protected void Attribute(string name, UnaryOpCode value) {

            wr.WriteAttributeString(name, Convert(value));
        }

        protected void Attribute(string name, BinaryOpCode value) {

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

        private static string Convert(UnaryOpCode opCode) {

            switch (opCode) {
                case UnaryOpCode.Not:
                    return "not";

                default:
                    throw new Exception("Valor invalido");
            }
        }

        private static string Convert(BinaryOpCode opCode) {

            switch (opCode) {
                case BinaryOpCode.Add:
                    return "add";

                case BinaryOpCode.Sub:
                    return "sub";

                case BinaryOpCode.Mul:
                    return "mult";

                case BinaryOpCode.Div:
                    return "div";

                case BinaryOpCode.Mod:
                    return "mod";

                case BinaryOpCode.And:
                    return "and";
                
                case BinaryOpCode.Or:
                    return "or";

                case BinaryOpCode.Xor:
                    return "xor";

                case BinaryOpCode.LogicalAnd:
                    return "logicalAnd";

                case BinaryOpCode.LogicalOr:
                    return "logicalOr";

                case BinaryOpCode.Equal:
                    return "equal";

                case BinaryOpCode.NoEqual:
                    return "noEqual";

                case BinaryOpCode.Less:
                    return "less";

                case BinaryOpCode.LessOrEqual:
                    return "lessOrEqual";

                case BinaryOpCode.Greather:
                    return "greather";

                case BinaryOpCode.GreatherOrEqual:
                    return "greatherOrEqual";

                default:
                    throw new Exception("Valor invalido");
            }
        }
    }
}
