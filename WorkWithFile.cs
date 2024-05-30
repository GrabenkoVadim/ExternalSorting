using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace KP;

public class WorkWithFile
{
    private string _partOfInputFileElements;
    private string _inputFilePath;
    
    public string getInput_List()
    {
        return _partOfInputFileElements;
    }

    public string GetInputFilePath()
    {
        return _inputFilePath;
    }

    [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
    public void SetupFile(int size, int min, int max)
    {
        _inputFilePath = InputFileMaker(size);
        
        Random random = new Random();
        File.WriteAllText(_inputFilePath, string.Empty);
        StringBuilder stringBuilder = new StringBuilder();

        using (var fileStream = new FileStream(_inputFilePath, FileMode.Append, FileAccess.Write, FileShare.None, 4096,
                   FileOptions.WriteThrough))
        using (var streamWriter = new StreamWriter(fileStream))
        {
            for (int i = 0; i < size; i++)
            {
                int randomNumber = random.Next(min, max);
                streamWriter.Write(randomNumber);
                streamWriter.Write('\n');
                if (i < 300)
                {
                    stringBuilder.Append(randomNumber).Append(" ");
                }
            }

            _partOfInputFileElements = stringBuilder.ToString();
        }
    }

    private string InputFileMaker(int size)
    {
        string directoryPath = Directory.GetCurrentDirectory();
        string fileName;
        if (size is > 1_000 and < 1_000_000)
        {
            fileName = "input_" + size / 1_000 + "k.txt";
        }
        else if (size < 1_000)
        {
            fileName = "input_" + size + ".txt";
        }
        else
        {
            fileName = "input_" + size / 1_000_000 + "m.txt";
        }

        string filePath = Path.Combine(directoryPath, fileName);
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        if (!File.Exists(filePath)) File.Create(filePath).Close();
        return filePath;
    }
    
    public string OutputFileMaker(long size)
    {
        string directoryPath = Directory.GetCurrentDirectory();
        string fileName;
        if (size is > 1_000 and < 1_000_000)
        {
            fileName = "output_" + size / 1_000 + "k.txt";
        }
        else if (size < 1_000)
        {
            fileName = "output_" + size + ".txt";
        }
        else
        {
            fileName = "output_" + size / 1_000_000 + "m.txt";
        }

        string filePath = Path.Combine(directoryPath, fileName);
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        if (!File.Exists(filePath)) File.Create(filePath).Close();
        return filePath;
    }
}