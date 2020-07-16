using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringNet
{
    public class UserInfoService : IUserInfoService
    {

        public string UserName { get; set; }
        public PerSon person { get; set; }
        public string MsgShow()
        {
            return "hello：" + UserName + "，年龄：" + person.Age;
        }
    }
}
