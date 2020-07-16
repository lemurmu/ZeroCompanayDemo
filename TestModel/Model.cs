using ITestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestModel
{
    [Serializable]
    public class Model : MarshalByRefObject
    {
        public string Hello()
        {
            return "对象信息" + GetPeople.Name;
        }
        public void SetName(string name)
        {
            people.Name = name;
        }

        private People people = new People { Name = "哈哈" };

        public People GetPeople => people;


    }





}
