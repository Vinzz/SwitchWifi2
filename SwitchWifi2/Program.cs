using SeleniumTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchWifi2
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new LiveboxAutomate().SwitchWifi();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.ToString());
                Console.WriteLine("press any key");
                Console.ReadKey();
            }
        }
    }
}
