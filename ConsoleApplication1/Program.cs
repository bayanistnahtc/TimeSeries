using System;
using ABMath.ModelFramework.Models;
using ABMath.ModelFramework.Data;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.RandomSources;
using MathNet.Numerics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Deedle;
using statistic;
using RDotNet;

using System.Text; 

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            Read readFirst = new Read();
            readFirst.fromCSV(@"C:\info.csv");
            TimeSeries firstSeries = new TimeSeries(readFirst.getValues(), readFirst.getTime(), readFirst.getWeights());
            Read readSecond = new Read();
            readSecond.fromCSV(@"C:\ts2.csv");
            TimeSeries secondSeries = new TimeSeries(readSecond.getValues(), readSecond.getTime(), readSecond.getWeights());

            Console.WriteLine("Here is information about the first time series:");
            firstSeries.print();

            Console.WriteLine("Size: {0}", firstSeries.size());
            Console.WriteLine("Min value: {0}", firstSeries.min());
            Console.WriteLine("Max value: {0}", firstSeries.max());
            Console.WriteLine("Mean value: {0}", firstSeries.mean());
            Console.WriteLine("Mode: {0}", firstSeries.mode());
            Console.WriteLine("Sum of the values: {0}", firstSeries.sum());
            Console.WriteLine("Exponent of the FIRST value (It is possible to "+
                             "choose the index of element): {0}", firstSeries.exponentByIndex(0));

            Console.WriteLine("Exponential smoothing:");
            firstSeries.exponentialSmoothing(0.8).print();
            Console.WriteLine(firstSeries.exponentialSmoothing(0.8).size());


            Console.WriteLine("Here is the two time series summation: ");
            TimeSeries.valuesSummation(firstSeries, secondSeries).print();
            Console.ReadKey();

        }

    }
}


