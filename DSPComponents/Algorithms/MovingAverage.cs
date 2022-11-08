using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }

        public override void Run()
        {
            int half_window = (InputWindowSize - 1) / 2;
            OutputAverageSignal = new Signal(new List<float>(), false);
            float result;
            for (int i = half_window; i < InputSignal.Samples.Count() - half_window; i++)
            {
                result = InputSignal.Samples[i];
                for (int j = 1; j <= half_window; j++)
                {
                    result += (InputSignal.Samples[i - j] + InputSignal.Samples[i + j]);
                }
                result /= InputWindowSize;
                OutputAverageSignal.Samples.Add(result);
            }
        }
    }
}
