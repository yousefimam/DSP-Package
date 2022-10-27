using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            float Real, Imaginary,Power;
            List<float> Amplitudes = new List<float>();
            List<float> PhaseShifts = new List<float>();

            List<float> Freqs = new List<float>();
            List<float> Signals = InputTimeDomainSignal.Samples;
            float rue = (float)((2 * Math.PI) / (Signals.Count * (1 / InputSamplingFrequency)));
            for (int k = 0; k < Signals.Count; k++) 
            {
                Real = 0;
                Imaginary = 0;
                for (int n = 0; n < Signals.Count; n++) //2*PI*J*k*n/N
                {
                    Power = ((((float)Math.PI) * 2 * n * k) / Signals.Count());
                    Real += (float)Math.Cos(Power) * Signals[n];
                    Imaginary += (float)Math.Sin(Power) *( - Signals[n]);
                }
                float Amplitude = (float)Math.Sqrt(Real * Real + Imaginary * Imaginary);
                float PhaseShift = (float)Math.Round(Math.Atan2(Imaginary, Real) , 5); //theta=imaginary/real
                float frequ = (float) Math.Round(rue * k , 1);
                Amplitudes.Add(Amplitude);
                PhaseShifts.Add(PhaseShift);
                Freqs.Add(frequ);
            }

            OutputFreqDomainSignal = new Signal(Signals, false);
            OutputFreqDomainSignal.Frequencies = Freqs;
            OutputFreqDomainSignal.FrequenciesAmplitudes = Amplitudes;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = PhaseShifts;
        }
    }
}
