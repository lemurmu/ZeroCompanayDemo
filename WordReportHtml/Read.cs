using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace WordReportHtml
{
    public class Read
    {
        public static List<string> ReadToHtml(string wordPathStr)
        {
            return ReadToHtml(new FileStream(wordPathStr, FileMode.Open));
        }

        public static List<string> ReadToHtml(Stream wordStream)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(wordStream, false))
            {
                //XmlWriterSettings settings = new XmlWriterSettings() { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Auto,DoNotEscapeUriAttributes=true};
                List<string> paragraphHtmls = new List<string>();

                MainDocumentPart mainPart = doc.MainDocumentPart;
                Body body = doc.MainDocumentPart.Document.Body;

                //段落
                foreach (var paragraph in body.Elements<Paragraph>())
                {
                    StringBuilder paragraphHtml = new StringBuilder();
                    //块
                    foreach (var run in paragraph.ChildElements)
                    {
                        if (run is Run)
                        {
                            foreach (OpenXmlElement openXmlElement in run.Elements())
                            {
                                //软回车
                                if (openXmlElement is Break br)
                                {
                                    paragraphHtmls.Add(paragraphHtml.ToString());
                                    paragraphHtml = new StringBuilder();
                                }
                                //文字块
                                else if (openXmlElement is Text text)
                                {
                                    paragraphHtml.Append(text.Text);
                                }
                                //图像块
                                else if (openXmlElement is Drawing drawing)
                                {
                                    //得到图像的内嵌ID（外嵌没做处理）
                                    var inline = drawing.Inline;
                                    var extent = inline.Extent;
                                    var pic = inline.Graphic.GraphicData.GetFirstChild<DocumentFormat.OpenXml.Drawing.Pictures.Picture>();
                                    var embed = pic.BlipFill.Blip.Embed.Value;

                                    //得到图像流
                                    var part = mainPart.GetPartById(embed);
                                    var stream = part.GetStream();

                                    //流转2进制
                                    byte[] bytes = new byte[stream.Length];
                                    stream.Read(bytes, 0, bytes.Length);

                                    //2进制转base64
                                    string imgHtml = $"<img width='{ImageExtent.EMU_TO_PX((decimal)extent.Cx.Value).ToString("0.")}' height='{ImageExtent.EMU_TO_PX((decimal)extent.Cy.Value).ToString("0.")}' src='data:{part.ContentType};base64," + Convert.ToBase64String(bytes) + "' />";
                                    paragraphHtml.Append(imgHtml);
                                }
                            }
                        }
                        //else if(run is DocumentFormat.OpenXml.Math.OfficeMath math)
                        //{
                        //    var x = new XmlDocument();
                        //    x.LoadXml(math.OuterXml);
                        //    using var ms = ConvertToMatchMl(x, settings);
                        //    paragraphHtml.Append(ConvertToLatex(settings, ms));
                        //}
                    }

                    paragraphHtmls.Add(paragraphHtml.ToString());
                }

                return paragraphHtmls;
            }

        }

        /// <summary>
        /// 合并文档
        /// </summary>
        /// <param name="finalFile"></param>
        /// <param name="files"></param>
        public static void Combine(string finalFile, List<string> files)
        {
            if (files.Count < 2)
            {
                return;
            }
            File.Copy(files[0], finalFile, true);
            using (WordprocessingDocument doc = WordprocessingDocument.Open(finalFile, true))
            {
                Body b = doc.MainDocumentPart.Document.Body;
                for (int i = 1; i < files.Count; i++)
                {
                    using (WordprocessingDocument doc1 = WordprocessingDocument.Open(files[i], true))
                    {
                        foreach (var inst in doc1.MainDocumentPart.Document.Body.Elements())
                        {
                            b.Append(inst.CloneNode(true));
                        }
                    }
                }
            }
        }

        private string ConvertToLatex(XmlWriterSettings settings, Stream ms)
        {
            var latexTransform = new XslCompiledTransform();
            latexTransform.Load(Path.Combine(AppContext.BaseDirectory, "xsltml", "mmltex.xsl"), new XsltSettings(true, true), new XmlUrlResolver());
            var la = new MemoryStream();
            latexTransform.Transform(new XmlTextReader(ms), XmlWriter.Create(la, settings));
            la.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(la, Encoding.UTF8);
            return sr.ReadToEnd();
        }

        private Stream ConvertToMatchMl(XmlDocument xmlDocument, XmlWriterSettings settings)
        {
            var ms = new MemoryStream();
            var xslTransform = new XslCompiledTransform();
            xslTransform.Load(Path.Combine(AppContext.BaseDirectory, "xsltml", "OMML2MML.XSL"));
            xslTransform.Transform(xmlDocument, XmlWriter.Create(ms, settings));
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }


    /// <summary>
    /// 图像长度单位转换
    /// </summary>
    public class ImageExtent
    {
        private const decimal CM_TO_PX = 96M;
        private const decimal INCH_TO_CM = 2.54M;
        /// <summary>
        /// 厘米到EMU（English Metric Unit)
        /// </summary>
        private const decimal CM_TO_EMU = 360000M;

        /// <summary>
        /// EMU（English Metric Unit) 到像素（px）
        /// </summary>
        /// <param name="EMU"></param>
        public static decimal EMU_TO_PX(decimal EMU)
        {
            return EMU / CM_TO_EMU / INCH_TO_CM * CM_TO_PX;
        }
    }
}

