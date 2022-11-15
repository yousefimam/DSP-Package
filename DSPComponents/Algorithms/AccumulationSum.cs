using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
           OutputSignal = new Signal(new List<float>(), InputSignal.Periodic);
           float sum_of_signals = 0;
           for (int i = 0; i < InputSignal.Samples.Count(); i++)
           {
                   sum_of_signals += InputSignal.Samples[i];
                   OutputSignal.Samples.Add(sum_of_signals);
               
           }
        }
     }
}