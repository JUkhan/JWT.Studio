using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jwtex;
using Jwtex.DummyData;

namespace ConApp
{
    class Program
    {
        static  void Main(string[] args)
        {
           
            GetData();
            Console.WriteLine(DateTime.Now.ToLongTimeString());
            Console.ReadKey();
        }

        static async Task GetData()
        {
            DDObject obj = new DDObject();
            obj.limit = 25;
            obj.columns = new List<DDColumn>
            {
                new DDColumn{ name="id", type="int", min=1, max=20},
                 new DDColumn{ name="name", type="animal"},
                  new DDColumn{ name="product", type="datetime", min=2010, max=2015}
            };
            DDManager dd = new DDManager();
            string data= await dd.GetData(obj);
            Console.WriteLine(data);
           
        }
    }
}
