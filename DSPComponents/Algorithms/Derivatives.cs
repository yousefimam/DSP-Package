using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> FirstDerivativeList = new List<float>();
            List<float> SecondDerivativeList = new List<float>();
            float SecondDerv;
            int j;
  
            for (int i = 1; i < InputSignal.Samples.Count(); i++)
                FirstDerivativeList.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]); //Y(n) = x(n)-x(n-1)

            for (j = 0; j < InputSignal.Samples.Count() -1 ; j++)
            {   
                if (j == 0)
                    SecondDerv = InputSignal.Samples[j + 1] - (2 * InputSignal.Samples[j]); //we can't have negative value in the array
                else
                    SecondDerv = InputSignal.Samples[j + 1] - (2 * InputSignal.Samples[j]) + InputSignal.Samples[j - 1]; //Y(n)= x(n+1)-2x(n)+x(n-1)
                SecondDerivativeList.Add(SecondDerv);
            }
            FirstDerivative = new Signal(FirstDerivativeList, false);
            SecondDerivative = new Signal(SecondDerivativeList, false);
        }
    }
}
