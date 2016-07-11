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
using Accord.Statistics.Analysis;
using Accord.Math;

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

            // Step 1. Get some data
            // ---------------------

            double[,] data = readFirst.fromTXT(@"c:\input.txt");
            //{
            //    { 2.5,  2.4 },
            //    { 0.5,  0.7 },
            //    { 2.2,  2.9 },
            //    { 1.9,  2.2 },
            //    { 3.1,  3.0 },
            //    { 2.3,  2.7 },
            //    { 2.0,  1.6 },
            //    { 1.0,  1.1 },
            //    { 1.5,  1.6 },
            //    { 1.1,  0.9 }
            //};

            for (var i = 0; i < data.Length / 2; i++)
            {
                for (var j = 0; j < data.Rank; j++)
                {
                    Console.Write("{0}\t", data[i, j]);
                }
                Console.WriteLine();
            }

            
            // Step 2. Subtract the mean
            // -------------------------
            //   Note: The framework does this automatically. By default, the framework
            //   uses the "Center" method, which only subtracts the mean. However, it is
            //   also possible to remove the mean *and* divide by the standard deviation
            //   (thus performing the correlation method) by specifying "Standardize"
            //   instead of "Center" as the AnalysisMethod.

            AnalysisMethod method = AnalysisMethod.Center; // AnalysisMethod.Standardize


            // Step 3. Compute the covariance matrix
            // -------------------------------------
            //   Note: Accord.NET does not need to compute the covariance
            //   matrix in order to compute PCA. The framework uses the SVD
            //   method which is more numerically stable, but may require
            //   more processing or memory. In order to replicate the tutorial
            //   using covariance matrices, please see the next example below.

            // Create the analysis using the selected method
            var pca = new PrincipalComponentAnalysis(data, method);

            // Compute it
            pca.Compute();


            // Step 4. Compute the eigenvectors and eigenvalues of the covariance matrix
            // -------------------------------------------------------------------------
            //   Note: Since Accord.NET uses the SVD method rather than the Eigendecomposition
            //   method, the Eigenvalues are not directly available. However, it is not the
            //   Eigenvalues themselves which are important, but rather their proportion:

            // Those are the expected eigenvalues, in descending order:
            //    double[] eigenvalues = pca.Eigenvalues;
            // eigenvalues = { 1.28402771, 0.0490833989 };

            // And this will be their proportion:
            //   double[] proportion = eigenvalues.Divide(eigenvalues.Sum());

            // Those are the expected eigenvectors,
            // in descending order of eigenvalues:
            //   double[,] eigenvectors =
            //   {
            //       { -0.677873399, -0.735178656 },
            //       { -0.735178656,  0.677873399 }
            //   };

            // Now, here is the place most users get confused. The fact is that
            // the Eigenvalue decomposition (EVD) is not unique, and both the SVD
            // and EVD routines used by the framework produces results which are
            // numerically different from packages such as STATA or MATLAB, but
            // those are correct.

            // If v is an eigenvector, a multiple of this eigenvector (such as a*v, with
            // a being a scalar) will also be an eigenvector. In the Lindsay case, the
            // framework produces a first eigenvector with inverted signs. This is the same
            // as considering a=-1 and taking a*v. The result is still correct.

            // Retrieve the first expected eigenvector
            //  double[] v = eigenvectors.GetColumn(0);

            // Multiply by a scalar and store it back
            //  eigenvectors.SetColumn(0, v.Multiply(-1));

            // At this point, the eigenvectors should equal the pca.ComponentMatrix,
            // and the proportion vector should equal the pca.ComponentProportions up
            // to the 9 decimal places shown in the tutorial.


            // Step 5. Deriving the new data set
            // ---------------------------------

            Console.WriteLine();
            Console.WriteLine("PCA:");
            double[,] actual = pca.Transform(data);
            for (var i = 0; i < actual.Length/2; i++)
            {
                for (var j = 0; j < actual.Rank; j++)
                {
                    Console.Write("{0}\t", actual[i, j]);
                }
                Console.WriteLine();
            }
            // transformedData shown in pg. 18
            //double[,] expected = new double[,]
            //{
            //    {  0.827970186, -0.175115307 },
            //    { -1.77758033,   0.142857227 },
            //    {  0.992197494,  0.384374989 },
            //    {  0.274210416,  0.130417207 },
            //    {  1.67580142,  -0.209498461 },
            //    {  0.912949103,  0.175282444 },
            //    { -0.099109437, -0.349824698 },
            //    { -1.14457216,   0.046417258 },
            //    { -0.438046137,  0.017764629 },
            //    { -1.22382056,  -0.162675287 },
            //};

            // At this point, the actual and expected matrices
            // should be equal up to 8 decimal places.
            Console.ReadKey();

        }

    }
}


