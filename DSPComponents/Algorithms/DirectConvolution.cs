using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            //List<List<float>> matrix = new List<List<float>>();
            float[,] matrix = new float[InputSignal2.Samples.Count, InputSignal1.Samples.Count];
            List<float> Output = new List<float>();
            List<int> Output_Indeices = new List<int>();
            for (int i = 0; i < InputSignal2.Samples.Count; i++)
            {
                for (int j = 0; j < InputSignal1.Samples.Count; j++)
                {
                    matrix[i,j] = InputSignal2.Samples[i] * InputSignal1.Samples[j];
                }
            }

            int Width = InputSignal1.Samples.Count;
            int Hieght = InputSignal2.Samples.Count;
            for (int col = 0; col < Width; col++)
            {
                int startcol = col, startrow = 0;
                float sum = 0;

                while (startcol >= 0 && startrow < Hieght)
                {
                    sum += matrix[startrow, startcol];
                    startcol--;
                    startrow++;
                }
                Output.Add(sum);
            }

            // For each row start column is N-1
            for (int row = 1; row < Hieght; row++)
            {
                int startrow = row, startcol = Width - 1;
                float sum = 0;
                while (startrow < Hieght && startcol >= 0)
                {
                    sum += matrix[startrow, startcol];
                    startcol--;
                    startrow++;
                }
                Output.Add(sum);
            }
            int index_of_zero_1stlist = 0, index_of_zero_2ndlist = 0, origin = 0;
            if (InputSignal1.SamplesIndices.Count > 0)
            {
                index_of_zero_1stlist = InputSignal1.SamplesIndices.IndexOf(0);
                index_of_zero_2ndlist = InputSignal2.SamplesIndices.IndexOf(0);
                origin = index_of_zero_1stlist + index_of_zero_2ndlist;
            }
            for (int i = 0; i < Output.Count; i++)
            {
                Output_Indeices.Add(i - origin);
            }

            OutputConvolvedSignal = new Signal(Output, Output_Indeices, false);

        }
    }
}
