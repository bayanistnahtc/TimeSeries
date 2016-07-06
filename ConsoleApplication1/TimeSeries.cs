using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /**
     * by Rustam Tukhvatov
     * 03.07.2016.
     * The Time series class contain methods to procces it
     * summation, subtraction, exponent, median, mode,  abs of elements 
     * Aggregation: sum, mean
     * 
     */
     
       
    class TimeSeries
    {
        /// <summary>
        /// dimension shows if the time series has time (list)
        /// </summary>
        private List <DateTime> time;
        private List<Double> values;
        private List<Double> weights;
        int dimension;

        private TimeSeries()
        {
            this.values = new List<Double>();
            time = new List<DateTime>();
            weights = new List<Double>();
            dimension = 0;
        }

        private TimeSeries(List<Double> values)
        {
            this.values = values;
            time = new List<DateTime>();
            weights = new List<Double>();
            dimension = 1;
        }

        public TimeSeries(List<Double> values, List<DateTime> time, List<Double> weight)
        {
            this.time = time;
            this.values = values;
            this.weights = weight;
            dimension = 2;
        }


        public List<Double> getValues()
        {
            return values;
        }
        /// <summary>
        /// the most frequantly value of the time series
        /// </summary>
        /// <returns></returns>
        public double mode()
        {
            try
            {
                List<double> i = new List<double>(values);
               // double[] i = new double[values.Count];
               // values.CopyTo(i, 0);
                i.Sort();
                double val_mode = i[0], help_val_mode = i[0];
                int old_counter = 0, new_counter = 0;
                int j = 0;
                for (; j <= i.Count - 1; j++)
                    if (i[j] == help_val_mode) new_counter++;
                    else if (new_counter > old_counter)
                    {
                        old_counter = new_counter;
                        new_counter = 1;
                        help_val_mode = i[j];
                        val_mode = i[j - 1];
                    }
                    else if (new_counter == old_counter)
                    {
                        val_mode = double.NaN;
                        help_val_mode = i[j];
                        new_counter = 1;
                    }
                    else
                    {
                        help_val_mode = i[j];
                        new_counter = 1;
                    }
                if (new_counter > old_counter) val_mode = i[j - 1];
                else if (new_counter == old_counter) val_mode = double.NaN;
                return val_mode;
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// The sum of time series values
        /// </summary>
        /// <returns></returns>
        public double sum()
        {
            return values.Sum();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double mean()
        {
            try
            {
                double sum = 0;
                for (int i = 0; i <= values.Count - 1; i++)
                    sum += values[i];
                return sum / values.Count;
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }


        public double min()
        {
            return values.Min();
        }

        public double max()
        {
            return values.Max();
        }

        /// <summary>
        /// return exponent by value
        /// e^{arg}
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public double exponent(double arg)
        {
            return Math.Exp(arg);
        }

        /// <summary>
        /// return exponent by index
        /// u_{i} - i-th value of series
        /// </summary>
        /// <param name="i"></param>
        /// <returns>e^{u_{i}}</returns>
        public double exponentByIndex(int i)
        {
            try
            {
                return Math.Exp(values[i]);
            }catch(IndexOutOfRangeException e)
            {
                System.Console.WriteLine(e.Message);
                // Set IndexOutOfRangeException to the new exception's InnerException.
                throw new System.ArgumentOutOfRangeException("index parameter is out of range.", e);
            }
            
        }
       /// <summary>
       /// |S_{i}|
       /// </summary>
       /// <returns></returns>
        public TimeSeries abs()
        {
            List<Double> absValues = new List<double>();
            foreach(var i in values)
            {
                absValues.Add(Math.Abs(i));
            }
            return new TimeSeries(absValues, this.time, this.weights);
        }

        public int size()
        {
            return values.Count;
        }

        /// <summary>
        /// v_{1} = u_{1}
        /// v_{2} = u_{1} + u_{2}
        /// ...
        /// v_{n} = u_{1} + u_{2}... + u_{n}
        /// </summary>
        /// <returns></returns>
        public TimeSeries cumulativeSum()
        {
            return cumulativeSum(size());
        }
        public TimeSeries cumulativeSum(int index)
        {
            List<Double> cSum = new List<Double>();
            double previuos = values[0];
            cSum.Add(previuos);
            for(var i = 1; i < index; i++)
            {
                previuos = previuos + values[i];
            }
            return new TimeSeries(cSum);
        }
        /// <summary>
        /// Ŷ_{t+1} = alpha*Y_{t} +(1-alpha)* Ŷ_{t}
        /// 
        ///   Ŷ_{t+1} – forecast to the next period t+1;
        ///   Y_{t} –  value to the current period;
        ///   alpha - the smoothing coefficient of series ,  0 < alpha < 1;
        ///   Ŷ_{t} – smoothed value, Ŷ1=Y.
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns>new Time series with smoothed values and same time and weight</returns>
        public TimeSeries exponentialSmoothing(double alpha)
        {
            if(alpha > 0 && alpha < 1)
            {
                List<Double> smoothTS = new List<double>();
                double y = 0;
                smoothTS.Add(values[0]);
                for (var i = 1; i < size(); i++)
                {
                    y = alpha * values[i] + (1 - alpha) * y;
                    smoothTS.Add(y);
                }
                return new TimeSeries(smoothTS, this.time, this.weights);
            }
            return new TimeSeries();
           
        }


        public void print()
        {
            int flag = dimension;

            if (time.Count == 0)
            {
                flag = 1;
            }

            switch (flag)
            {
                case 1:
                    for (var i = 0; i < size(); i++)
                    {
                        Console.WriteLine("[{0}]", values[i]);
                    }
                    break;
                case 2:
                    for (var i = 0; i < size() ; i++)
                    {
                        Console.WriteLine("[{0} : {1}]", time[i], values[i]);
                    }
                    break;

                default:
                    Console.WriteLine("Empty TS");
                    break;

            }

        }

        /// <summary>
        /// S_{i} = u_{i} + v_{i}
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static TimeSeries valuesSummation(TimeSeries u, TimeSeries v)
        {
            if (u.size() == v.size())
            {
                List<Double> a = new List<double>(u.values);
                List<Double> b = new List<double>(v.values);
                List<Double> result = new List<double>(); 
                for(var i = 0; i < u.size(); i++)
                {
                    result.Add(a[i] + b[i]);
                }
                return new TimeSeries(result);
            }
            return new TimeSeries();
        }
        /// <summary>
        /// S_{i} = u_{i} - v_{i}
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static TimeSeries valuesSubstraction(TimeSeries u, TimeSeries v)
        {
            if (u.size() == v.size())
            {
                List<Double> a = new List<double>(u.values);
                List<Double> b = new List<double>(v.values);
                List<Double> result = new List<double>();
                for (var i = 0; i < u.size(); i++)
                {
                    result.Add(a[i] - b[i]);
                }
                return new TimeSeries(result);
            }
            return new TimeSeries();
        }
    }


}
