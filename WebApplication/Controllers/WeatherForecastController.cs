using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 查询天气
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //  [Route("getWeather/[action]")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.Log(LogLevel.Information, "fuck you !man ");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 通过索引查找天气
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [Route("GetWeatherByIndex")]
        public dynamic GetWeatherByName([FromBody] int index)
        {
            if (index >= Summaries.Length)
            {
                return new
                {
                    msg = "索引错误",
                    paramter = nameof(index)
                };

            }
            string weatherName = Summaries[index];

            return new
            {
                weatherName = weatherName,
                msg = "查找成功"
            };
        }

        /// <summary>
        /// 获取天气列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSunmmaries")]
        public ActionResult GetSunmmaries()
        {
            string content = "weather:";
            for (int i = 0; i < Summaries.Length; i++)
            {
                if (i == Summaries.Length - 1)
                {
                    content += Summaries[i];
                }
                else
                {
                    content += Summaries[i] + ",";
                }
            }
            return Content(content);
        }
    }
}
