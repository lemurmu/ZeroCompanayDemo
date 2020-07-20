using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace LD.Platform.Function
{
    #region 根据文件路径获取图标

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };

    public class SHHelper
    {
        internal class Win32
        {
            public const uint SHGFI_ICON = 0x100;
            public const uint SHGFI_LARGEICON = 0x0;
            public const uint SHGFI_SMALLICON = 0x1;
            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        }

        /// <summary>
        /// 根据文件路径获取小图标
        /// </summary>
        /// <param name="fileName">文件路径(例如：F:\,F:\Images,F:\Images\Bg.jpg)</param>
        /// <returns>Icon图标</returns>
        internal static Icon GetSmallIcon(string fileName)
        {
            IntPtr hImgSmall;
            SHFILEINFO shinfo = new SHFILEINFO();

            hImgSmall = Win32.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);

            return System.Drawing.Icon.FromHandle(shinfo.hIcon);
        }

        /// <summary>
        /// 根据文件路径获取大图标
        /// </summary>
        /// <param name="fileName">文件路径(例如：F:\,F:\Images,F:\Images\Bg.jpg)</param>
        /// <returns>Icon图标</returns>
        internal static Icon GetLargeIcon(string fileName)
        {
            IntPtr hImgLarge;  // list   
            SHFILEINFO shinfo = new SHFILEINFO();

            hImgLarge = Win32.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON);
            return System.Drawing.Icon.FromHandle(shinfo.hIcon);
        }

        /// <summary>
        /// 从文件扩展名得到文件关联图标
        /// </summary>
        /// <param name="fileName">文件名或文件扩展名</param>
        /// <param name="smallIcon">是否是获取小图标，否则是大图标</param>
        /// <returns>Icon图标</returns>
        internal static Icon GetFileIcon(string fileName, bool smallIcon)
        {
            SHFILEINFO fi = new SHFILEINFO();
            Icon ic = null;

            int iTotal = (int)Win32.SHGetFileInfo(fileName, 100, ref fi, 0, (uint)(smallIcon ? 273 : 272));
            if (iTotal > 0)
            {
                ic = Icon.FromHandle(fi.hIcon);
            }
            return ic;
        }
    }

    #endregion
}
