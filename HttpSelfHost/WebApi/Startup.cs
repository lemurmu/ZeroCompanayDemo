using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HttpSelfHost.WebApi
{
    class Startup
    {

        private static string _siteDir =
              System.Configuration.ConfigurationManager.AppSettings.Get("SiteDir");
        public void Configuration(IAppBuilder app)
        {
            // web api 接口
            HttpConfiguration config = InitWebApiConfig();
            app.UseWebApi(config);
            //静态文件
            app.Use((context, fun) =>
            {
                return StaticWebFileHandel(context, fun);
            });
        }

        /// <summary>
        /// 路由初始化
        /// </summary>
        public HttpConfiguration InitWebApiConfig()
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );
            // 配置 http 服务的路由
            var cors = new EnableCorsAttribute("*", "*", "*");//跨域允许设置
            config.EnableCors(cors);

            config.Formatters
               .XmlFormatter.SupportedMediaTypes.Clear();
            //默认返回 json
            config.Formatters
                .JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("datatype", "json", "application/json"));
            //返回格式选择
            config.Formatters
                .XmlFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("datatype", "xml", "application/xml"));

            //json 序列化设置
            config.Formatters
                .JsonFormatter.SerializerSettings = new
                Newtonsoft.Json.JsonSerializerSettings()
                {
                    //NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    DateFormatString = "yyyy-MM-dd HH:mm:ss" //设置时间日期格式化
                };
            return config;
        }
        /// <summary>
        /// 客户端请求静态文件处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public Task StaticWebFileHandel(IOwinContext context, Func<Task> next)
        {
            //获取物理文件路径
            var path = GetFilePath(context.Request.Path.Value);
            //验证路径是否存在
            if (!File.Exists(path) && !path.EndsWith("html"))
            {
                path += ".html";
            }
            if (File.Exists(path))
            {
                return SetResponse(context, path);
            }
            //不存在返回下一个请求
            return next();
        }
        public static string GetFilePath(string relPath)
        {
            if (relPath.IndexOf("index") > -1)
            {
                String temp = relPath;
            }
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = relPath.TrimStart('/').Replace('/', '\\');
            if (_siteDir == ".")
            {
                return Path.Combine(basePath, filePath);
            }
            else
            {
                return Path.Combine(
                  AppDomain.CurrentDomain.BaseDirectory
                  , _siteDir == "." ? "" : _siteDir
                  , relPath.Replace('/', '\\')).TrimStart('\\');
            }
        }

        public Task SetResponse(IOwinContext context, string path)
        {
            /*
                .txt text/plain
                RTF文本 .rtf application/rtf
                PDF文档 .pdf application/pdf
                Microsoft Word文件 .word application/msword
                PNG图像 .png image/png
                GIF图形 .gif image/gif
                JPEG图形 .jpeg,.jpg image/jpeg
                au声音文件 .au audio/basic
                MIDI音乐文件 mid,.midi audio/midi,audio/x-midi
                RealAudio音乐文件 .ra, .ram audio/x-pn-realaudio
                MPEG文件 .mpg,.mpeg video/mpeg
                AVI文件 .avi video/x-msvideo
                GZIP文件 .gz application/x-gzip
                TAR文件 .tar application/x-tar
                任意的二进制数据 application/octet-stream
             */
            var perfix = Path.GetExtension(path);
            if (perfix == ".html")
                context.Response.ContentType = "text/html; charset=utf-8";
            else if (perfix == ".txt")
                context.Response.ContentType = "text/plain";
            else if (perfix == ".js")
                context.Response.ContentType = "application/x-javascript";
            else if (perfix == ".css")
                context.Response.ContentType = "text/css";
            else
            {
                if (perfix == ".jpeg" || perfix == ".jpg")
                    context.Response.ContentType = "image/jpeg";
                else if (perfix == ".gif")
                    context.Response.ContentType = "image/gif";
                else if (perfix == ".png")
                    context.Response.ContentType = "image/png";
                else if (perfix == ".svg")
                    context.Response.ContentType = "image/svg+xml";
                else if (perfix == ".woff")
                    context.Response.ContentType = "application/font-woff";
                else if (perfix == ".woff2")
                    context.Response.ContentType = "application/font-woff2";
                else if (perfix == ".ttf")
                    context.Response.ContentType = "application/octet-stream";

                return context.Response.WriteAsync(File.ReadAllBytes(path));
            }
            //truetype
            return context.Response.WriteAsync(File.ReadAllText(path));
        }

    }
}
