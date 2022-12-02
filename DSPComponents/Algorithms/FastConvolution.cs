using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int N_Signal = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
        
            for (int i = InputSignal1.Samples.Count; i < N_Signal; i++)
                InputSignal1.Samples.Add(0);
            for (int i = InputSignal2.Samples.Count; i < N_Signal; i++)
                InputSignal2.Samples.Add(0);

            List<Complex> FFT_Input1 = FFT(InputSignal1.Samples);
            List<Complex> FFT_Input2 = FFT(InputSignal2.Samples);
            List<Complex> Multiplication = new List<Complex> ();

            for (int i = 0; i < FFT_Input1.Count; i++)
            {
                Multiplication.Add(Complex.Multiply(FFT_Input1[i], FFT_Input2[i]));
            }

            List<float> Amplitudes = new List<float>();
            List<float> PhaseShifts = new List<float>();
            for (int k = 0; k < Multiplication.Count; k++)
            {
                var Amplitude = (float)Math.Sqrt(Multiplication[k].Real * Multiplication[k].Real + Multiplication[k].Imaginary * Multiplication[k].Imaginary);
                var PhaseShift = Math.Atan2(Multiplication[k].Imaginary, Multiplication[k].Real); //theta=imaginary/real
                Amplitudes.Add(Amplitude);
                PhaseShifts.Add((float)PhaseShift);
            }

            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            var Frequencies = new List<float> { };
            IDFT.InputFreqDomainSignal = new DSPAlgorithms.DataStructures.Signal(true, Frequencies, Amplitudes, PhaseShifts);
            IDFT.Run();

            OutputConvolvedSignal = new Signal(IDFT.OutputTimeDomainSignal.Samples, false);
        }

        private List<Complex> FFT(List<float> signal)
        {
            List<Complex> harmonics = new List<Complex>();

            for (int k = 0; k < signal.Count; k++)
            {
                float Real = 0, Imaginary = 0, Power;

                for (int n = 0; n < signal.Count; n++) //2*PI*J*k*n/N
                {
                    Power = ((((float)Math.PI) * 2 * n * k) / signal.Count());
                    Real += (float)Math.Cos(Power) * signal[n];
                    Imaginary += (float)Math.Sin(Power) * (-signal[n]);
                }
                harmonics.Add(new Complex(Real, Imaginary));
            }
            return harmonics;
        }
    }
}
