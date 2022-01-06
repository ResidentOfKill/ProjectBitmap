using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempFolderRemover
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Environment.GetCommandLineArgs().Skip(1).First();
            while(Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path,true);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
