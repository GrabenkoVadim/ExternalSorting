using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace KP
{
    public class PolyphaseSorting : ISorting
    {
        private  string _partOfOutputFileElements;
        private  int _diskReadCount;
        private  int _diskWriteCount;
        
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

        public void Sort(string inputFile, string outputFile)
        {
            var subsequences = CreateAndSetupFiles(inputFile, out var selectedFibonacciNumbers);
            var tempFiles = TempFiles();
            SplitIntoFiles(subsequences, selectedFibonacciNumbers, tempFiles);
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }
            tempFiles = MergeRuns(tempFiles);
            foreach (var tempFile in tempFiles)
            {
                if (File.Exists(tempFile))
                {
                    File.Move(tempFile, outputFile);
                }
            }

            SortFinalFile(outputFile);
            LinesCleaner(outputFile);
            First300ElementsForTextField(outputFile);
        }

        [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
        private  string[] MergeRuns(string[] tempFiles)
        {
            int counter = 0;

            while (true)
            {
                List<string> tempFilesList = tempFiles.ToList();
                string receiver = null;
                foreach (string tempFile in tempFiles)
                {
                    if (!File.Exists(tempFile))
                    {
                        tempFilesList.Remove(tempFile);
                        continue;
                    }
                    FileInfo fileInfo = new FileInfo(tempFile);
                    _diskReadCount++;
                    if (fileInfo.Length == 0)
                    {
                        if (tempFilesList.Count > 2)
                        {
                            receiver = tempFile;
                            break;
                        }

                        tempFilesList.Remove(tempFile);
                        File.Delete(tempFile);
                        break;
                    }
                }

                if (tempFilesList.Count <= 1)
                {
                    break;
                }

                if (receiver == null)
                {
                    var fileSubsequenceCounts = tempFilesList.ToDictionary(file => file, SubsCounter);
                    var minFile = fileSubsequenceCounts.OrderBy(kvp => kvp.Value).First().Key;
                    var nextMinFile = fileSubsequenceCounts.Where(kvp => kvp.Key != minFile).OrderBy(kvp => kvp.Value)
                        .FirstOrDefault().Key;
                    if (nextMinFile == null)
                    {
                        receiver = minFile;
                    }
                    else
                    {
                        RedistributeElements(minFile, nextMinFile, SubsCounter(minFile));
                        receiver = minFile;
                    }
                }

                BalanceFiles(tempFiles, receiver);

                foreach (string tempFile in tempFiles)
                {
                    if (!File.Exists(tempFile))
                    {
                        continue;
                    }
                    FileInfo fileInfo = new FileInfo(tempFile);
                    _diskReadCount++;
                    if (fileInfo.Exists && tempFile != receiver && fileInfo.Length == 0)
                    {
                        tempFilesList.Remove(tempFile);
                        fileInfo.Delete();
                    }
                }

                tempFiles = tempFilesList.ToArray();
                int count = Merge(tempFiles, receiver, counter);
                if (count == 0) break;

                foreach (string tempFile in tempFiles)
                {
                    if (tempFile != receiver)
                    {
                        DeleteRuns(tempFile, count);
                    }
                }
            }

            return tempFiles;
        }

        [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
        private  void BalanceFiles(string[] tempFiles, string receiver)
        {
            var fileSubsequenceCounts = tempFiles.ToDictionary(file => file, SubsCounter);

            foreach (var file1 in fileSubsequenceCounts)
            {
                foreach (var file2 in fileSubsequenceCounts)
                {
                    if (file1.Key != file2.Key && file1.Key != receiver && file2.Key != receiver && file1.Value > 0 &&
                        file2.Value > 0)
                    {
                        if (file1.Value >= 2 * file2.Value)
                        {
                            RedistributeElements(file1.Key, file2.Key, file1.Value / 2);
                        }
                    }
                }
            }
        }

        private  void LinesCleaner(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
            _diskReadCount++;
            _diskWriteCount++;
            File.WriteAllLines(filePath, lines);
        }

        private  int SubsCounter(string fileName)
        {
            int count = 0;
            using (StreamReader sr = new StreamReader(fileName))
            {
                _diskReadCount++;
                string? line;
                bool inSubsequence = false;

                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        if (inSubsequence)
                        {
                            count++;
                            inSubsequence = false;
                        }
                    }
                    else
                    {
                        inSubsequence = true;
                    }
                }
                if (inSubsequence)
                {
                    count++;
                }
            }
            return count;
        }

        private  void RedistributeElements(string sourceFile, string targetFile, int numberOfSubsequencesToMove)
        {
            List<string> elementsToMove = new List<string>();
            List<string> remainingElements = new List<string>();
            int subsequenceCount = 0;
            bool move = true;
            using (StreamReader sr = new StreamReader(sourceFile))
            {
                _diskReadCount++;
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        if (move && subsequenceCount < numberOfSubsequencesToMove)
                        {
                            elementsToMove.Add(line);
                        }
                        else
                        {
                            remainingElements.Add(line);
                        }
                    }
                    else
                    {
                        if (move && subsequenceCount < numberOfSubsequencesToMove)
                        {
                            elementsToMove.Add(line);
                            subsequenceCount++;
                        }
                        else
                        {
                            remainingElements.Add(line);
                        }

                        if (subsequenceCount >= numberOfSubsequencesToMove)
                        {
                            move = false;
                        }
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter(sourceFile))
            {
                _diskWriteCount++;
                foreach (var element in remainingElements)
                {
                    sw.WriteLine(element);
                }
            }
            using (StreamWriter sw = new StreamWriter(targetFile, true))
            {
                _diskWriteCount++;
                foreach (var element in elementsToMove)
                {
                    sw.WriteLine(element);
                }
            }
        }

        private void SortFinalFile(string outputFile)
        {
            string[] lines = File.ReadAllLines(outputFile);
            _diskReadCount++;
            List<int> numbers = new List<int>();
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    numbers.Add(int.Parse(line));
                }
            }
            numbers.Sort();
            string[] sortedLines = numbers.Select(num => num.ToString()).ToArray();
            _diskWriteCount++;
            File.WriteAllLines(outputFile, sortedLines);
        }

        private string[] TempFiles()
        {
            int tempFilesCount = 4;
            string[] tempFiles = new string[tempFilesCount];
            for (int i = 0; i < tempFilesCount; i++)
            {
                tempFiles[i] = $"temp{i}.txt";
                File.Create(tempFiles[i]).Dispose();
                _diskWriteCount++;
            }
            return tempFiles;
        }

        private  List<List<int>> CreateAndSetupFiles(string inputFile, out List<int> selectedFibonacciNumbers)
        {
            List<List<int>> subsequences = GetSubsequences(inputFile);
            int subsequencesCount = subsequences.Count;
            List<int> fibonacciNumbers = GetFibonacciNumbers(subsequencesCount);

            int sum = 0;
            selectedFibonacciNumbers = new List<int>();
            for (int i = fibonacciNumbers.Count - 1; i >= 0; i--)
            {
                if (sum + fibonacciNumbers[i] <= subsequencesCount)
                {
                    sum += fibonacciNumbers[i];
                    selectedFibonacciNumbers.Add(fibonacciNumbers[i]);
                    if (sum == subsequencesCount)
                    {
                        break;
                    }
                }
            }
            return subsequences;
        }

        private  List<List<int>> GetSubsequences(string inputFile)
        {
            List<List<int>> subsequences = new List<List<int>>();
            using (StreamReader sr = new StreamReader(inputFile))
            {
                _diskReadCount++;
                string? line;
                int? previousNumber = null;
                List<int> currentSubsequence = new List<int>();

                while ((line = sr.ReadLine()) != null)
                {
                    foreach (var num in line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        int number = int.Parse(num);

                        if (previousNumber != null && number < previousNumber)
                        {
                            subsequences.Add(new List<int>(currentSubsequence));
                            currentSubsequence.Clear();
                        }

                        currentSubsequence.Add(number);
                        previousNumber = number;
                    }
                }

                if (currentSubsequence.Count > 0)
                {
                    subsequences.Add(currentSubsequence);
                }
            }

            return subsequences;
        }

        private  void SplitIntoFiles(List<List<int>> subsequences, List<int> fibonacciNumbers,
            string[] tempFiles)
        {
            var largestFibonacciNumbers = fibonacciNumbers.OrderByDescending(x => x).Take(3).ToList();
            int subsequenceIndex = 0;
            int totalSubsequences = subsequences.Count;

            int remainingSubsequences = totalSubsequences - largestFibonacciNumbers.Sum();

            for (int i = 0; i < 3; i++)
            {
                using (StreamWriter sw = new StreamWriter(tempFiles[i]))
                {
                    _diskWriteCount++;
                    for (int j = 0; j < largestFibonacciNumbers[i]; j++)
                    {
                        if (subsequenceIndex < totalSubsequences)
                        {
                            foreach (int num in subsequences[subsequenceIndex])
                            {
                                sw.WriteLine(num);
                            }

                            sw.WriteLine();
                            subsequenceIndex++;
                        }
                    }

                    if (remainingSubsequences > 0)
                    {
                        int subsequencesToWrite = remainingSubsequences / (3 - i);
                        for (int k = 0; k < subsequencesToWrite; k++)
                        {
                            foreach (int num in subsequences[subsequenceIndex])
                            {
                                sw.WriteLine(num);
                            }

                            sw.WriteLine();
                            subsequenceIndex++;
                            remainingSubsequences--;
                        }
                    }
                }
            }
        }

        private  List<int> GetFibonacciNumbers(int limit)
        {
            List<int> fibonacciNumbers = new List<int> { 1, 1 };
            while (true)
            {
                int nextFibonacci = fibonacciNumbers[fibonacciNumbers.Count - 1] + fibonacciNumbers[fibonacciNumbers.Count - 2];
                if (nextFibonacci > limit)
                    break;
                fibonacciNumbers.Add(nextFibonacci);
            }
            return fibonacciNumbers;
        }

        [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
        private int Merge(string[] files, string receiver, int counter)
        {
            List<StreamReader> readers = new List<StreamReader>();
            foreach (string file in files)
            {
                if (file != receiver)
                {
                    readers.Add(new StreamReader(file));
                    _diskReadCount++;
                }
            }

            using (StreamWriter writer = new StreamWriter(receiver))
            {
                _diskWriteCount++;
                List<Queue<int>> queues = new List<Queue<int>>();
                foreach (StreamReader reader in readers)
                {
                    Queue<int> queue = new Queue<int>();
                    FillQueue(reader, queue);
                    queues.Add(queue);
                }

                bool continueMerging = true;

                while (continueMerging)
                {
                    List<List<int>> subsequences = new List<List<int>>();

                    foreach (Queue<int> queue in queues)
                    {
                        subsequences.Add(ReadNextRun(queue));
                    }
                    if (subsequences.Exists(subsequent => subsequent.Count == 0))
                    {
                        continueMerging = false;
                    }
                    else
                    {
                        MergeRuns(subsequences, writer);
                        counter++;
                    }
                }
            }

            foreach (StreamReader reader in readers)
            {
                reader.Close();
            }

            return counter;
        }

        [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
        private  List<int> ReadNextRun(Queue<int> queue)
        {
            List<int> subsequence = new List<int>();

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();
                subsequence.Add(current);

                if (queue.Count > 0 && queue.Peek() < current)
                {
                    break;
                }
            }
            return subsequence;
        }

        [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
        private  void MergeRuns(List<List<int>> subsequences, StreamWriter writer)
        {
            List<int> merged = new List<int>();

            foreach (var subsequence in subsequences)
            {
                merged.AddRange(subsequence);
            }

            merged.Sort();

            foreach (var num in merged)
            {
                writer.WriteLine(num);
            }

            writer.WriteLine();
        }

        [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
        private  void FillQueue(StreamReader reader, Queue<int> queue)
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    queue.Enqueue(int.Parse(line));
                }
            }
        }

        [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
        private  void DeleteRuns(string fileName, int numberOfSubsequencesToDelete)
        {
            List<List<int>> subsequences = new List<List<int>>();
            List<string> linesToKeep = new List<string>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                _diskReadCount++;
                string? line;
                List<int> currentSubsequence = new List<int>();

                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        subsequences.Add(currentSubsequence);
                        currentSubsequence = new List<int>();
                    }
                    else
                    {
                        currentSubsequence.Add(int.Parse(line));
                    }
                }

                if (currentSubsequence.Count > 0)
                {
                    subsequences.Add(currentSubsequence);
                }
            }

            for (int i = numberOfSubsequencesToDelete; i < subsequences.Count; i++)
            {
                foreach (int num in subsequences[i])
                {
                    linesToKeep.Add(num.ToString());
                }

                linesToKeep.Add(string.Empty);
            }

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                _diskWriteCount++;
                foreach (var line in linesToKeep)
                {
                    sw.WriteLine(line);
                }
            }
        }
    }
}
