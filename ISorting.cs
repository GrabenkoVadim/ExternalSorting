namespace KP;

public interface ISorting
{
    int GetDiskReadCount();
    
    int GetDiskWriteCount();
    
    string GetPartOfOutputFileElements();

    void Sort(string inputFile, string outputFile);
}