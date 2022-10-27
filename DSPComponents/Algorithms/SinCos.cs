using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos: Algorithm
    {
        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }
        public override void Run()
        {
            List<float> Output = new List<float>();
            if (type.Equals("cos"))
            {
                for (int i = 0; i < (int)SamplingFrequency; i++)
                {
                    float FF = (float) AnalogFrequency / SamplingFrequency;
                    double value = Math.Cos(2 * Math.PI * FF * i + PhaseShift);
                    float element =  A * Convert.ToSingle(value);
                    Output.Add(element);
                }
            }
            else
            {
                for (int i = 0; i < (int)SamplingFrequency; i++)
                {
                    float FF = (float)AnalogFrequency / SamplingFrequency;
                    double value = Math.Sin(2 * Math.PI * FF * i + PhaseShift);
                    float element = A * Convert.ToSingle(value);
                    Output.Add(element);
                }
            }
            samples = Output;
        }
    }
}
