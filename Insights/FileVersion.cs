namespace Insights
{
    public class FileVersion
    {
        public static string Get(string fileName)
        {
            if (fileName.StartsWith('/'))
            {
                fileName = fileName.Substring(1);
            }
            string filePath = $"{Startup.WebRootPath}/{fileName}";
            return System.IO.File.GetLastWriteTime(filePath).ToString("yyyyMMddhhmmss");
        }
    }
}
