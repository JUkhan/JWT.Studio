using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwtex.DummyData
{
    public class DDManager
    {
        private Dictionary<string, List<string>> dataDic = new Dictionary<string, List<string>>();
        private Random rand = new Random();
        public DDManager()
        {
            SetHuman();
            SetCountry();
            SetAnimal();
        }

        public async Task<string> GetDataAsync(DDObject obj)
        {
            return await Task.FromResult<string>(GetData(obj));
        }
        public  string GetData(DDObject obj)
        {
            StringBuilder stringBuilder = new StringBuilder();
           
            stringBuilder.Append("[");
            for (int i = 0; i < obj.limit; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append(',');
                }
                bool isFirst = true;
                stringBuilder.Append("{");
                foreach (var item in obj.columns)
                {
                    if (!isFirst)
                    {
                        stringBuilder.Append(',');
                    }
                    switch (item.type.ToLower())
                    {
                        case "bool":
                            stringBuilder.AppendFormat("\"{0}\":{1}", item.name, GetBool());
                            break;
                        case "guid":
                            stringBuilder.AppendFormat("\"{0}\":\"{1}\"", item.name, GetGuid());
                            break;
                        case "country":
                            stringBuilder.AppendFormat("\"{0}\":\"{1}\"", item.name, GetDataFromDic(item.type, i));
                            break;
                        case "human":
                            stringBuilder.AppendFormat("\"{0}\":\"{1}\"", item.name, GetDataFromDic(item.type, i));
                            break;
                        case "animal":
                            stringBuilder.AppendFormat("\"{0}\":\"{1}\"", item.name, GetDataFromDic(item.type, i));
                            break;
                        case "int":
                            stringBuilder.AppendFormat("\"{0}\":{1}", item.name, GetRandomNumber(item.min, item.max));
                            break;
                        case "decimal":
                        case "float":
                        case "double":
                            stringBuilder.AppendFormat("\"{0}\":{1}.{2}", item.name, GetRandomNumber(item.min, item.max), Get4Digit());
                            break;
                        case "date":
                            stringBuilder.AppendFormat("\"{0}\":\"{1}\"", item.name, GetDate(item));
                            break;
                        case "datetime":
                            stringBuilder.AppendFormat("\"{0}\":\"{1}\"", item.name, GetDateTime(item));
                            break;
                        default:
                            stringBuilder.AppendFormat("\"{0}\":\"{1}-{2}\"", item.name, item.type ?? item.name, i + 1);
                            break;
                    }
                    isFirst = false;
                }
                stringBuilder.Append("}");
               
            }
            stringBuilder.Append("]");

            return   stringBuilder.ToString();
        }

        private string GetBool()
        {
            var list = new List<string> { "true", "false" };
            return list[GetRandomNumber(0, 2)];
        }

        private string GetDataFromDic(string key, int index){
            if(index>=dataDic[key].Count){
                return key+"-"+index;
            }
            return dataDic[key][index];
        }
        private void SetHuman()
        {
            dataDic["human"] = new List<string> { 
                "ABDUL JALIL", "ABDUL KABIR", "ABDUL KARIM", "ABDUL KHALIQ", "ABDUL LATEEF","ABDUL MAAJID",
                 "ABDUL MATEEN", "ABDUL MUBDI", "ABDUL MUEED", "ABDUL MUGHNI", "ABDUL MUHSI","ABDUL MUHSIN",
                 "ABDUL MUNTAQIM", "ABDUL MUMIN", "ABDUL MUJIB", "ABDUL MUQTADIR", "ABDUL MUZANNI","ABDUL  NOOR",
                 "ABDUL-HADI","ABDUL-HALEEM"
            };
        }
        private void SetCountry()
        {
            dataDic["country"] = new List<string> { 
                "Afghanistan", "Albania", "Algeria", "Armenia", "Australia","Austria",
                "Bahrain", "Bangladesh", "Barbados", "Japan", "Bhutan","Bulgaria",
                  "Cambodia", "Cameroon", "Canada", "Belgium", "Cyprus","Denmark",
                   "Hong Kong", "Iceland", "India", "Iran", "Iraq","Ireland"
            };
        }
        private void SetAnimal()
        {
            dataDic["animal"] = new List<string> { 
                "Lion", "Tiger", "Cat", "Monkey", "Mongoose","Mouse",
                "Owl", "Otter", "Kangaroo", "Koala", "Komodo dragon","Leopard",
                  "Lobster", "Lyrebird", "Salamander", "Salmon", "Sea lion","Seal",
                   "Shark", "Snake","Crocodile"
            };
        }
        private string GetDate(DDColumn col)
        {
            DateTime date = DateTime.Now;
            if (col.min < 1970)
            {
                col.min = date.Year-5;
            }
            if (col.max < 1970)
            {
                col.max = date.Year;
            }

            return string.Format("{0:0#}/{1:0#}/{2}", GetRandomNumber(1, 12), GetRandomNumber(1, 30), GetRandomNumber(col.min, col.max));
        }

        private string GetDateTime(DDColumn col)
        {
            List<string> am=new List<string>(){"AM", "PM"};
            return string.Format("{0} {1:0#}:{2:0#}:{3:0#} {4}", GetDate(col), GetRandomNumber(1,12), GetRandomNumber(1,59), GetRandomNumber(1,59), am[GetRandomNumber(0,2)]);
        }
        private string Get4Digit()
        {
            return string.Format("{0}{1}{2}{3}", GetRandomNumber(0,9),
                GetRandomNumber(0, 9), GetRandomNumber(0, 9), GetRandomNumber(0, 9)
                );
        }
        private int GetRandomNumber(int min, int max)
        {
            return rand.Next(min, max);
        }
        private string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
