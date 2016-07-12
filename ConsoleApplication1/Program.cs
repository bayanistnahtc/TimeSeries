using System;
using Accord.Statistics.Analysis;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            Read readFirst = new Read();

            Console.WriteLine("time series:");

            // Reproducing Lindsay Smith's "Tutorial on Principal Component Analysis"
            // using the framework's default method. The tutorial can be found online
            // at http://www.sccg.sk/~haladova/principal_components.pdf

            // Step 1. На вход подаются набор данных
            // ---------------------

            double[,] data = readFirst.fromTXT(@"c:\input.txt");
           // {
           //     { 2.5, 2.4, 2.3 },
           //     { 0.5, 0.7, 0.8 },
           //     { 2.2, 2.9, 2.9 },
           //     { 1.9, 2.2, 2.0 },
           //     { 3.1, 3.0, 2.8 },
           //     { 2.3, 2.7, 2.2 },
           //     { 2.0, 1.6, 1.6 },
           //     { 1.0, 1.1, 1.2 },
           //     { 1.5, 1.6, 1.3 },
           //     { 1.1, 0.9, 1.0 }
           //};
            
            //печатаю данные 
            for (var i = 0; i < data.Length/data.GetLength(1); i++)
            {
                for (var j = 0; j < data.GetLength(1); j++)
                {
                    Console.Write("{0}\t", data[i, j]);
                }
                Console.WriteLine();
            }


            // Step 2. Вычитание среднего значения (иначе стандартизация)
      

            AnalysisMethod method = AnalysisMethod.Center; // AnalysisMethod.Standardize


            // Step 3. Вычисление ковариационной матрицы 
            
            var pca = new PrincipalComponentAnalysis(data, method);

            // Compute it
            pca.Compute();


            // Step 4. Вычисление собственных векторов и собственных значений ковариационной матрицы
           
            // Step 5. Получение нового набора данных
            // ---------------------------------

            Console.WriteLine();
            Console.WriteLine("PCA:");
            //второй аргумент - это размерность преобразованного набора данных 
            double[,] actual = pca.Transform(data, 2);  
            for (var i = 0; i < actual.Length / 2; i++)
            {
                for (var j = 0; j < actual.Rank; j++)
                {
                    Console.Write("{0}\t", actual[i, j]);
                }
                Console.WriteLine();
            }
           
            //записываем новый набор даннх в текстовый файл
            Write.toTXT(actual);
            Console.ReadKey();
        }

    }
}


