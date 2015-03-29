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

        static  void GetData()
        {
            DDObject obj = new DDObject();
            obj.limit = 3;
            obj.columns = new List<DDColumn>
            {
                new DDColumn{ name="id", type="int", min=1, max=20},
                 new DDColumn{ name="name", type="animal" , array=true, limit=5, min=1, max=100},
                 
            };
            DDManager dd = new DDManager();
            string data=  dd.GetData(obj);
            Console.WriteLine(data);
           
        }
    }
}
