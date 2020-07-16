using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.Text.RegularExpressions;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using System.Drawing;
using System.Drawing.Imaging;

namespace OfficeOperation
{

    //
    // Summary:
    //     Defines the JustificationValues enumeration.
    public enum AlignmentValue
    {
        //
        // Summary:
        //     Align Left.
        //     When the item is serialized out as xml, its value is "left".
        Left = 0,
        //
        // Summary:
        //     start.
        //     When the item is serialized out as xml, its value is "start".
        //     This item is only available in Office2010.
        Start = 1,
        //
        // Summary:
        //     Align Center.
        //     When the item is serialized out as xml, its value is "center".
        Center = 2,
        //
        // Summary:
        //     Align Right.
        //     When the item is serialized out as xml, its value is "right".
        Right = 3,
        //
        // Summary:
        //     end.
        //     When the item is serialized out as xml, its value is "end".
        //     This item is only available in Office2010.
        End = 4,
        //
        // Summary:
        //     Justified.
        //     When the item is serialized out as xml, its value is "both".
        Both = 5,
        //
        // Summary:
        //     Medium Kashida Length.
        //     When the item is serialized out as xml, its value is "mediumKashida".
        MediumKashida = 6,
        //
        // Summary:
        //     Distribute All Characters Equally.
        //     When the item is serialized out as xml, its value is "distribute".
        Distribute = 7,
        //
        // Summary:
        //     Align to List Tab.
        //     When the item is serialized out as xml, its value is "numTab".
        NumTab = 8,
        //
        // Summary:
        //     Widest Kashida Length.
        //     When the item is serialized out as xml, its value is "highKashida".
        HighKashida = 9,
        //
        // Summary:
        //     Low Kashida Length.
        //     When the item is serialized out as xml, its value is "lowKashida".
        LowKashida = 10,
        //
        // Summary:
        //     Thai Language Justification.
        //     When the item is serialized out as xml, its value is "thaiDistribute".
        ThaiDistribute = 11
    }

    public class Word : IWordOperations
    {
        private string _filePath = AppDomain.CurrentDomain.BaseDirectory + "temp.doc";
        //> word constructure 
        private WordprocessingDocument _wordDcmt = null;
        private Body _body = null;
        // 
        //private string _docText = null;
        //private bool FStartReplace;

        public Word(string filePath)
        {
            _filePath = filePath;
        }
        public Word() { }
        #region 属性
        public WordprocessingDocument getWordDocument() { return _wordDcmt; }
        #endregion

