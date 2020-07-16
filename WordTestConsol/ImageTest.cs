using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = DocumentFormat.OpenXml.Drawing;
using Drawing = DocumentFormat.OpenXml.Wordprocessing.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace WordTestConsol
{
    public class ImageTest
    {

        public void InsertAPicture(string fileName, string document, string bookmark)
        {
            OpenSettings openSettings = new OpenSettings();

            // Add the MarkupCompatibilityProcessSettings
            openSettings.MarkupCompatibilityProcessSettings =
                new MarkupCompatibilityProcessSettings(
                    MarkupCompatibilityProcessMode.ProcessAllParts,
                    FileFormatVersions.Office2007);

            //   string document = @"E:\**项目\**文书\108.docx";
            //   document = @"F:\Project_Code\SVNProject\SDHS_SZCG_ZCCG\SZZF\SZZFWord\xcjckyyszj.docx";
            using (WordprocessingDocument wordprocessingDocument =
                WordprocessingDocument.Open(document, true))
            {
                MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;

                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);

                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    imagePart.FeedData(stream);
                }



                AddImageToBody(wordprocessingDocument, mainPart.GetIdOfPart(imagePart));
            }
        }

        //帮助文档中的源码，直接拷贝出来修改，根据
        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = 1990000L, Cy = 1792000L },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                       "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 1990000L, Cy = 1792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         //  EditId = "50D07946"
                     });

            //书签处写入---这是关键 tp为传递过来的 书签名称，替换成变量即可
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            var bookmarks = from bm in mainPart.Document.Body.Descendants<BookmarkStart>()
                            where bm.Name == "tp"
                            select bm;
            var bookmark = bookmarks.SingleOrDefault();
            if (bookmark != null)
            {
                OpenXmlElement elem = bookmark.NextSibling();
                while (elem != null && !(elem is BookmarkEnd))
                {
                    OpenXmlElement nextElem = elem.NextSibling();
                    elem.Remove();
                    elem = nextElem;
                }
                var parent = bookmark.Parent;   // bookmark's parent element
                Run run = new Run(new RunProperties());
                //该处插入element，注意区分和字符插入的方法Append(dd)
                parent.InsertAfter<Run>(new Run(new Run(element)), bookmark);

            }


        }

    }
}
