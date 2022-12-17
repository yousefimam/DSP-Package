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
            
            Console.WriteLine(InputSignal); //display the given signal

            FIR f = new FIR();
            f.InputTimeDomainSignal = InputSignal;
            f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            f.InputTransitionBand = maxF - miniF;
            f.InputFS = Fs;
            f.InputStopBandAttenuation = 50;
            f.InputCutOffFrequency = 1500;
            f.InputTransitionBand = 500;
            f.Run();  // filter signal with fir filter with band maxf and minif

            StreamWriter WriteInFile = new StreamWriter("FIRCoefficients.txt");
            for (int i = 0; i < f.OutputHn.Samples.Count(); i++)
                WriteInFile.WriteLine(f.OutputHn.Samples[i]);
            WriteInFile.Close();

            if (newFs >= 2 * maxF)
            {
                Sampling s = new Sampling();
                s.L = L;
                s.M = M;
                s.Run();
                StreamWriter WriteInFile2 = new StreamWriter("Sampled_Signal.txt");
                for (int i = 0; i < s.OutputSignal.Samples.Count(); i++)
                    WriteInFile2.WriteLine(s.OutputSignal.Samples.Count());
                WriteInFile2.Close();
            }
            else
                Console.WriteLine("newFs is not valid");


            DC_Component remove_dc = new DC_Component();
            remove_dc.InputSignal= InputSignal;
            remove_dc.Run();
            StreamWriter WriteInFile3 = new StreamWriter("Removed_DC.txt");
            for (int i = 0; i < remove_dc.OutputSignal.Samples.Count(); i++)
                 WriteInFile3.WriteLine(remove_dc.OutputSignal.Samples.Count());
            WriteInFile3.Close();
            Console.WriteLine(remove_dc.OutputSignal); // display resulted signal after removing dc component

            Normalizer n = new Normalizer();
            n.InputSignal = InputSignal;
            n.InputMinRange = -1;
            n.InputMaxRange = 1;
            n.Run();
            StreamWriter WriteInFile4 = new StreamWriter("Normalized_Signal.txt");
            for (int i = 0; i < n.OutputNormalizedSignal.Samples.Count(); i++)
                WriteInFile4.WriteLine(n.OutputNormalizedSignal.Samples.Count());
            Console.WriteLine(n.OutputNormalizedSignal); // display normalized signal

            DiscreteFourierTransform d = new DiscreteFourierTransform();
            d.InputSamplingFrequency = newFs;
            d.InputTimeDomainSignal = InputSignal;
            d.Run();
            StreamWriter WriteInFile5 = new StreamWriter("DFT_Signal.txt");
            for (int i = 0; i < d.OutputFreqDomainSignal.Samples.Count(); i++)
                WriteInFile5.WriteLine(d.OutputFreqDomainSignal.Samples.Count());
            Console.WriteLine(d.OutputFreqDomainSignal); // display DFT signal
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