        #region 私有方法
        private static Drawing getReferenceOfImage(string relationshipId, Int64 cx, Int64 cy)
        {
            //cx = 5274000L; cy = 3274000L;
            // Define the reference of the image.
            Drawing element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = cx, Cy = cy },        // 图片框大小
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
                                             new A.Extents() { Cx = cx, Cy = cy }),
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
                         EditId = "50D07946"
                     });

            return element;
        }

        ~Word()
        {

            if (_wordDcmt != null && _wordDcmt.FileOpenAccess == FileAccess.ReadWrite)
            {

                _wordDcmt.Close();
                _wordDcmt = null;

            }
        }
        #endregion

        #region 接口

        #region 文档操作

        public void createAndOpen()
        {
            _wordDcmt = WordprocessingDocument.Create(_filePath, WordprocessingDocumentType.Document, true);
            _wordDcmt.AddMainDocumentPart();
            _wordDcmt.MainDocumentPart.Document = new Document(new Body());
            _body = _wordDcmt.MainDocumentPart.Document.Body;
        }
        /// <summary>
        /// 打开Word文档
        /// </summary>
        /// <param name="editable">是否可编辑</param>
        /// <param name="autoSave">是否自动存储</param>
        public void open(bool editable, bool autoSave)
        {
            OpenSettings os = new OpenSettings();
            os.AutoSave = autoSave;
            _wordDcmt = WordprocessingDocument.Open(_filePath, editable, os);
            _body = _wordDcmt.MainDocumentPart.Document.Body;
        }

        /// <summary>
        /// 以模板的方式打开
        /// </summary>
        public void openAsTemplate()
        {
            openAsTemplate(true, true);
        }
        /// <summary>
        /// 以模板的方式打开
        /// </summary>
        public void openAsTemplate(Stream _straem)
        {
            openAsTemplate(_straem, true, true);
        }
        /// <summary>
        /// 以模板的方式打开
        /// </summary>
        /// <param name="editable">是否可编辑</param>
        /// <param name="autoSave">是否自动存储</param>
        public void openAsTemplate(Stream _straem, bool editable, bool autoSave)
        {
            OpenSettings os = new OpenSettings();
            os.AutoSave = autoSave;
            using (var wordDoc = WordprocessingDocument.Open(_straem, editable, os))
            {
                _wordDcmt = wordDoc.Clone() as WordprocessingDocument;
                _body = _wordDcmt.MainDocumentPart.Document.Body;
            }
        }

        /// <summary>
        /// 以模板的方式打开
        /// </summary>
        /// <param name="editable">是否可编辑</param>
        /// <param name="autoSave">是否自动存储</param>
        public void openAsTemplate(bool editable, bool autoSave)
        {
            OpenSettings os = new OpenSettings();
            os.AutoSave = autoSave;

            using (var wordDoc = WordprocessingDocument.Open(_filePath, editable, os))
            {
                _wordDcmt = wordDoc.Clone() as WordprocessingDocument;
                _body = _wordDcmt.MainDocumentPart.Document.Body;
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        public void saveFile()
        {
            _wordDcmt.Save();
        }

        /// <summary>
        /// 以指定路径保存文件，并以相同设置打开文档
        /// </summary>
        /// <param name="path">路径</param>
        public void saveFile(string path)
        {
            _wordDcmt.SaveAs(path);
        }

        public bool fileIsOccupied(string filePath)
        {
            FileStream stream = null;

            try
            {
                stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }

        /// <summary>
        /// 保存到新文件，并将新文件作为编辑对象
        /// </summary>
        /// <param name="filePath">路径</param>
        public void saveAsNewFile(string filePath)
        {
            //if (fileIsOccupied(filePath)) return;
            if (_wordDcmt != null)
            {
                var cloneDoc = _wordDcmt.Clone(filePath);
                _wordDcmt.Close();
                cloneDoc.Save();
                _wordDcmt = cloneDoc as WordprocessingDocument;

            }
            //OpenSettings os = new OpenSettings();
            //os.AutoSave = true;
            //using (var wd = WordprocessingDocument.Open(cloneDoc.Package, os)) {
            //    wd.Save();
            //    wd.Close();
            //}
            //MemoryStream m - new MemoryStream()
            //var cloneDoc = _wordDcmt.SaveAs(filePath);
            //using (StreamWriter sw = cloneDoc.)
            //cloneDoc.Save();
            //cloneDoc.Close();
        }

        /// <summary>
        /// 关闭当前文档
        /// </summary>
        public void closeFile()
        {
            if (_wordDcmt != null)
                _wordDcmt.Close();
            _wordDcmt = null;

        }
        #endregion

        #region 表
        /// <summary>
        /// 在文档尾添加表,并指定表的表头
        /// </summary>
        /// <param name="columnCaptions">表头内容</param>
        public void addTableWithCaptionsInEnd(List<string> columnCaptions)
        {
            var tb = createTableWithCaptions(columnCaptions);
            _body.Append(tb);
        }

        private Table createTableWithCaptions(List<string> columnCaptions)
        {
            Table tb = new Table();

            TableProperties props = new TableProperties(
                                        new TableBorders(
                                            new TopBorder
                                            {
                                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                                Size = 8
                                            },
                                            new BottomBorder
                                            {
                                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                                Size = 8
                                            },
                                            new LeftBorder
                                            {
                                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                                Size = 8
                                            },
                                            new RightBorder
                                            {
                                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                                Size = 8
                                            },
                                            new InsideHorizontalBorder
                                            {
                                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                                Size = 8
                                            },
                                            new InsideVerticalBorder
                                            {
                                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                                Size = 8
                                            }));

            tb.AppendChild<TableProperties>(props);
            TableRow tr = new TableRow();
            foreach (var caption in columnCaptions)
            {
                var tc = new TableCell();
                tc.Append(new Paragraph(new Run(new Text(caption))));
                //  columns with automatically sized.
                tc.Append(new TableCellProperties(
                  new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                tc.Append(new TableCellProperties(
                    new TableLayout { Type = TableLayoutValues.Fixed }));
                tr.Append(tc);
            }
            tb.Append(tr);

            return tb;
        }

        private Table createTableWithCaptions(List<string> columnCaptions, List<string> columWidth)
        {
            Table tb = new Table();

            TableProperties props = new TableProperties(
                                        new TableBorders(
                                            new TopBorder
                                            {
                                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                                Size = 8
                                            },
                                            new BottomBorder
                                            {
                                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                                Size = 8
                                            },
                                            new LeftBorder
                                            {
                                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                                Size = 8
                                            },
                                            new RightBorder
                                            {
                                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                                Size = 8
                                            },
                                            new InsideHorizontalBorder
                                            {
                                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                                Size = 8
                                            },
                                            new InsideVerticalBorder
                                            {
                                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                                Size = 8
                                            }));

            tb.AppendChild<TableProperties>(props);

            TableRow tr = new TableRow();
            int i = 0;
            string withV = "20";
            foreach (var caption in columnCaptions)
            {
                var tc = new TableCell();
                tc.Append(new Paragraph(new Run(new Text(caption))));

                if (i < columWidth.Count)
                {
                    withV = columWidth[i];
                }
                tc.Append(new TableCellProperties(
                  new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = withV }));
                ++i;
                tr.Append(tc);
            }
            tb.Append(tr);
            //tb.Append(new TableProperties(
            //    new Justification() { Val = JustificationValues.Center }));

            return tb;
        }

        /// <summary>
        /// 在文档尾添加表,并指定表的表头与列宽
        /// </summary>
        /// <param name="columnCaptions">表头</param>
        /// <param name="columWidth">列宽</param>
        public void addTableWithCaptionsInEnd(List<string> columnCaptions, List<string> columWidth)
        {
            var tb = createTableWithCaptions(columnCaptions, columWidth);
            _body.Append(tb);
        }

        public void mergeTableCell(Int32 tableIndex)
        {
            Int32 c = _body.Elements<Table>().Count();
            if (tableIndex > c || --tableIndex < 0)
            {       // 表格指定超出范围
                //string s = "specified wrong tableIndex";
            }

            Table t = _body.Elements<Table>().ElementAt(tableIndex);

        }

        /// <summary>
        /// 在指定段落插入表
        /// </summary>
        /// <param name="columnCaptions">表头</param>
        /// <param name="paragraphIndex">第几段，以1开始的序列</param>
        public void insertTableWithCaptions(List<string> columnCaptions, Int32 paragraphIndex)
        {
            var tb = createTableWithCaptions(columnCaptions);

            _body.InsertAt(tb, paragraphIndex);
        }

        /// <summary>
        /// 在指定段落插入表,并指定表的表头与列宽
        /// </summary>
        /// <param name="columnCaptions">表头</param>
        /// <param name="columWidth">列宽</param>
        /// <param name="paragraphIndex">第几段，以1开始的序列</param>
        public void insertTableWithCaptions(List<string> columnCaptions, List<string> columWidth, Int32 paragraphIndex)
        {
            var tb = createTableWithCaptions(columnCaptions, columWidth);
            _body.InsertAt(tb, paragraphIndex);
        }

        /// <summary>
        /// 在书签前插入表,并指定表的表头
        /// </summary>
        /// <param name="columnCaptions">表头</param>
        /// <param name="bookmaskName">书签名</param>
        public void insertTableWithCaptions(List<string> columnCaptions, string bookmaskName)
        {
            var bookmaskEnu = _body.Descendants<BookmarkStart>();

            foreach (var bookmask in bookmaskEnu)
            {
                if (bookmask.Name == bookmaskName)
                {
                    bookmask.RemoveAllChildren();
                    var tb = createTableWithCaptions(columnCaptions);
                    bookmask.Parent.InsertBeforeSelf<Table>(tb);
                    //bookmask.Parent.InsertAfter<Table>(tb, bookmask);
                    //bookmask.Parent.InsertAfter<Run>(new Run(createPictureDrawing(image, imageFormat, width, height)), bookmask);
                }
            }
        }

        /// <summary>
        /// 在书签处插入表,并指定表的表头与列宽
        /// </summary>
        /// <param name="columnCaptions">表头</param>
        /// <param name="columWidth">列宽（eg: columWidth.Add("2000")）,比例567:1厘米</param>
        /// <param name="bookmaskName">书签名</param>
        public void insertTableWithCaptions(List<string> columnCaptions, List<string> columWidth, string bookmaskName, ParagraphProperties paragraphProperties = null)
        {
            var bookmaskEnu = _body.Descendants<BookmarkStart>();

            foreach (var bookmask in bookmaskEnu)
            {
                if (bookmask.Name == bookmaskName)
                {
                    bookmask.RemoveAllChildren();

                    var tb = createTableWithCaptions(columnCaptions, columWidth);


                    //Paragraph paragraph = new Paragraph(new Run(tb));
                    //if (paragraphProperties != null)
                    //    paragraph.ParagraphProperties = (ParagraphProperties)paragraphProperties.Clone();

                    bookmask.Parent.InsertBeforeSelf<Table>(tb);
                    //bookmask.Parent.InsertBeforeSelf<Paragraph>(paragraph);
                    // bookmask.Parent.InsertAfter<Run>(new Run(createPictureDrawing(image, imageFormat, width, height)), bookmask);
                }
            }
        }
        public bool DeleteBookMarkTable(string bookmaskName)
        {
            BookmarkStart bookmarkStart = GetBookmarkStart(bookmaskName);

            if (bookmarkStart == null) return false;
            var tb = bookmarkStart.Parent.Parent.Parent.Parent;

            tb.RemoveAllChildren();
            //bookmarkStart.RemoveAllChildren();
            //tb.RemoveChild<TableGrid>();
            //tb.RemoveAllChildren<TableCell>();
            //tb.RemoveAllChildren<TableRow>();
            //tb.RemoveAllChildren<TableProperties>();
            //var t= tb.Elements<TableProperties>();
            //Run run = new Run(new Text("test"));
            //Paragraph paragraph = new Paragraph(run);
            //tb.Parent.InsertAfterSelf<Paragraph>(paragraph);

            //tb.RemoveAnnotations(typeof(Table));
            // 

            return true;
        }

        /// <summary>
        /// 指定表的表格，替换其内容
        /// </summary>
        /// <param name="rowIndex">第几行，以1开始的序列</param>
        /// <param name="columnIndex">第几列，以1开始的序列</param>
        /// <param name="cellText">表格内容</param>
        /// <param name="bookmaskName">表所在的书签名</param>
        /// <param name="CanAddCell">若指定表格在表的行列之外，是否添加行列补全</param>
        /// <returns></returns>
        public bool replaceTableCell(int rowIndex, int columnIndex, string cellText, string bookmaskName, bool CanAddCell = true, BookmarkStart bookmarkStart = null)
        {

            if (bookmarkStart == null)
            {
                bookmarkStart = GetBookmarkStart(bookmaskName);
                if (bookmarkStart == null) return false;
            }
            var tb = bookmarkStart.Parent.Parent.Parent.Parent;
            if (!(tb is Table)) return false;

            TableRow row = null;
            if (CanAddCell)
            {
                int count = tb.Elements<TableRow>().Count();
                if (rowIndex > tb.Elements<TableRow>().Count())
                {
                    TableRow addRow = tb.Elements<TableRow>().Last().Clone() as TableRow;
                    var lastRow = tb.Elements<TableRow>().Last();
                    tb.InsertAfter<TableRow>(addRow, lastRow);
                }
            }
            else if (rowIndex > tb.Elements<TableRow>().Count()) return false;
            row = tb.Elements<TableRow>().ElementAt(rowIndex - 1) as TableRow;

            var cells = row.Elements<TableCell>();
            TableCell cell = null;
            if (CanAddCell)
            {
                while (columnIndex > cells.Count())
                {
                    cell = cells.ElementAt(cells.Count() - 1).Clone() as TableCell;
                    row.Append(cell);
                }
            }
            else if (columnIndex > cells.Count()) return false;
            cell = cells.ElementAt(columnIndex - 1);

            Paragraph CellPa = cell.Elements<Paragraph>().First();

            var CellRuns = CellPa.Elements<Run>();

            if (CellRuns.Count() <= 0)
            {
                //CellPa.Remove();
                //CellPa = new Paragraph(new Run(new Text(" ")));
                //cell.RemoveAllChildren();
                CellPa.Append(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(" ")));

            }
            while (CellRuns.Count() > 1)
            {
                CellPa.RemoveChild<Run>(CellRuns.Last<Run>());
            }
            Run tmpRun = CellRuns.First<Run>();
            var tmpText = tmpRun.Elements<DocumentFormat.OpenXml.Wordprocessing.Text>().First();
            tmpText.Text = cellText;
            return true;
        }


        /// <summary>
        /// 指定表的某行，替换其内容
        /// </summary>
        /// <param name="rowIndex">第几行，以1开始的序列</param>
        /// <param name="startColum">第几列，以1开始的序列</param>
        /// <param name="dataLst">数据</param>
        /// <param name="bookmaskName">表所在的书签名</param>
        /// <param name="CanAddCell">若指定表格在表的行列之外，是否添加行列补全</param>
        /// <returns></returns>
        public bool replaceTableRow(int rowIndex, int startColum, List<string> dataLst, string bookmaskName, bool CanAddCell = true)
        {
            if (startColum < 1) return false;
            BookmarkStart bookmarkStart = GetBookmarkStart(bookmaskName);
            for (int i = 0; i < dataLst.Count; i++)
            {
                if (!replaceTableCell(rowIndex, startColum + i, dataLst[i], bookmaskName, CanAddCell, bookmarkStart))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 指定表的某列，替换其内容
        /// </summary>
        /// <param name="startRow">第几行，以1开始的序列</param>
        /// <param name="columIndex">第几列，以1开始的序列</param>
        /// <param name="dataLst">数据</param>
        /// <param name="bookmaskName">表所在的书签名</param>
        /// <param name="CanAddCell">若指定表格在表的行列之外，是否添加行列补全</param>
        /// <returns></returns>
        public bool replaceTableColumn(int startRow, int columIndex, List<string> dataLst, string bookmaskName, bool CanAddCell = true)
        {
            if (startRow < 1) return false;
            BookmarkStart bookmarkStart = GetBookmarkStart(bookmaskName);
            for (int i = 0; i < dataLst.Count; i++)
            {
                if (!replaceTableCell(startRow + i, columIndex, dataLst[i], bookmaskName, CanAddCell, bookmarkStart))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 获取表格某行的内容
        /// </summary>
        /// <param name="rowIndex">第几行，以1开始的序列</param>
        /// <param name="bookmaskName">表所在的书签名</param>
        /// <returns></returns>
        public List<string> getTableRowValue(int rowIndex, string bookmaskName, BookmarkStart bookmarkStart = null)
        {
            if (--rowIndex < 0) return null;     // 参数中的index应皆为1开始的序列

            List<string> ret = new List<string>();
            try
            {
                var bookmaskEnu = _body.Descendants<BookmarkStart>();
                if (bookmarkStart == null)
                {
                    bookmarkStart = GetBookmarkStart(bookmaskName);
                    if (bookmarkStart == null) return null;
                }

                var tb = bookmarkStart.Parent.Parent.Parent.Parent;
                if (!(tb is Table)) return null;
                TableRow row = null;
                row = tb.Elements<TableRow>().ElementAt(rowIndex);
                IEnumerable<TableCell> cells = row.Elements<TableCell>();
                foreach (TableCell cell in cells)
                {
                    ret.Add(cell.InnerText);
                }
                return ret;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取表格某列的内容
        /// </summary>
        /// <param name="rowIndex">第几列，以1开始的序列</param>
        /// <param name="bookmaskName">表所在的书签名</param>
        /// <returns></returns>
        public List<string> getTableColumnValue(int columnIndex, string bookmaskName, BookmarkStart bookmarkStart = null)
        {
            if (--columnIndex < 0) return null;     // 参数中的index应皆为1开始的序列

            List<string> rets = new List<string>();
            try
            {
                var bookmaskEnu = _body.Descendants<BookmarkStart>();
                if (bookmarkStart == null)
                {
                    bookmarkStart = GetBookmarkStart(bookmaskName);
                    if (bookmarkStart == null) return null;
                }

                var tb = bookmarkStart.Parent.Parent.Parent.Parent;
                if (!(tb is Table)) return null;
                TableRow row = null;
                int count = tb.Elements<TableRow>().Count();
                for (int i = 0; i < count; i++)
                {
                    row = tb.Elements<TableRow>().ElementAt(i);
                    TableCell cell = row.Elements<TableCell>().ElementAt(columnIndex);
                    rets.Add(cell.InnerText);
                }

                return rets;
            }
            catch
            {
                rets.Clear();
                return rets;
            }

        }

        /// <summary>
        /// 添加数据到指定列表末尾
        /// </summary>
        /// <param name="rowsData"></param>
        /// <param name="tableIndex">第几个列表，从1开始计数</param>
        public void addRowsToTable(List<List<string>> rowsData, Int32 tableIndex)
        {
            Int32 c = _body.Elements<Table>().Count();
            if (tableIndex > c || --tableIndex < 0)
            {       // 表格指定超出范围
                //string s = "specified wrong tableIndex";
            }

            Table t = _body.Elements<Table>().ElementAt(tableIndex);

            foreach (var row in rowsData)
            {
                TableRow tr = new TableRow();
                foreach (var v in row)
                {
                    tr.Append(new TableCell(new Paragraph(new Run(new Text(v)))));
                }
                t.Append(tr);
            }
        }

        /// <summary>
        /// 添加数据到指定表,
        /// </summary>
        /// <param name="rowsData"></param>
        /// <param name="bookmask">书签前的第一个表格</param>
        public void addRowsToTableBeforeBookMark(List<List<string>> rowsData, string bookmarkName)
        {
            var bookmaskEnu = _body.Descendants<BookmarkStart>();

            BookmarkStart mark = null;
            foreach (var bookmaskStart in bookmaskEnu)
            {
                if (bookmaskStart.Name == bookmarkName)
                {
                    //bookmaskStart.Parent.InsertBefore<Run>(new Run(new Text(text)), bookmaskStart);
                    mark = bookmaskStart;
                    break;
                }
            }

            if (mark == null) return;

            if (mark.Parent is Paragraph &&
                mark.Parent.ElementsBefore().Last() is Table)
            {
                Table table = mark.Parent.ElementsBefore().Last() as Table;

                foreach (var row in rowsData)
                {
                    TableRow tr = new TableRow();
                    foreach (var v in row)
                    {
                        tr.Append(new TableCell(new Paragraph(new Run(new Text(v)))));
                    }
                    table.Append(tr);
                }
            }
        }

        /// <summary>
        /// 设置表格的对齐方式
        /// </summary>
        /// <param name="a">对齐方式</param>
        /// <param name="tableIndex">第几个列表，从1开始的序列</param>
        public void setTableAlignment(AlignmentValue a, Int32 tableIndex)
        {
            Int32 c = _body.Elements<Table>().Count();
            if (tableIndex > c || --tableIndex < 0)
            {       // 表格指定超出范围
                //string s = "specified wrong tableIndex";
            }

            Table t = _body.Elements<Table>().ElementAt(tableIndex);
            TableProperties tpr;
            if (t.Elements<TableProperties>().Count() != 0)
                tpr = t.Elements<TableProperties>().First();
            else
                tpr = t.PrependChild(new TableProperties());
            JustificationValues j = (JustificationValues)Enum.ToObject(typeof(JustificationValues), a);
            tpr.AppendChild(new Justification() { Val = j });
        }

        /// <summary>
        /// 清除列表中除了第一行的所有内容
        /// </summary>
        /// <param name="tableIndex"></param>
        public void clearTableExceptFirstRow(Int32 tableIndex)
        {
            Int32 c = _body.Elements<Table>().Count();
            if (tableIndex > c || --tableIndex < 0)
            {       // 表格指定超出范围
                //string s = "specified wrong tableIndex";
            }

            Table t = _body.Elements<Table>().ElementAt(tableIndex);

            for (int trc = t.Descendants<TableRow>().Count(); trc > 1; --trc)
            {
                t.Elements<TableRow>().Last().Remove();
            }
        }
        #endregion 

        #region 图片
        /// <summary>
        /// 替换图片，pictureIndex指定第几张图片
        /// </summary>
        /// <param name="pictureIndex"></param>
        /// <param name="picturePath"></param>
        public void replacePicture(string picturePath, Int32 pictureIndex)
        {
            var picEnu = _body.Descendants<Drawing>();
            Int32 count = picEnu.Count();
            if (pictureIndex > count || --pictureIndex < 0)
            {
                // 指定图片的序号超出范围
            }

            A.Blip blipElement = picEnu.ElementAt(pictureIndex).Descendants<A.Blip>().First();
            string imageId;
            if (blipElement != null)
                imageId = blipElement.Embed.Value;
            else
                return;

            ImagePart imagePart = (ImagePart)_wordDcmt.MainDocumentPart.GetPartById(imageId);
            using (FileStream stream = new FileStream(picturePath, FileMode.Open))
            {
                imagePart.FeedData(stream);
            }
        }

        /// <summary>
        /// 替换图片，pictureIndex指定替换图片的序号
        /// </summary>
        /// <param name="pictureIndex"></param>
        /// <param name="image"></param>
        /// <param name="imageFormat">图片类型</param>
        public void replacePicture(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat imageFormat, Int32 pictureIndex)
        {
            var picEnu = _body.Descendants<Drawing>().ToArray();
            Int32 count = picEnu.Count();
            if (pictureIndex > count || --pictureIndex < 0)
            {
                // 指定图片的序号超出范围
            }

            Drawing drawing = picEnu[pictureIndex];

            //得到图像的内嵌ID（外嵌没做处理）
            var inline = drawing.Inline;
            var extent = inline.Extent;
            var pic = inline.Graphic.GraphicData.GetFirstChild<DocumentFormat.OpenXml.Drawing.Pictures.Picture>();
            var imageId = pic.BlipFill.Blip.Embed.Value;
            Console.WriteLine(imageId);
            ////得到图像流
            //var part = _wordDcmt.MainDocumentPar.GetPartById(embed);
            //var stream = part.GetStream();

            //var im = picEnu.ElementAt(pictureIndex);
           
            //A.Blip blipElement = picEnu.ElementAt(pictureIndex).Descendants<A.Blip>().First();
            //string imageId;
            //if (blipElement != null)
            //    imageId = blipElement.Embed.Value;
            //else
            //    return;

            using (var ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);
                ms.Position = 0;
                ImagePart imagePart = (ImagePart)_wordDcmt.MainDocumentPart.GetPartById(imageId);
               // imagePart = _wordDcmt.MainDocumentPart.AddImagePart(ImagePartType.Png);

                imagePart.FeedData(ms);
            }
        }

        /// <summary>
        /// 添加图片在文档尾
        /// </summary>
        /// <param name="picturePath"></param>
        public void addPictureInEnd(string picturePath)
        {
            addPictureInEnd(picturePath, 14.64F, 7.32F);
        }

        /// <summary>
        /// 添加图片在文档尾，并指定图片大小
        /// </summary>
        /// <param name="picturePath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void addPictureInEnd(string picturePath, float width, float height)
        {
            int i = picturePath.LastIndexOf('.');
            string suffix = picturePath.Substring(++i);

            if (suffix == "jpg")
            {
                suffix = "jpeg";
            }

            ImagePart imagePart;
            ImagePartType imagePartType;
            if (Enum.TryParse<ImagePartType>(suffix, true, out imagePartType))
            {
                imagePart = _wordDcmt.MainDocumentPart.AddImagePart(imagePartType);
            }
            else
            {
                return;
            }

            using (FileStream stream = new FileStream(picturePath, FileMode.Open))
            {
                imagePart.FeedData(stream);
            }

            Int64 cx = (Int64)(width * 360000);
            Int64 cy = (Int64)(height * 360000);
            var element = getReferenceOfImage(_wordDcmt.MainDocumentPart.GetIdOfPart(imagePart), cx, cy);
            // Append the reference to body, the element should be in a Run.
            _wordDcmt.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
        }

        private Paragraph createPictureParagraph(Image image, ImageFormat imageFormat, float width, float height)
        {

            // Append the reference to body, the element should be in a Run.
            return new Paragraph(new Run(createPictureDrawing(image, imageFormat, width, height)));
        }

        private Drawing createPictureDrawing(Image image, ImageFormat imageFormat, float width, float height)
        {
            ImagePart imagePart;
            ImagePartType imagePartType;
            if (Enum.TryParse<ImagePartType>(imageFormat.ToString(), true, out imagePartType))
            {
                imagePart = _wordDcmt.MainDocumentPart.AddImagePart(imagePartType);
            }
            else
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);
                ms.Position = 0;
                imagePart.FeedData(ms);
            }

            Int64 cx = (Int64)(width * 360000);
            Int64 cy = (Int64)(height * 360000);
            var element = getReferenceOfImage(_wordDcmt.MainDocumentPart.GetIdOfPart(imagePart), cx, cy);

            return element;
        }

        /// <summary>
        /// 添加图片在文档尾
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        public void addPictureInEnd(Image image, ImageFormat imageFormat)
        {
            addPictureInEnd(image, imageFormat, 14.64F, 7.32F);
        }

        /// <summary>
        /// 添加图片在文档尾，并指定图片大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void addPictureInEnd(Image image, ImageFormat imageFormat, float width, float height)
        {
            var p = createPictureParagraph(image, imageFormat, width, height);
            _wordDcmt.MainDocumentPart.Document.Body.AppendChild(p);
        }

        /// <summary>
        /// 在指定段落插入图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        /// <param name="paragraphIndex">第几段，以1开始的序列</param>
        public void insertPicture(Image image, ImageFormat imageFormat, Int32 paragraphIndex)
        {
            insertPicture(image, imageFormat, 14.64F, 14.64F, paragraphIndex);
        }

        /// <summary>
        /// 在书签处插入图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        /// <param name="bookmaskName">书签名</param>
        public void insertPicture(Image image, ImageFormat imageFormat, string bookmaskName)
        {
            insertPicture(image, imageFormat, 14.64F, 14.64F, bookmaskName);
        }

        /// <summary>
        /// 在指定段落插入图片，并指定图片大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="paragraphIndex">第几段，以1开始的序列</param>
        public void insertPicture(Image image, ImageFormat imageFormat, float width, float height, Int32 paragraphIndex)
        {
            var p = createPictureParagraph(image, imageFormat, width, height);
            _body.InsertAt(p, paragraphIndex);
        }

        /// <summary>
        /// 在书签处插入图片，并指定图片大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bookmaskName">书签名</param>
        public void insertPicture(Image image, ImageFormat imageFormat, float width, float height, string bookmaskName)
        {
            var bookmaskEnu = _body.Descendants<BookmarkStart>();

            foreach (var bookmask in bookmaskEnu)
            {
                if (bookmask.Name == bookmaskName)
                {
                    bookmask.RemoveAllChildren();
                    bookmask.Parent.InsertAfter<Run>(new Run(createPictureDrawing(image, imageFormat, width, height)), bookmask);
                }
            }
        }

        /// <summary>
        /// 指定图片的布局 
        /// </summary>
        /// <param name="a">布局方式</param>
        /// <param name="pictureIndex">第几段，以1开始的序列</param>
        public void setPictureAlignment(AlignmentValue a, Int32 pictureIndex)
        {
            var picEnu = _body.Descendants<Drawing>();
            Int32 count = picEnu.Count();
            if (pictureIndex > count || --pictureIndex < 0)
            {
                // 指定图片的序号超出范围
            }

            var picElem = picEnu.ElementAt(pictureIndex);
            var p = picElem.Parent.Parent as Paragraph;

            ParagraphProperties pPrp;
            if (p.Elements<ParagraphProperties>().Count() != 0)
                pPrp = p.Elements<ParagraphProperties>().First();
            else
                pPrp = p.PrependChild(new ParagraphProperties());
            JustificationValues j = (JustificationValues)Enum.ToObject(typeof(JustificationValues), a);
            pPrp.AppendChild(new Justification() { Val = j });
        }
        #endregion

        #region 文本
        /// <summary>
        /// 查找并替换文本，searchText支持正则表达式 
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="replacementText"></param>
        public void replaceTextInFile(string searchText, string replacementText)
        {
            //string docText = null;
            //using (StreamReader sr = new StreamReader(_wordDcmt.MainDocumentPart.GetStream())) {
            //    docText = sr.ReadToEnd();
            //}

            //Regex regexText = new Regex(searchText);
            //docText = regexText.Replace(docText, replacementText);

            //using (StreamWriter sw = new StreamWriter(_wordDcmt.MainDocumentPart.GetStream(FileMode.Create))) {
            //    sw.Write(docText);
            //}

            Regex regexText = new Regex(searchText);
            foreach (var text in _body.Descendants<Text>())
            {
                text.Text = regexText.Replace(text.Text, replacementText);
                //if (text.Text.Contains(searchText)) {
                //    text.Text = text.Text.Replace(searchText, replacementText);
                //}
            }
        }

        /// <summary>
        /// 在指定段落中查找并替换文本，searchText支持正则表达式
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="replacementText"></param>
        /// <param name="paragraphIndex">第几个段落，从1开始计数</param>
        public void replaceTextInParagrah(string searchText, string replacementText, Int32 paragraphIndex)
        {
            Int32 c = _body.Elements<Paragraph>().Count();
            if (paragraphIndex > c || --paragraphIndex < 0)
            {     // 段落指定超出范围
                //string s = "specified wrong paragraphIndex";
            }

            Paragraph p = _body.Elements<Paragraph>().ElementAt(paragraphIndex);
            Regex regexText = new Regex(searchText);

            foreach (var text in p.Descendants<Text>())
            {
                text.Text = regexText.Replace(text.Text, replacementText);
            }
        }

        /// <summary>
        /// 在指定表格中查找并替换文本，searchText支持正则表达式
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="replacementText"></param>
        /// <param name="tableIndex">第几个列表，从1开始计数</param>
        public void replaceTextInTable(string searchText, string replacementText, Int32 tableIndex)
        {
            Int32 c = _body.Elements<Table>().Count();
            if (tableIndex > c || --tableIndex < 0)
            {     // 表格指定超出范围
                //string s = "specified wrong paragraphIndex";
            }

            Table t = _body.Elements<Table>().ElementAt(tableIndex);
            Regex regexText = new Regex(searchText);

            foreach (var text in t.Descendants<Text>())
            {
                text.Text = regexText.Replace(text.Text, replacementText);
            }
        }

        //public void startReplaceTextInFile()
        //{
        //    using (StreamReader sr = new StreamReader(_wordDcmt.MainDocumentPart.GetStream())) {
        //        _docText = sr.ReadToEnd();
        //        FStartReplace = true;
        //    }
        //}

        //public void replaceTextInFile(string searchText, string replacementText)
        //{
        //    if (!FStartReplace || _docText == null) return;

        //    Regex regexText = new Regex(searchText);
        //    _docText = regexText.Replace(_docText, replacementText);
        //}

        //public void endReplaceTextInFile()
        //{
        //    using (StreamWriter sw = new StreamWriter(_wordDcmt.MainDocumentPart.GetStream(FileMode.Create))) {
        //        sw.Write(_docText);
        //        FStartReplace = false;
        //        _docText = null;
        //    }
        //}

        /// <summary>
        /// 替换书签处的文本
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="bookmaskName">书签名</param>
        public void replaceTextByBookmask(string text, string bookmaskName)
        {
            var bookmaskEnu = _body.Descendants<BookmarkStart>();
            //var pEnu = _body.Elements<Paragraph>();
            //foreach (var p in pEnu) {
            //    var b = p.Descendants<BookmarkStart>(); 
            //    if (b != null) {
            //        p.InsertAfter<Run>(new Run())
            //    }
            //}

            foreach (var bookmaskStart in bookmaskEnu)
            {
                if (bookmaskStart.Name == bookmaskName)
                {
                    var bookmaskNext = bookmaskStart.NextSibling();

                    if (!(bookmaskNext is Run))
                    {
                        bookmaskStart.Parent.InsertAfter<Run>(new Run(new Text(text)), bookmaskStart);
                        continue;
                    }
                    else
                    {
                        Run bookmaskRun = bookmaskNext as Run;
                        BookmarkEnd bookmaskEnd;
                        while (true)
                        {
                            bookmaskEnd = bookmaskStart.NextSibling<BookmarkEnd>();
                            if (bookmaskEnd.Id.Value == bookmaskStart.Id.Value)
                                break;
                        }
                        OpenXmlElement nextElement = bookmaskStart.NextSibling();
                        while (true)
                        {
                            if (nextElement == bookmaskEnd)
                            {
                                var e = nextElement as BookmarkEnd;
                                if (e.Id.Value == bookmaskEnd.Id.Value)
                                    break;
                            }
                            else if (nextElement is Run)
                            {
                                var temp = nextElement;
                                nextElement = nextElement.NextSibling();
                                temp.Remove();
                                continue;
                            }
                            nextElement = nextElement.NextSibling();
                        }
                        bookmaskRun.GetFirstChild<Text>().Text = text;
                        bookmaskStart.RemoveAllChildren();
                        bookmaskStart.Parent.InsertAfter<Run>(bookmaskRun, bookmaskStart);
                        //while (true) {
                        //    var extraRun = bookmaskRun.NextSibling<Run>();
                        //    if (extraRun != null) {
                        //        extraRun.Remove();
                        //    } else {
                        //        break;
                        //    }
                        //}
                        //bookmaskRun.GetFirstChild<Text>().Text = text;
                    }
                }
            }
        }

        /// <summary>
        /// 批量替换书签处的文本
        /// </summary>
        /// <param name="textDct">字典的Key为书签名，Value为内容</param>
        public void replaceTextByBookmask(Dictionary<string, string> textDct)
        {
            foreach (var v in textDct)
            {
                replaceTextByBookmask(v.Value, v.Key);
            }
        }

        /// <summary>
        /// 在指定书签前插入文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="bookmaskName"></param>
        public void insertTextBeforeBookmark(string text, string bookmaskName)
        {
            var bookmaskEnu = _body.Descendants<BookmarkStart>();

            foreach (var bookmarkStart in bookmaskEnu)
            {
                if (bookmarkStart.Name == bookmaskName)
                {
                    bookmarkStart.Parent.InsertBefore<Run>(new Run(new Text(text)), bookmarkStart);
                }
            }
        }

        ///// <summary>
        ///// 在指定书签前插入文本
        ///// </summary>
        ///// <param name="text"></param>
        ///// <param name="bookmaskName"></param>
        //public void insertTextParagraphBeforeBookmark(string text, string bookmaskName)
        //{
        //    var bookmaskEnu = _body.Descendants<BookmarkStart>();

        //    foreach (var bookmaskStart in bookmaskEnu) {
        //        if (bookmaskStart.Name == bookmaskName) {
        //            bookmaskStart.Parent.InsertBeforeSelf<Paragraph>(new Paragraph(new Run(new Text(text))));
        //        }
        //    }
        //}

        /// <summary>
        /// 在指定书签前插入文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="bookmaskName"></param>
        public void insertTextParagraphBeforeBookmark(string text, string bookmaskName, RunProperties properties = null, ParagraphProperties paragraphProperties = null)
        {
            var bookmaskEnu = _body.Descendants<BookmarkStart>();

            foreach (var bookmaskStart in bookmaskEnu)
            {
                if (bookmaskStart.Name == bookmaskName)
                {
                    Run run = new Run(new Text(text));
                    if (properties != null)
                        run.RunProperties = (RunProperties)properties.Clone();
                    Paragraph paragraph = new Paragraph(run);
                    if (paragraphProperties != null)
                        paragraph.ParagraphProperties = (ParagraphProperties)paragraphProperties.Clone();
                    bookmaskStart.Parent.InsertBeforeSelf<Paragraph>(paragraph);
                }
            }
        }
        /// <summary>
        /// 在指定书签前插入文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="bookmaskName"></param>
        public void insertTextParagraphAfterBookmark(string text, string bookmaskName, RunProperties properties = null, ParagraphProperties paragraphProperties = null)
        {
            var bookmaskEnu = _body.Descendants<BookmarkStart>();

            foreach (var bookmaskStart in bookmaskEnu)
            {
                if (bookmaskStart.Name == bookmaskName)
                {
                    Run run = new Run(new Text(text));
                    if (properties != null)
                        run.RunProperties = (RunProperties)properties.Clone();
                    Paragraph paragraph = new Paragraph(run);
                    if (paragraphProperties != null)
                        paragraph.ParagraphProperties = (ParagraphProperties)paragraphProperties.Clone();
                    bookmaskStart.Parent.InsertAfterSelf<Paragraph>(paragraph);
                }
            }
        }
        #endregion

        #region 书签
        public BookmarkStart GetBookmarkStart(string bookmaskName)
        {

            var bookmaskEnu = _body.Descendants<BookmarkStart>();
            BookmarkStart bookmarkStart = null;
            foreach (var bookmask in bookmaskEnu)
            {
                if (bookmask.Name == bookmaskName)
                {
                    bookmarkStart = bookmask;
                }
            }
            return bookmarkStart;
        }


        #endregion
        #endregion
    }
}
