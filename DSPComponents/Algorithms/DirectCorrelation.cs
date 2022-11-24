using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> to_multiply;
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            if (InputSignal2 == null)
            {
                if (InputSignal1.Periodic == true)
                {
                    #region Auto-Correlation for periodic signals
                    to_multiply = new List<float>(InputSignal1.Samples);
                    int N = InputSignal1.Samples.Count;
                    //Normalization Value
                    float X = 0;
                    for (int i = 0; i < N; i++)
                    {
                        X += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                    }
                    X = X / N;

                    //Correlation
                    for (int i = 0; i < N; i++)
                    {
                        float result = 0;
                        for (int j = 0; j < N; j++)
                        {
                            result += InputSignal1.Samples[j] * to_multiply[j];
                        }
                        result = result / N;
                        float Normalized = result / X;
                        OutputNonNormalizedCorrelation.Add(result);
                        OutputNormalizedCorrelation.Add(Normalized);
                        float first_value = to_multiply[0];
                        to_multiply.RemoveAt(0);
                        to_multiply.Add(first_value);
                    }
                }
                #endregion Auto-Correlation for periodic signals
                else
                {
                    #region Auto-Correlation for Non-periodic signals
                    to_multiply = new List<float>(InputSignal1.Samples);
                    int N = InputSignal1.Samples.Count;
                    //Normalization Value
                    float X = 0;
                    for (int i = 0; i < N; i++)
                    {
                        X += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                    }
                    X = X / N;

                    //Correlation
                    for (int i = 0; i < N; i++)
                    {
                        float result = 0;
                        for (int j = 0; j < N; j++)
                        {
                            result += InputSignal1.Samples[j] * to_multiply[j];
                        }
                        result = result / N;
                        float Normalized = result / X;
                        OutputNonNormalizedCorrelation.Add(result);
                        OutputNormalizedCorrelation.Add(Normalized);
                        to_multiply.RemoveAt(0);
                        to_multiply.Add(0.0f);
                    }
                    #endregion Auto-Correlation for Non-periodic signals
                }
            }
            else
            {
                if (InputSignal1.Periodic == true)
                {
                    #region Cross-Correlation for periodic signals
                    to_multiply = new List<float>(InputSignal2.Samples);
                    int N = InputSignal1.Samples.Count;
                    //Normalization Value
                    float X = 0 , Y = 0;
                    for (int i = 0; i < N; i++)
                    {
                        X += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                        Y += InputSignal2.Samples[i] * InputSignal2.Samples[i];
                    }
                    X = ((float)Math.Sqrt(X * Y))/N;

                    //Correlation
                    for (int i = 0; i < N; i++)
                    {
                        float result = 0;
                        for (int j = 0; j < N; j++)
                        {
                            result += InputSignal1.Samples[j] * to_multiply[j];
                        }
                        result =result / N;
                        float Normalized =result / X;
                        OutputNonNormalizedCorrelation.Add(result);
                        OutputNormalizedCorrelation.Add(Normalized);
                        float first_value = to_multiply[0];
                        to_multiply.RemoveAt(0);
                        to_multiply.Add(first_value);
                    }
                    #endregion Cross-Correlation for periodic signals
                }
                else
                {
                    #region Cross-Correlation for Non-periodic signals
                    to_multiply = new List<float>(InputSignal2.Samples);
                    int N = InputSignal1.Samples.Count;
                    //Normalization Value
                    float X = 0, Y = 0;
                    for (int i = 0; i < N; i++)
                    {
                        X += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                        Y += InputSignal2.Samples[i] * InputSignal2.Samples[i];
                    }
                    X = ((float)Math.Sqrt(X * Y)) / N;

                    //Correlation
                    for (int i = 0; i < N; i++)
                    {
                        float result = 0;
                        for (int j = 0; j < N; j++)
                        {
                            result += InputSignal1.Samples[j] * to_multiply[j];
                        }
                        result = result / N;
                        float Normalized = result / X;
                        OutputNonNormalizedCorrelation.Add(result);
                        OutputNormalizedCorrelation.Add(Normalized);
                        to_multiply.RemoveAt(0);
                        to_multiply.Add(0.0f);
                    }
                    #endregion Cross-Correlation for Non-periodic signals
                }
            }
        }
    }
}