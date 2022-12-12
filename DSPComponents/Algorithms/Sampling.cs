﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), false);
            List <float> list_of_signal_values =  new List<float>();
            if (M == 0 &&L != 0 )   //upsampling
            {
                for(int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    list_of_signal_values.Add(InputSignal.Samples[i]);
                    for(int j = 0; j < L-1; j++)
                        list_of_signal_values.Add(0);
                }
                InputSignal = new Signal(list_of_signal_values, false);
                FIR f = new FIR();
                f.InputTimeDomainSignal = InputSignal;
                f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                f.InputFS = 8000;
                f.InputStopBandAttenuation = 50;
                f.InputCutOffFrequency = 1500;
                f.InputTransitionBand = 500;
                f.Run();
                OutputSignal=f.OutputYn;
            }
            else if (L == 0 && M != 0)  //down sampling
            {
                FIR f = new FIR();
                f.InputTimeDomainSignal = InputSignal;
                f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                f.InputFS = 8000;
                f.InputStopBandAttenuation = 50;
                f.InputCutOffFrequency = 1500;
                f.InputTransitionBand = 500;
                f.Run();              
                for (int i = 0; i <f.OutputYn.Samples.Count();i+=M )
                {
                    list_of_signal_values.Add(f.OutputYn.Samples[i]);
                   
                }
                OutputSignal = new Signal(list_of_signal_values, false);
            }
            else if(L!=0 && M != 0)
            {
                List<float> list_of_signal_values2 = new List<float>();

                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    list_of_signal_values2.Add(InputSignal.Samples[i]);
                    for (int j = 0; j < L-1; j++)
                    {
                        list_of_signal_values2.Add(0);
                    }
                }
                InputSignal = new Signal(list_of_signal_values2, false);
                FIR f = new FIR();
                f.InputTimeDomainSignal = InputSignal;
                f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                f.InputFS = 8000;
                f.InputStopBandAttenuation = 50;
                f.InputCutOffFrequency = 1500;
                f.InputTransitionBand = 500;
                f.Run();
                List<float> list_of_signal_values3 = new List<float>();

                for (int i = 0; i < f.OutputYn.Samples.Count(); i += M)
                {
                    list_of_signal_values3.Add(f.OutputYn.Samples[i]);

                }
                OutputSignal = new Signal(list_of_signal_values3, false);
            }
            else
                Console.WriteLine("error message");
        }
    }
}