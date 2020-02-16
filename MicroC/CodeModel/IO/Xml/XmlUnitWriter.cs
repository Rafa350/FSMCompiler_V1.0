namespace MicroCompiler.CodeModel.IO.Xml {

    using System;
    using System.IO;
    using System.Text;
    using System.Xml;

    public sealed class XmlUnitWriter : IUnitWriter {

        public void Write(Stream stream, UnitDeclaration unit) {

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (unit == null)
                throw new ArgumentNullException(nameof(unit));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CloseOutput = false;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.IndentChars = "    ";

            XmlWriter wr = XmlWriter.Create(stream, settings);
            try {
                IVisitor visitor = new XmlVisitor(wr);
                unit.AcceptVisitor(visitor);
            }
            finally {
                wr.Close();
            }
        }
    }
}
