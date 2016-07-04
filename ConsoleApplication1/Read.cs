using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Read
    {
        private  List<DateTime> time = new List<DateTime>();
        private  List<Double> values = new List<Double>();
        private  List<Double> weights = new List<Double>();

        public void fromCSV(string file)
        {
            String[] strs;

            try
            {
                strs = System.IO.File.ReadAllText(file).Split('\n');
                // Console.WriteLine(strs.Count());
                DateTime date;
                double val;
                double weight;
                foreach (var i in strs)
                {
                    var splitted = i.Split(';');
                    date = DateTime.ParseExact(splitted[0], "dd.MM.yyyy HH:mm", null);
                    val = Convert.ToDouble(splitted[1].Replace('.', ','));
                    if (splitted.Length > 2)
                    {
                        weight = Convert.ToDouble(splitted[2].Replace('.', ','));
                        weights.Add(weight);
                    }

                    time.Add(date);
                    values.Add(val);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("file problem: {0}", e);
            }

        }
        public  List<Double> getValues()
        {
            return values;
        }
        public  List<DateTime> getTime()
        {
            return time;
        }
        public  List<Double> getWeights()
        {
            return weights;
        }

    }
}