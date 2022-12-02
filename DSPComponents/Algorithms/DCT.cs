using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            OutputSignal = new Signal(new List<float>(), true);
            int N = InputSignal.Samples.Count;
            float first = (float) Math.Sqrt(2.0 / N), second = (float)Math.PI /(4 * N);
            for (int i = 0; i < N; i++)
            {
                float R = 0.0f;
                for (int j = 0; j < N; j++)
                {
                    R += (float) ((first * InputSignal.Samples[j]) * (float) (Math.Cos(second * (2 * j - 1) * (2 * i - 1))));
                }
                OutputSignal.Samples.Add(R);
            } 
        }
    }
}
