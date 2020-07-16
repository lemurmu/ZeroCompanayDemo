using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITestModel
{

    public interface IModel
    {
        string Hello();

        void SetName(string name);

        People GetPeople { set; get; }
    }

    [Serializable]
    public class People : MarshalByRefObject
    {
        public string Name { get; set; }
    }
}
