﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);
            

            FIR f = new FIR();
            f.InputTimeDomainSignal = InputSignal;
            if (miniF != 0 && maxF != 0)
            {
                f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
                f.InputF1 = miniF;
                f.InputF2 = maxF;
            }
            else if (miniF == 0 && maxF != 0)
            {
                f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                f.InputCutOffFrequency = maxF;
            }
            else if (miniF != 0 && maxF == 0)
            {
                f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.HIGH;
                f.InputCutOffFrequency = miniF;
            }
            f.InputFS = Fs;
            f.InputStopBandAttenuation = 50;
            f.InputTransitionBand = 500;
            f.Run();  


            /*StreamWriter WriteInFile = new StreamWriter("FilteredSignal.txt");
            WriteInFile.WriteLine(0);
            WriteInFile.WriteLine(0);
            WriteInFile.WriteLine(f.OutputYn.Samples.Count());
            for (int i = 0; i < f.OutputYn.Samples.Count(); i++)
            {
                WriteInFile.Write(f.OutputYn.SamplesIndices[i]);
                WriteInFile.Write(" ");
                WriteInFile.Write(f.OutputYn.Samples[i]);
                WriteInFile.WriteLine(" ");
            }
            WriteInFile.Close();*/
            

            Sampling s = new Sampling();
            if (newFs >= Fs/2)
            {
                s.L = L;
                s.M = M;
                s.InputSignal = f.OutputYn;
                s.Run();
                /*WriteInFile = new StreamWriter("SampledSignal.txt");
                WriteInFile.WriteLine(0);
                WriteInFile.WriteLine(0);
                WriteInFile.WriteLine(s.OutputSignal.Samples.Count());
                for (int i = 0; i < s.OutputSignal.Samples.Count(); i++)
                {
                    WriteInFile.Write(s.OutputSignal.SamplesIndices[i]);
                    WriteInFile.Write(" ");
                    WriteInFile.Write(s.OutputSignal.Samples[i]);
                    WriteInFile.WriteLine(" ");
                }
                WriteInFile.Close();*/
            }
            else
                Console.WriteLine("newFs is not valid");
            

            DC_Component dc = new DC_Component();
            dc.InputSignal= s.OutputSignal;
            dc.Run();
            

            /*WriteInFile = new StreamWriter("DC_Removed_Signal.txt");
            WriteInFile.WriteLine(0);
            WriteInFile.WriteLine(0);
            WriteInFile.WriteLine(dc.OutputSignal.Samples.Count());
            for (int i = 0; i < dc.OutputSignal.Samples.Count(); i++)
            {
                WriteInFile.Write(dc.OutputSignal.SamplesIndices[i]);
                WriteInFile.Write(" ");
                WriteInFile.Write(dc.OutputSignal.Samples[i]);
                WriteInFile.WriteLine(" ");
            }
            WriteInFile.Close();*/


            Normalizer n = new Normalizer();
            n.InputSignal = dc.OutputSignal;
            n.InputMinRange = -1;
            n.InputMaxRange = 1;
            n.Run();


            /*WriteInFile = new StreamWriter("Normalized_Signal.txt");
            WriteInFile.WriteLine(0);
            WriteInFile.WriteLine(0);
            WriteInFile.WriteLine(n.OutputNormalizedSignal.Samples.Count());
            for (int i = 0; i < n.OutputNormalizedSignal.Samples.Count(); i++)
            {
                WriteInFile.Write(n.OutputNormalizedSignal.SamplesIndices[i]);
                WriteInFile.Write(" ");
                WriteInFile.Write(n.OutputNormalizedSignal.Samples[i]);
                WriteInFile.WriteLine(" ");
            }
            WriteInFile.Close();*/


            DiscreteFourierTransform d = new DiscreteFourierTransform();
            d.InputSamplingFrequency = Fs;
            d.InputTimeDomainSignal = n.OutputNormalizedSignal;
            d.Run();
            OutputFreqDomainSignal = d.OutputFreqDomainSignal;
            
            /*
            WriteInFile = new StreamWriter("FreqDomSignal.txt");
            WriteInFile.WriteLine(1);
            WriteInFile.WriteLine(0);
            WriteInFile.WriteLine(d.OutputFreqDomainSignal.Frequencies.Count());
            for (int i = 0; i < d.OutputFreqDomainSignal.Frequencies.Count(); i++)
            {
                WriteInFile.Write(d.OutputFreqDomainSignal.Frequencies[i]);
                WriteInFile.Write(" ");
                WriteInFile.Write(d.OutputFreqDomainSignal.FrequenciesAmplitudes[i]);
                WriteInFile.Write(" ");
                WriteInFile.Write(d.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                WriteInFile.WriteLine(" ");
            }
            WriteInFile.Close();
            */
        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
