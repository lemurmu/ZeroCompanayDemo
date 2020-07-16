using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{api-version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="orderCode">订单号</param>
        /// <param name="orderName">订单名称</param>
        /// <returns></returns>
        [HttpGet("{orderCode}/{orderName}")]
        public OrderModel QueryOrder(string orderCode, string orderName)
        {
            OrderModel model = new OrderModel
            {

                OrderCode = orderCode

            };
            return model;
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public OrderModel SubOrder([FromBody] OrderModel model)
        {
            return model;
        }
    }

    public class OrderModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 订单类型
        /// <Remark>
        /// 0 商家入驻
        /// 1 线下交易
        /// </Remark>
        /// </summary>
        public OrderTypeInfo OrderType { get; set; }

    }

    public enum OrderTypeInfo
    {
        /// <summary>
        /// 商家入驻
        /// </summary>
        [System.ComponentModel.Description("商家入驻")]
        StoreEntry = 0,
        /// <summary>
        /// 线下交易
        /// </summary>
        [System.ComponentModel.Description("线下交易")]
        StoreTrade = 1,

    }
}
