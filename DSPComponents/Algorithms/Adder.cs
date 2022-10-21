using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> Output = new List<float>();
            for (int i = 0; i < InputSignals.Count; i++)
            {
                for (int j = 0; j < InputSignals[i].Samples.Count; j++)
                {
                    if (!(Output.Count > j))
                        Output.Add(0.0f);

                        Output[j] += InputSignals[i].Samples[j];
                }
            }
            OutputSignal = new Signal (Output , false);
{}
        }
    }
}