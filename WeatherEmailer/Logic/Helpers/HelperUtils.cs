using System;
using System.IO;

public class HelperUtils
{
    public static bool CheckFileExistsInBuildDirectory(string fileName)
    {
        return File.Exists(AppContext.BaseDirectory + fileName);
    }
}