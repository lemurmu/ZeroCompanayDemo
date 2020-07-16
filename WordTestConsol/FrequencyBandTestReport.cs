using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOperation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WordTestConsol
{
    public class FrequencyBandTestReport
    {
        Word m_word;
        string m_saveFile;
        bool m_isClose = false;

        #region 初始化操作
        public FrequencyBandTestReport(string _saveFile, string _tempfile = null)
        {
            OpenTempFile(_tempfile, _saveFile);
        }
        public FrequencyBandTestReport() { OpenTempFile(null); }
        ~FrequencyBandTestReport()
        {
            Close();
        }
        #endregion
        #region 文档打开与关闭
        bool m_IsOpenTemplate = false;//是否以模板方式打开文档
        /// <summary>
        /// 直接编辑现有文档
        /// </summary>
        /// <param name="_tempfile"></param>
        public void OpenFile(string _tempfile)
        {
            m_word = new Word(_tempfile);
            m_word.open(true, true);
            m_IsOpenTemplate = false;

        }
        static object lockObj = new object();
        /// <summary>
        /// 以路径文档为模版打开文档
        /// </summary>
        /// <param name="_tempfile"></param>
        /// <param name="_saveFile"></param>
        public void OpenTempFile(string _tempfile, string _saveFile = null)
        {
            if (_tempfile == null)
            {
                Assembly exe = (typeof(FrequencyBandTestReport)).Assembly;
                _tempfile = System.IO.Path.GetDirectoryName(exe.Location) + "\\resource" + "\\test.docx";
            }

            m_word = new Word(_tempfile);
            lock (lockObj)
            {
                m_word.openAsTemplate(true, true);
            }
            m_IsOpenTemplate = true;
            m_saveFile = _saveFile;
        }
        public void Close(string _saveFile = "")
        {
            try
            {
                lock (lockObj)
                {
                    if (m_isClose) return;
                    if (!string.IsNullOrEmpty(_saveFile)) m_saveFile = _saveFile;
                    m_word.saveFile();
                    if (m_IsOpenTemplate) m_word.saveAsNewFile(m_saveFile);
                    m_word.closeFile();

                    m_isClose = true;
                }
            }
            catch
            {

            }
        }
        #endregion


        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="tableDataDict"></param>
        /// <param name="imageDict"></param>
        public void WriteData(Dictionary<WordLabel, List<FrequencyBandTestData>> tableDataDict, Dictionary<WordLabel, List<Image>> imageDict)
        {
            try
            {
                foreach (var item in tableDataDict)
                {
                    List<FrequencyBandTestData> data = item.Value;
                    WordLabel key = item.Key;
                    TableWriteD(key.ToString(), data);
                }

                int startIndex = 2;//从第二张开始替换
                foreach (var image in imageDict)
                {
                    List<Image> images = image.Value;
                    for (int i = 0; i < images.Count; i++)
                    {
                        m_word.replacePicture(images[i], ImageFormat.Png, startIndex);
                        startIndex++;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        bool TableWriteD(string _BookName, List<FrequencyBandTestData> data)
        {
            if (data.Count == 0)
                m_word.DeleteBookMarkTable(_BookName);
            try
            {
                BookmarkStart bookStart = m_word.GetBookmarkStart(_BookName);
                int startIndex = 5;

                foreach (var item in data)
                {
                    List<string> rowsData = new List<string>
                    {
                        item.Freq.ToString(),
                        item.Bandwidth.ToString(),
                        item.Directional.ToString(),
                        item.StartTime.ToString(),
                        item.StopTime.ToString()
                    };
                    for (int i = 1; i <= rowsData.Count; i++)
                    {
                        m_word.replaceTableCell(startIndex, i, rowsData[i - 1].ToString(), _BookName, true, bookStart);
                    }
                    startIndex++;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }



        public void TableWriteD(string _BookName)
        {
            //if (data.Count == 0)
            //    m_word.DeleteBookMarkTable(_BookName);
            try
            {
                BookmarkStart bookStart = m_word.GetBookmarkStart(_BookName);
                int startIndex = 7;
                List<string> data = new List<string>()
                {
                    "25.36",
                    "56.25"
                };

                for (int i = 0; i < data.Count; i++)
                {
                    m_word.replaceTableCell(startIndex, i + 1, data[i].ToString(), _BookName, true, bookStart);
                }
            }
            catch
            {

            }
        }
    }
}
