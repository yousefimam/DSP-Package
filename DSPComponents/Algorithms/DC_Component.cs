using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> Output = new List<float>();
            float MEAN = InputSignal.Samples.Average();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                Output.Add(InputSignal.Samples[i] - MEAN);   
            }
            OutputSignal = new Signal(Output, false);
        }
    }
}
