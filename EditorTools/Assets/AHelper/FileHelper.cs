using System.IO;

namespace Tools
{
    public static class FileHelper
    {
        public static void CreateFile(string path, string message)
        {
            if (string.IsNullOrEmpty(path)) throw new IOException("CreateFile path is null or Empty");

            string filePath = Path.Combine(path, Timer.GetCurTimeDays() + ".log");

            using (StreamWriter file = new StreamWriter(filePath, true))
            {
                file.Write(message);
            }
        }
    }
}