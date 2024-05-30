using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KP
{
    public class BalancedMultiwayMerging : ISorting
    {
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
        
        const string TempFilePrefix = "temp_chunk_";
        const int NumberOfChunks = 10;

        public void Sort(string inputFilePath, string outputFilePath)
        {
            var chunks = SplitIntoChunks(inputFilePath);
            MergeChunks(chunks, outputFilePath);
            First300ElementsForTextField(outputFilePath);
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

        private List<string> SplitIntoChunks(string inputFilePath)
        {
            var chunks = new List<string>();

            int totalLines = 0;
            using (var reader = new StreamReader(inputFilePath))
            {
                _diskReadCount++;
                while (reader.ReadLine() != null)
                {
                    totalLines++;
                }
            }

            int chunkSize = (int)Math.Ceiling((double)totalLines / NumberOfChunks);

            using (var reader = new StreamReader(inputFilePath))
            {
                _diskReadCount++;
                int[] buffer = new int[chunkSize];
                int count = 0;
                int chunkIndex = 1;

                while (!reader.EndOfStream)
                {
                    string line;
                    while (count < chunkSize && (line = reader.ReadLine()) != null)
                    {
                        buffer[count++] = int.Parse(line);
                    }

                    if (count > 0)
                    {
                        Array.Sort(buffer, 0, count);
                        string chunkFilePath = Path.Combine(Directory.GetCurrentDirectory(),
                            $"{TempFilePrefix}{chunkIndex++}.txt");
                        chunks.Add(chunkFilePath);
                        using (var writer = new StreamWriter(chunkFilePath))
                        {
                            _diskWriteCount++; 
                            for (int i = 0; i < count; i++)
                            {
                                writer.WriteLine(buffer[i]);
                            }
                        }
                        count = 0;
                    }
                }
            }
            return chunks;
        }

        private void MergeChunks(List<string> chunks, string outputFilePath)
        {
            int phase = 0;
            while (chunks.Count > 2)
            {
                var newChunks = new List<string>();
                int chunkIndex = 1;

                for (int i = 0; i < chunks.Count; i += 2)
                {
                    if (i + 1 < chunks.Count)
                    {
                        string mergedChunk = Path.Combine(Directory.GetCurrentDirectory(),
                            $"{TempFilePrefix}{phase}_{chunkIndex++}.txt");
                        MergeTwoChunks(chunks[i], chunks[i + 1], mergedChunk);
                        newChunks.Add(mergedChunk);
                    }
                    else
                    {
                        string lastChunk = Path.Combine(Directory.GetCurrentDirectory(),
                            $"{TempFilePrefix}{phase}_{chunkIndex++}.txt");
                        File.Move(chunks[i], lastChunk);
                        newChunks.Add(lastChunk);
                    }
                }

                foreach (var chunk in chunks)
                {
                    File.Delete(chunk);
                }

                chunks = newChunks;
                phase++;
            }

            if (chunks.Count == 2)
            {
                MergeTwoChunks(chunks[0], chunks[1], outputFilePath);

                File.Delete(chunks[0]);
                File.Delete(chunks[1]);
            }
            else if (chunks.Count == 1)
            {
                File.Move(chunks[0], outputFilePath);
            }
        }

        private void MergeTwoChunks(string chunk1, string chunk2, string outputChunk)
        {
            using (var reader1 = new StreamReader(chunk1))
            using (var reader2 = new StreamReader(chunk2))
            using (var writer = new StreamWriter(outputChunk))
            {
                _diskReadCount++;
                _diskReadCount++;
                _diskWriteCount++;
                string line1 = reader1.ReadLine();
                string line2 = reader2.ReadLine();

                while (line1 != null && line2 != null)
                {
                    if (int.Parse(line1) <= int.Parse(line2))
                    {
                        writer.WriteLine(line1);
                        line1 = reader1.ReadLine();
                    }
                    else
                    {
                        writer.WriteLine(line2);
                        line2 = reader2.ReadLine();
                    }
                }

                while (line1 != null)
                {
                    writer.WriteLine(line1);
                    line1 = reader1.ReadLine();
                }

                while (line2 != null)
                {
                    writer.WriteLine(line2);
                    line2 = reader2.ReadLine();
                }
            }
        }
    }
}
