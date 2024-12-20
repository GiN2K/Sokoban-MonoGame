using System;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

namespace Sokoban
{
    public class XSLTProcessor
    {
        public static void GenerateLevelsStatus(string xmlPath, string xsltPath, string outputPath, string completedLevels)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xsltPath);

            XsltArgumentList args = new XsltArgumentList();
            args.AddParam("completedLevels", "", completedLevels);

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<?xml-stylesheet type=\"text/xsl\" href=\"LevelsStatusToHTML.xslt\"?>");

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true
                };

                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                using (XmlReader reader = XmlReader.Create(xmlPath))
                {
                    xslt.Transform(reader, args, xmlWriter);
                }
            }
        }
    }
}