using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jwt.Core.DuckTyping;
using System.Text.RegularExpressions;
namespace jwtTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //IService ser = DuckTyping.Cast<IService>(new Service());
            //ser.Meth1(23);
            string input = "<div ui-view=\"mac\"></div><div ui-view=\"jac\"></div><div ui-view></div>";

            // Here we call Regex.Match.
            var matches = Regex.Matches(input, "ui-view=\"([a-zA-Z0-9]+)\"",  RegexOptions.IgnoreCase);
          
            // Here we check the Match instance.
            foreach (Match item in matches)
            {
                 //string key = item..Groups[1].Value;
                Console.WriteLine(item.Groups[1].Value);
            }
            //if (match.Success)
            //{
            //    // Finally, we get the Group value and display it.
            //    string key = match.Groups[1].Value;
            //    Console.WriteLine(key);
            //}

            Console.ReadKey();
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public interface IService
    {
        string Prop1 { get; }       

        bool Meth1(int x);
    }
    public class Service
    {
        public string Prop1 { get; set; }
        public bool Meth1(int x)
        {
            return true;
        }
    }
}
