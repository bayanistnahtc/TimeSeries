using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Write
    {
        public static void toTXT(double[,] data)
        {
            try
            {
            //location is somethig like 
            //C:\Projects\ConsoleApplication1\ConsoleApplication1\bin\Debug
                using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"Output.txt"))
                {
                    for (var i = 0; i < data.Length / 2; i++)
                    {
                        for (var j = 0; j < data.Rank; j++)
                        {
                            file.Write("{0}\t", data[i, j]);
                        }
                        file.WriteLine();
                    }
                }
            }catch(Exception e)
            {
                Console.WriteLine("exception{0}", e);
            }
            
        }
    }
}
