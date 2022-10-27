using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            int N = InputSignal1.Samples.Count;
            DirectCorrelation dc = new DirectCorrelation();
            dc.InputSignal1 = InputSignal1;
            dc.Run();
            float max_value = dc.OutputNonNormalizedCorrelation.Max();
            int index_max = dc.OutputNonNormalizedCorrelation.IndexOf(max_value);
            float cor_value = 0;
            for (int j = 0; j < N; j++)
            {
                cor_value += InputSignal1.Samples[j] * InputSignal2.Samples[j];
            }
            cor_value = cor_value / N;
            int index_cor_value = dc.OutputNonNormalizedCorrelation.IndexOf(cor_value);
            OutputTimeDelay = (index_max - index_cor_value) * InputSamplingPeriod;
        }
    }
}
