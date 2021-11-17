using Saxon.Api;
using System;
using System.IO;

namespace Visualization
{
    
    public class Visualize
    {
        private const String xrXSLT = "cii-xr.xsl";
        private const String htmlXSLT = "xrechnung-html.xsl";

        private static void Xform(string xmlPathString, string resourcesPathString, string xmlFileName, string xsltFileName, string outputFileName)
        {
            Uri xmlPath = new Uri(xmlPathString);
            Uri resourcesPath = new Uri(resourcesPathString);

            Processor processor = new Processor();

            XdmNode input = processor.NewDocumentBuilder().Build(new Uri(xmlPath, xmlFileName));
            Xslt30Transformer transformer = processor.NewXsltCompiler().Compile(new Uri(resourcesPath, xsltFileName)).Load30();
            transformer.GlobalContextItem = input;

            Serializer serializer = processor.NewSerializer();

            var output = new FileInfo(xmlPathString + outputFileName);
            FileStream outStream = new FileStream(output.ToString(), FileMode.Create, FileAccess.Write);

            serializer.SetOutputStream(outStream);

            transformer.ApplyTemplates(input, serializer);

            outStream.Close();
        }

        public static void TransformToHtml(string xmlPathString, string resourcesPathString, string xmlFileName)
        {
            var htmlFileName = xmlFileName.Remove(xmlFileName.Length - 4) + "-report.html";
            var tempXmlFileName = xmlFileName.Remove(xmlFileName.Length - 4) + "-temp.xml";
            Xform(xmlPathString, resourcesPathString, xmlFileName, xrXSLT, tempXmlFileName);
            Xform(xmlPathString, resourcesPathString, tempXmlFileName, htmlXSLT, htmlFileName);
        }
    }
}
