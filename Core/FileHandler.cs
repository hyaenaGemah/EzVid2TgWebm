using System.Text;
using EzVid2TgWebm.Const;

namespace EzVid2TgWebm.Core
{
    public class FileHandler
    {
        public IEnumerable<string> GetAllInputFilenames(string inputDir)
        {
            string[] foundFiles = Directory.GetFiles(inputDir).Where(current => ValidateFileFormat(current)).ToArray();
            StringBuilder fileList = new StringBuilder().AppendLine("\nFile(s) found to convert:");

            foreach (string filename in foundFiles)
            {
                fileList.AppendLine($" -> {filename}");
            }

            Console.WriteLine(foundFiles.Length == 0 ? "No file found to convert. Halting conversion process." : fileList.ToString());
            return foundFiles;
        }

        private bool ValidateFileFormat(string filename)
        {
            string extension = filename.Split('.').Last();
            return Constants.FFMPEG_SUPPORTED_FORMATS.Contains(extension);
        }
    }
}