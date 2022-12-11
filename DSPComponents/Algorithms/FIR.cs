using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            OutputHn = new Signal(new List<float>(), new List<int>(), false);
            float delta_F = InputTransitionBand / InputFS;
            int start = StartIndes(InputStopBandAttenuation, delta_F);
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                float F_DASH = (InputCutOffFrequency.Value + (InputTransitionBand / 2)) / InputFS;
                List<float> windows = CalculateW(start, InputStopBandAttenuation);
                float N = start * 2 + 1;
                for (int i = 0; i < N; i++)
                {
                    float n = Math.Abs(start - i);
                    if (n == 0)
                    {
                        float HD = 2 * F_DASH;
                        OutputHn.Samples.Add(HD * windows[i]);
                    }
                    else
                    {
                        float x = (float) Math.Sin(n * 2 * Math.PI * F_DASH) / (float) (n * 2 * Math.PI * F_DASH);
                        float HD = 2 * F_DASH * x;
                        OutputHn.Samples.Add(HD * windows[i]);
                    }
                }
                Indecies(start);
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                float F_DASH = (InputCutOffFrequency.Value - (InputTransitionBand / 2)) / InputFS;
                List<float> windows = CalculateW(start, InputStopBandAttenuation);
                float N = start * 2 + 1;
                for (int i = 0; i < N; i++)
                {
                    float n = Math.Abs(start - i);
                    if (n == 0)
                    {
                        float HD = 1 - (2 * F_DASH);
                        OutputHn.Samples.Add(HD * windows[i]);
                    }
                    else
                    {
                        float x = (float)Math.Sin(n * 2 * Math.PI * F_DASH) / (float)(n * 2 * Math.PI * F_DASH);
                        float HD = -2 * F_DASH * x;
                        OutputHn.Samples.Add(HD * windows[i]);
                    }
                }
                Indecies(start);
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                float F_DASH1 = (InputF1.Value - (InputTransitionBand / 2)) / InputFS;
                float F_DASH2 = (InputF2.Value + (InputTransitionBand / 2)) / InputFS;
                List<float> windows = CalculateW(start, InputStopBandAttenuation);
                float N = start * 2 + 1;
                for (int i = 0; i < N; i++)
                {
                    float n = Math.Abs(start - i);
                    if (n == 0)
                    {
                        float HD = 2 * (F_DASH2 - F_DASH1);
                        OutputHn.Samples.Add(HD * windows[i]);
                    }
                    else
                    {
                        float x = (float)Math.Sin(n * 2 * Math.PI * F_DASH2) / (float)(n * 2 * Math.PI * F_DASH2);
                        float y = (float)Math.Sin(n * 2 * Math.PI * F_DASH1) / (float)(n * 2 * Math.PI * F_DASH1);
                        float HD = (2 * F_DASH2 * x) - (2 * F_DASH1 * y) ;
                        OutputHn.Samples.Add(HD * windows[i]);
                    }
                }
                Indecies(start);
            }
            else
            {
                float F_DASH1 = (InputF1.Value + (InputTransitionBand / 2)) / InputFS;
                float F_DASH2 = (InputF2.Value - (InputTransitionBand / 2)) / InputFS;
                List<float> windows = CalculateW(start, InputStopBandAttenuation);
                float N = start * 2 + 1;
                for (int i = 0; i < N; i++)
                {
                    float n = Math.Abs(start - i);
                    if (n == 0)
                    {
                        float HD = 1 - (2 * (F_DASH2 - F_DASH1));
                        OutputHn.Samples.Add(HD * windows[i]);
                    }
                    else
                    {
                        float x = (float)Math.Sin(n * 2 * Math.PI * F_DASH2) / (float)(n * 2 * Math.PI * F_DASH2);
                        float y = (float)Math.Sin(n * 2 * Math.PI * F_DASH1) / (float)(n * 2 * Math.PI * F_DASH1);
                        float HD = (2 * F_DASH1 * y) - (2 * F_DASH2 * x);
                        OutputHn.Samples.Add(HD* windows[i]);
                    }
                }
                Indecies(start);
            }
            DirectConvolution dc = new DirectConvolution();

            dc.InputSignal1 = InputTimeDomainSignal;
            dc.InputSignal2 = new Signal(OutputHn.Samples,OutputHn.SamplesIndices,false);

            dc.Run();

            OutputYn = new Signal(dc.OutputConvolvedSignal.Samples, dc.OutputConvolvedSignal.SamplesIndices, false);

        }
        private int StartIndes(float SBA , float delta)
        {
            float coe;
            if (SBA <= 21)
                coe = 0.9f / delta;
            else if (SBA > 21 && SBA <= 44)
                coe = 3.1f / delta;
            else if (SBA > 44 && SBA <= 53)
                coe = 3.3f / delta;
            else
                coe = 5.5f / delta;

            int start;
            coe = (float) Math.Ceiling(coe);
            if (coe % 2.0f == 0)
                coe += 1.0f;

            start = (int)(coe - 1) / 2;
            return start;
        }
        private void Indecies(int start)
        {
            int N = start * 2 + 1;
            start *= -1;
            for (int i = 0; i < N; i++)
            {
                OutputHn.SamplesIndices.Add(start);
                //OutputYn.SamplesIndices.Add(start);
                start++;
            }
        }
        private List <float> CalculateW(int start, float SBA)
        {
            List<float> val = new List<float>();
            float N = start * 2 + 1;
            for (int i = 0; i < N; i++)
            {
                float n = Math.Abs(start - i);
                if (SBA <= 21)
                    val.Add(1);
                else if (SBA > 21 && SBA <= 44)
                {
                    float x = (float) Math.Cos((2 * Math.PI * n) / N);
                    val.Add(0.5f + 0.5f * x) ;
                }
                else if (SBA > 44 && SBA <= 53)
                {
                    float x = (float)Math.Cos((2 * Math.PI * n) / N);
                    val.Add(0.54f + 0.46f * x);
                }
                else
                {
                    float x = (float)Math.Cos((2 * Math.PI * n) / (N-1));
                    float y = (float)Math.Cos((4* Math.PI * n) / (N-1));
                    val.Add(0.42f + 0.5f * x + 0.08f * y);
                }
            }
            return val;
        }
    }
}
