using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Class1
    {
        static void Main(string[] args)
        {
            var rate = ConfigurationManager.AppSettings["Rate"];
        }
    }
}
