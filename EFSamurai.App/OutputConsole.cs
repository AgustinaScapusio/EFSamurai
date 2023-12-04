using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSamurai.App
{
    public static class OutputConsole
    {
        public static void DisplayTitle() { }
        public static void DisplayText() { }
        public static void DisplayStringList(List<string> list) { 
        foreach(var l in list)
            {
                Console.WriteLine(l);
            }
        }
    }
}
