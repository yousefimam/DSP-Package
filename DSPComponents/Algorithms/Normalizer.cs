using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            List<float> Output = new List<float>();
            float range = InputMaxRange - InputMinRange;
            float maximum = InputSignal.Samples.Max();
            float minimum = InputSignal.Samples.Min();
            float difference = maximum - minimum;

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float x = (float)((InputSignal.Samples[i] - minimum) / difference) * range + InputMinRange;
                Output.Add(x);
            }

            OutputNormalizedSignal = new Signal(Output, false);
        }
    }
}
