using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            if (InputLevel <= 0)
                InputLevel = (int) Math.Pow(2, InputNumBits);
            else
                InputNumBits = ((int)Math.Log(InputLevel , 2.0));

            float maximum = InputSignal.Samples.Max();
            float minimum = InputSignal.Samples.Min();
            float delta = (float) (maximum - minimum) / InputLevel , halfofdelta = (float)(delta / 2) + 0.000002f;
            List<float> QuantizedValues = new List<float>();
            List<String> EncodedValues = new List<String>();
            List<int> IntervalIndices = new List<int>();
            List<float> SampledError = new List<float>();
            List<float> Midpoints = new List<float>();
            float index = minimum;
            for (int i = 0; i < InputLevel; i++)
            {
                Midpoints.Add((float)(index + index + delta)/2);
                index += delta;
            }
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < Midpoints.Count; j++)
                {
                    float diff = Math.Abs(InputSignal.Samples[i] - Midpoints[j]);
                    if (diff <= halfofdelta)
                    {
                        QuantizedValues.Add(Midpoints[j]);
                        IntervalIndices.Add(j + 1);
                        EncodedValues.Add(Convert.ToString(j, 2));
                        SampledError.Add(Midpoints[j] - InputSignal.Samples[i]);
                        break;
                    }
                }
            }
            for (int i = 0; i < EncodedValues.Count; i++)
            {
                    EncodedValues[i] = EncodedValues[i].PadLeft(InputNumBits, '0');  
            }

            OutputQuantizedSignal = new Signal( QuantizedValues,false);
            OutputIntervalIndices = IntervalIndices;
            OutputEncodedSignal = EncodedValues;
            OutputSamplesError = SampledError;
        }
    }
}
