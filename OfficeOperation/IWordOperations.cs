using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeOperation
{
    public interface IWordOperations
    {
        #region 文档操作
        /// <summary>
        /// 打开Word文档
        /// </summary>
        /// <param name="editable">是否可编辑</param>
        /// <param name="autoSave">是否自动存储</param>
        void open(bool editable, bool autoSave);
        /// <summary>
        /// 以模板的方式打开
        /// </summary>
        void openAsTemplate(Stream _straem);
        /// <summary>
        /// 以模板的方式打开
        /// </summary>
        void openAsTemplate();
        /// <summary>
        /// 以模板的方式打开
        /// </summary>
        /// <param name="editable">是否可编辑</param>
        /// <param name="autoSave">是否自动存储</param>
        void openAsTemplate(Stream _straem, bool editable, bool autoSave);
        /// <summary>
        /// 以模板的方式打开
        /// </summary>
        /// <param name="editable">是否可编辑</param>
        /// <param name="autoSave">是否自动存储</param>
        void openAsTemplate(bool editable, bool autoSave);

        /// <summary>
        /// 保存文件
        /// </summary>
        void saveFile();

        /// <summary>
        /// 以指定路径保存文件，并以相同设置打开文档
        /// </summary>
        /// <param name="path">路径</param>
        void saveFile(string path);

        /// <summary>
        /// 保存到新文件，并将新文件作为编辑对象
        /// </summary>
        /// <param name="filePath">路径</param>
        void saveAsNewFile(string filePath);

        /// <summary>
        /// 关闭当前文档
        /// </summary>
        void closeFile();
        #endregion

        #region 表
        /// <summary>
        /// 在书签处插入表,并指定表的表头与列宽
        /// </summary>
        /// <param name="columnCaptions">表头</param>
        /// <param name="columWidth">列宽</param>
        /// <param name="bookmaskName">书签名</param>
       void insertTableWithCaptions(List<string> columnCaptions, List<string> columWidth, string bookmaskName, ParagraphProperties _paragraphPropertiesCenter = null);
        /// <summary>
        /// 在书签前插入表,并指定表的表头
        /// </summary>
        /// <param name="columnCaptions">表头</param>
        /// <param name="bookmaskName">书签名</param>
        void insertTableWithCaptions(List<string> columnCaptions, string bookmaskName);

      
        /// <summary>
        /// 在指定段落插入表
        /// </summary>
        /// <param name="columnCaptions">表头</param>
        /// <param name="paragraphIndex">第几段，以1开始的序列</param>
        void insertTableWithCaptions(List<string> columnCaptions, Int32 paragraphIndex);

        /// <summary>
        /// 在指定段落插入表,并指定表的表头与列宽
        /// </summary>
        /// <param name="columnCaptions">表头</param>
        /// <param name="columWidth">列宽</param>
        /// <param name="paragraphIndex">第几段，以1开始的序列</param>
        void insertTableWithCaptions(List<string> columnCaptions, List<string> columWidth, Int32 paragraphIndex);
        /// <summary>
        /// 在文档尾添加表,并指定表的表头
        /// </summary>
        /// <param name="columnCaptions">表头内容</param>
        void addTableWithCaptionsInEnd(List<string> columnCaptions);

        /// <summary>
        /// 在文档尾添加表,并指定表的表头与列宽
        /// </summary>
        /// <param name="columnCaptions">表头</param>
        /// <param name="columWidth">列宽</param>
        void addTableWithCaptionsInEnd(List<string> columnCaptions, List<string> columWidth);



        /// <summary>
        /// 添加数据到指定表,
        /// </summary>
        /// <param name="rowsData"></param>
        /// <param name="bookmask">书签前的第一个表格</param>
        void addRowsToTableBeforeBookMark(List<List<string>> rowsData, string bookmarkName);

        /// <summary>
        /// 指定表的表格，替换其内容
        /// </summary>
        /// <param name="rowIndex">第几行，以1开始的序列</param>
        /// <param name="columnIndex">第几列，以1开始的序列</param>
        /// <param name="cellText">表格内容</param>
        /// <param name="bookmaskName">表所在的书签名</param>
        /// <param name="CanAddCell">若指定表格在表的行列之外，是否添加行列补全</param>
        /// <returns></returns>
        bool replaceTableCell(int rowIndex, int columnIndex, string cellText, string bookmaskName, bool CanAddCell = true, BookmarkStart bookmarkStart = null);

        /// <summary>
        /// 指定表的某行，替换其内容
        /// </summary>
        /// <param name="rowIndex">第几行，以1开始的序列</param>
        /// <param name="startColum">第几列，以1开始的序列</param>
        /// <param name="dataLst">数据</param>
        /// <param name="bookmaskName">表所在的书签名</param>
        /// <param name="CanAddCell">若指定表格在表的行列之外，是否添加行列补全</param>
        /// <returns></returns>
        bool replaceTableRow(int rowIndex, int startColum, List<string> dataLst, string bookmaskName, bool CanAddCell = true);

        /// <summary>
        /// 指定表的某列，替换其内容
        /// </summary>
        /// <param name="startRow">第几行，以1开始的序列</param>
        /// <param name="columIndex">第几列，以1开始的序列</param>
        /// <param name="dataLst">数据</param>
        /// <param name="bookmaskName">表所在的书签名</param>
        /// <param name="CanAddCell">若指定表格在表的行列之外，是否添加行列补全</param>
        /// <returns></returns>
        bool replaceTableColumn(int startRow, int columIndex, List<string> dataLst, string bookmaskName, bool CanAddCell = true);

        /// <summary>
        /// 获取表格某行的内容
        /// </summary>
        /// <param name="rowIndex">第几行，以1开始的序列</param>
        /// <param name="bookmaskName">表所在的书签名</param>
        /// <returns></returns>
        List<string> getTableRowValue(int rowIndex, string bookmaskName, BookmarkStart bookmarkStart = null);

        /// <summary>
        /// 获取表格某列的内容
        /// </summary>
        /// <param name="rowIndex">第几列，以1开始的序列</param>
        /// <param name="bookmaskName">表所在的书签名</param>
        /// <returns></returns>
        List<string> getTableColumnValue(int columnIndex, string bookmaskName, BookmarkStart bookmarkStart = null);

        /// <summary>
        /// 添加行内容到指定列表末尾
        /// </summary>
        /// <param name="rowsData">行内容</param>
        /// <param name="tableIndex">第几个列表，从1开始的序列</param>
        void addRowsToTable(List<List<string>> rowsData, Int32 tableIndex);

        /// <summary>
        /// 设置表格的对齐方式
        /// </summary>
        /// <param name="a">对齐方式</param>
        /// <param name="tableIndex">第几个列表，从1开始的序列</param>
        void setTableAlignment(AlignmentValue a, Int32 tableIndex);


        /// <summary>
        /// 清除表除第一行的所有内容
        /// </summary>
        /// <param name="tableIndex">第几个列表，从1开始的序列</param>
        void clearTableExceptFirstRow(Int32 tableIndex);
#endregion

        #region 图片
        /// <summary>
        /// 替换图片
        /// </summary>
        /// <param name="picturePath">图片路径</param>
        /// <param name="pictureIndex">第几张图片</param>
        void replacePicture(string picturePath, Int32 pictureIndex);

        /// <summary>
        /// 替换图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat">图片类型</param>
        /// <param name="pictureIndex">第几张图片</param>
        void replacePicture(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat imageFormat, Int32 pictureIndex);

        /// <summary>
        /// 添加图片在文档尾
        /// </summary>
        /// <param name="picturePath"></param>
        void addPictureInEnd(string picturePath);

        /// <summary>
        /// 添加图片在文档尾，并指定图片大小
        /// </summary>
        /// <param name="picturePath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void addPictureInEnd(string picturePath, float width, float height);

        /// <summary>
        /// 添加图片在文档尾
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        void addPictureInEnd(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat imageFormat);

        /// <summary>
        /// 添加图片在文档尾，并指定图片大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void addPictureInEnd(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat imageFormat, float width, float height);

        /// <summary>
        /// 在指定段落插入图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        /// <param name="paragraphIndex">第几段，以1开始的序列</param>
        void insertPicture(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat imageFormat, Int32 paragraphIndex);

        /// <summary>
        /// 在书签处插入图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        /// <param name="bookmaskName">书签名</param>
        void insertPicture(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat imageFormat, string bookmaskName);

        /// <summary>
        /// 在指定段落插入图片，并指定图片大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="paragraphIndex">第几段，以1开始的序列</param>
        void insertPicture(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat imageFormat, float width, float height, Int32 paragraphIndex);

        /// <summary>
        /// 在书签处插入图片，并指定图片大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageFormat"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bookmaskName">书签名</param>
        void insertPicture(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat imageFormat, float width, float height, string bookmaskName);

        /// <summary>
        /// 指定图片的布局 
        /// </summary>
        /// <param name="a">布局方式</param>
        /// <param name="pictureIndex">第几段，以1开始的序列</param>
        void setPictureAlignment(AlignmentValue a, Int32 pictureIndex);
        #endregion

        #region 文本

        /// <summary>
        /// 在指定书签前插入文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="bookmaskName"></param>
      void insertTextParagraphBeforeBookmark(string text, string bookmaskName, RunProperties properties = null, ParagraphProperties paragraphProperties = null);
        
            /// <summary>
            /// 查找并替换文本，searchText支持正则表达式 
            /// </summary>
            /// <param name="searchText"></param>
            /// <param name="replacementText"></param>
            void replaceTextInFile(string searchText, string replacementText);

        /// <summary>
        /// 在指定段落中查找并替换文本，searchText支持正则表达式
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="replacementText"></param>
        /// <param name="paragraphIndex">第几个段落，从1开始计数</param>
        void replaceTextInParagrah(string searchText, string replacementText, Int32 paragraphIndex);

        /// <summary>
        /// 在指定表格中查找并替换文本，searchText支持正则表达式
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="replacementText"></param>
        /// <param name="tableIndex">第几个列表，从1开始计数</param>
        void replaceTextInTable(string searchText, string replacementText, Int32 tableIndex);

        /// <summary>
        /// 替换书签处的文本
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="bookmaskName">书签名</param>
        void replaceTextByBookmask(string text, string bookmaskName);

        /// <summary>
        /// 批量替换书签处的文本
        /// </summary>
        /// <param name="textDct">字典的Key为书签名，Value为内容</param>
        void replaceTextByBookmask(Dictionary<string, string> textDct);
        #endregion
        #region 标签
        BookmarkStart GetBookmarkStart(string BookMarkStart);
        
        #endregion
    }
}
