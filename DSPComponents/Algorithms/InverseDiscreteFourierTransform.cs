using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            float Real, Imaginary, Power , Real1, Imaginary1;
            Complex X, Y;
            List<float> Amplitudes = InputFreqDomainSignal.FrequenciesAmplitudes;
            List<float> PhaseShifts = InputFreqDomainSignal.FrequenciesPhaseShifts;
            List<float> Samples = new List<float> ();
            for (int k = 0; k < Amplitudes.Count; k++)
            {
                Complex Result = new Complex(0.0 , 0.0);
                for (int n = 0; n < Amplitudes.Count; n++) //2*PI*J*k*n/N
                {
                    Power = ((((float)Math.PI) * 2 * n * k) / Amplitudes.Count());
                    Real1 = (float)Math.Cos(PhaseShifts[n]) * Amplitudes[n];
                    Imaginary1 = (float)Math.Sin(PhaseShifts[n]) * Amplitudes[n];
                    X = new Complex(Real1, Imaginary1);
                    Real = (float)Math.Cos(Power);
                    Imaginary = (float)Math.Sin(Power);
                    Y = new Complex(Real, Imaginary);
                    Complex mult = Complex.Multiply(X, Y);
                    Result = Complex.Add(Result,mult);
                }
                float Sample = (float)  Math.Round(Result.Real)/Amplitudes.Count;
                Samples.Add(Sample);
            }
            OutputTimeDomainSignal = new Signal(Samples, false);
            
            
        }
    }
}
