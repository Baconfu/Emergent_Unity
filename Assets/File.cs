
using System.IO;


public class File
{
    private string data;
    public File(string fileName) 
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            data = sr.ReadToEnd();


        }
    }

    public string Data()
    {
        return data;
    }
}