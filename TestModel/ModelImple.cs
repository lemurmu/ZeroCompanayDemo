using ITestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModel
{
    public class ModelImple : MarshalByRefObject, IModel
    {
        public string Hello()
        {
            return "what the fuck are you doing?" + GetPeople.Name;
        }

        public void SetName(string name)
        {
            GetPeople.Name = name;
        }

        public People GetPeople { set; get; } = new People { Name = "Tom" };
    }
}
