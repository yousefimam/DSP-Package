using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            OutputShiftedSignal = new Signal(new List<float>(), true);
            if (InputSignal.Periodic == true)
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    OutputShiftedSignal.Samples.Add(InputSignal.Samples[i]);
                    OutputShiftedSignal.SamplesIndices.Add(InputSignal.SamplesIndices[i] + ShiftingValue);
                }
                OutputShiftedSignal.Periodic = true;
            }
            else
            {
                for (int i = 0; i < InputSignal.Samples.Count(); i++)
                {
                    OutputShiftedSignal.Samples.Add(InputSignal.Samples[i]);
                    OutputShiftedSignal.SamplesIndices.Add(InputSignal.SamplesIndices[i] - ShiftingValue);
                }
            }
                
          

        }
    }
}
