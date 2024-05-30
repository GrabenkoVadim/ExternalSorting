using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KP
{
    public class NaturalMergeSort : ISorting
    {
        private const string TempFile1 = "temp1.txt";
        private const string TempFile2 = "temp2.txt";
        private string _partOfOutputFileElements;
        private int _diskReadCount;
        private int _diskWriteCount;

        public int GetDiskReadCount()
        {
            return _diskReadCount;
        }

        public int GetDiskWriteCount()
        {
            return _diskWriteCount;
        }

        public string GetPartOfOutputFileElements()
        {
            return _partOfOutputFileElements;
        }

        public void Sort(string inputFile, string outputFile)
        {
            SplitIntoRuns(inputFile, TempFile1, TempFile2);
            MergeRuns(TempFile1, TempFile2, outputFile);
            while (true)
            {
                SplitIntoRuns(outputFile, TempFile1, TempFile2);
                File.WriteAllText(outputFile, string.Empty);
                MergeRuns(TempFile1, TempFile2, outputFile);
                if (HasSingleRun(outputFile))
                {
                    break;
                }
            }

            File.Delete(TempFile1);
            File.Delete(TempFile2);
            First300ElementsForTextField(outputFile);
        }

        private bool HasSingleRun(string filePath)
        {
            int runCount = 0;
            using (StreamReader sr = new StreamReader(filePath))
            {
                _diskReadCount++;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        runCount++;
                        if (runCount > 1)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void SplitIntoRuns(string inputFile, string tempFile1, string tempFile2)
        {
            using (StreamReader streamReader = new StreamReader(inputFile))
            using (StreamWriter streamWriter = new StreamWriter(tempFile1))
            using (StreamWriter writer = new StreamWriter(tempFile2))
            {
                _diskReadCount++;
                _diskWriteCount += 2;
                bool writeToTemp1 = true;
                List<int> currentRun = new List<int>();
                string line;
                int? previousNumber = null;

                while ((line = streamReader.ReadLine()) != null)
                {
                    foreach (var num in line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        int number = int.Parse(num);

                        if (previousNumber != null && number < previousNumber)
                        {
                            WriteRun(currentRun, writeToTemp1 ? streamWriter : writer);
                            currentRun.Clear();
                            writeToTemp1 = !writeToTemp1;
                        }

                        currentRun.Add(number);
                        previousNumber = number;
                    }
                }

                if (currentRun.Count > 0)
                {
                    WriteRun(currentRun, writeToTemp1 ? streamWriter : writer);
                }
            }
        }

        private void WriteRun(List<int> run, StreamWriter sw)
        {
            foreach (int num in run)
            {
                sw.WriteLine(num);
            }

            sw.WriteLine();
            _diskWriteCount++;
        }

        private void MergeRuns(string tempFile1, string tempFile2, string outputFile)
        {
            using (StreamReader sr1 = new StreamReader(tempFile1))
            using (StreamReader sr2 = new StreamReader(tempFile2))
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                _diskReadCount += 2;
                _diskWriteCount++;
                List<int> run1 = ReadNextRun(sr1);
                List<int> run2 = ReadNextRun(sr2);

                while (run1 != null || run2 != null)
                {
                    List<int> mergedRun = MergeSort(run1, run2);
                    WriteRun(mergedRun, sw);

                    run1 = ReadNextRun(sr1);
                    run2 = ReadNextRun(sr2);
                }
            }
        }

        private List<int> ReadNextRun(StreamReader sr)
        {
            List<int> run = new List<int>();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                run.Add(int.Parse(line));
            }

            return run.Count > 0 ? run : null;
        }

        private List<int> MergeSort(List<int> run1, List<int> run2)
        {
            if (run1 == null) return run2;
            if (run2 == null) return run1;

            List<int> mergedRun = run1.Concat(run2).ToList();
            mergedRun.Sort();

            return mergedRun;
        }

        private void First300ElementsForTextField(string outputFilePath)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (var reader = new StreamReader(outputFilePath))
            {
                _diskReadCount++;
                for (int i = 0; i < 300 && !reader.EndOfStream; i++)
                {
                    string line = reader.ReadLine();
                    stringBuilder.Append(line).Append(" ");
                }
            }

            _partOfOutputFileElements = stringBuilder.ToString();
        }
    }
}
