using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }


        public List<float> InputSignal2_Samples = new List<float>();
        public List<float> OutputSignal_DFT1 = new List<float>();
        public List<float> OutputSignal_DFT2 = new List<float>();

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            if (InputSignal2 == null) //auto-correlation
            {
                List<float> auto_correlation = new List<float>();
                List<double> signal_samples_1 = new List<double>();
                List<double> signal_samples_1_same = new List<double>();
                List<Complex> Complex_InputSignal1 = new List<Complex>();
                List<Complex> Complex_InputSignal2 = new List<Complex>();
                List<Complex> Multiply_Signals = new List<Complex>();
                List<Complex> result = new List<Complex>();

                double normaliz_sum = 0, samples_sum = 0, samples_sum_same = 0;

                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    signal_samples_1.Add(InputSignal1.Samples[i]);
                    signal_samples_1_same.Add(InputSignal1.Samples[i]);
                }
                for (int i = 0; i < signal_samples_1.Count; i++)
                {
                    samples_sum += signal_samples_1[i] * signal_samples_1[i];
                    samples_sum_same += signal_samples_1_same[i] * signal_samples_1_same[i];
                }
                normaliz_sum = samples_sum * samples_sum_same;
                normaliz_sum = Math.Sqrt(normaliz_sum);
                normaliz_sum /= signal_samples_1.Count;

                Complex_InputSignal1 = FFT(InputSignal1.Samples);
                for (int i = 0; i < Complex_InputSignal1.Count; i++)
                    Complex_InputSignal1[i] = new Complex(Complex_InputSignal1[i].Real,- Complex_InputSignal1[i].Imaginary );

                Complex_InputSignal2 = FFT(InputSignal1.Samples);

                for (int i = 0; i < Complex_InputSignal1.Count; i++)
                    Multiply_Signals.Add(Complex_InputSignal1[i] * Complex_InputSignal2[i]);

                result = IFFT(Multiply_Signals);

                for (int i = 0; i < Complex_InputSignal1.Count; i++)
                    auto_correlation.Add((float)result[i].Real / Complex_InputSignal1.Count / Complex_InputSignal1.Count);

                OutputNonNormalizedCorrelation = auto_correlation;

                for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                    OutputNormalizedCorrelation.Add((float)(OutputNonNormalizedCorrelation[i] / normaliz_sum));
            }
            else // cross-correlation
            {
                List<float> auto_correlation = new List<float>();
                List<float> signal1_samples = new List<float>();
                List<float> signal2_samples = new List<float>();
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    signal1_samples.Add(InputSignal1.Samples[i]);
                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                    signal2_samples.Add(InputSignal2.Samples[i]);

                int signal1_length = (int)Math.Pow(2, Math.Ceiling(Math.Log(signal1_samples.Count, 2)));
                int signal2_length = (int)Math.Pow(2, Math.Ceiling(Math.Log(signal2_samples.Count, 2)));
                int max_length = Math.Max(signal1_length, signal2_length);

                for (int i = signal1_samples.Count; i < max_length; i++)
                    signal1_samples.Add(0);
                for (int i = signal2_samples.Count; i < max_length; i++)
                    signal2_samples.Add(0);

                double normalization_summation = 0, signal1_samples_summation = 0, signal2_samples_summation = 0;
                for (int i = 0; i < signal1_samples.Count; i++)
                {
                    signal1_samples_summation += signal1_samples[i] * signal1_samples[i];
                    signal2_samples_summation += signal2_samples[i] * signal2_samples[i];
                }
                normalization_summation = signal1_samples_summation * signal2_samples_summation;
                normalization_summation = Math.Sqrt(normalization_summation);
                normalization_summation /= signal1_samples.Count;

                List<Complex> InputSignal1Complex = new List<Complex>();
                InputSignal1Complex = FFT(signal1_samples);
                for (int i = 0; i < InputSignal1Complex.Count; i++)
                    InputSignal1Complex[i] = new Complex(InputSignal1Complex[i].Real,- InputSignal1Complex[i].Imaginary );

                List<Complex> InputSignal2Complex = new List<Complex>();
                InputSignal2Complex = FFT(signal2_samples);

                List<Complex> SignalsMultiplication = new List<Complex>();

                for (int i = 0; i < InputSignal1Complex.Count; i++)
                    SignalsMultiplication.Add(InputSignal1Complex[i] * InputSignal2Complex[i]);

                List<Complex> answer = new List<Complex>();
                answer = IFFT(SignalsMultiplication);

                for (int i = 0; i < InputSignal1Complex.Count; i++)
                    auto_correlation.Add((float)answer[i].Real / InputSignal1Complex.Count / InputSignal1Complex.Count);

                OutputNonNormalizedCorrelation = auto_correlation;

                for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                    OutputNormalizedCorrelation.Add((float)(OutputNonNormalizedCorrelation[i] / normalization_summation));
            }
        }

        private List<Complex> FFT(List<float> signal_samples)
        {
            List<Complex> harmonics = new List<Complex>();
            if (signal_samples.Count == 2)
            {
                harmonics.Add(new Complex(signal_samples[0] + signal_samples[1], 0));
                harmonics.Add(new Complex(signal_samples[0] - signal_samples[1], 0));
                return harmonics;
            }
            else
            {
                List<float> even = new List<float>();
                List<float> odd = new List<float>();
                for (int i = 0; i < signal_samples.Count; i++)
                {
                    if (i % 2 == 0)
                        even.Add(signal_samples[i]);
                    else
                        odd.Add(signal_samples[i]);
                    harmonics.Add(0);
                }
                List<Complex> even_harmonics = FFT(even);
                List<Complex> odd_harmonics = FFT(odd);

                for (int i = 0; i < even.Count; i++)
                {
                    double theta = -1 * i * 2 * Math.PI / signal_samples.Count;
                    double real = Math.Cos(theta);
                    double imag = Math.Sin(theta);
                    harmonics[i] = even_harmonics[i] + odd_harmonics[i] * new Complex(real, imag);
                    harmonics[i + even.Count] = even_harmonics[i] - odd_harmonics[i] * new Complex(real, imag);
                }
                return harmonics;
            }
        }
        private List<Complex> IFFT(List<Complex> harmonics_amp_pha)
        {
            List<Complex> harmonics = new List<Complex>();
            if (harmonics_amp_pha.Count == 2)
            {
                harmonics.Add(harmonics_amp_pha[0] + harmonics_amp_pha[1]);
                harmonics.Add(harmonics_amp_pha[0] - harmonics_amp_pha[1]);
                return harmonics;
            }
            else
            {
                List<Complex> even = new List<Complex>();
                List<Complex> odd = new List<Complex>();
                for (int i = 0; i < harmonics_amp_pha.Count; i++)
                {
                    if (i % 2 == 0)
                        even.Add(harmonics_amp_pha[i]);
                    else
                        odd.Add(harmonics_amp_pha[i]);
                    harmonics.Add(0);
                }
                List<Complex> even_harmonics = IFFT(even);
                List<Complex> odd_harmonics = IFFT(odd);

                for (int i = 0; i < even.Count; i++)
                {
                    double sum = i * 2 * Math.PI / harmonics_amp_pha.Count;
                    double real = Math.Cos(sum);
                    double imag = Math.Sin(sum);
                    harmonics[i] = even_harmonics[i]+ odd_harmonics[i]* new Complex(real, imag);
                    harmonics[i + even.Count] = even_harmonics[i]- odd_harmonics[i]* new Complex(real, imag);
                }

                return harmonics;
            }
        }
    }
}