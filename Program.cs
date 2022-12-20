using System.Runtime.InteropServices;
using EzVid2TgWebm.Const;
using EzVid2TgWebm.Core;

namespace EzVid2TgWebm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                bool windowsMode = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                string inputFolder = windowsMode ?
                                     Path.GetFullPath(Constants.PATH_WIN_INPUT) :
                                     Path.GetFullPath(Constants.PATH_LINUX_INPUT);
                string outputFolder = windowsMode ?
                                      Path.GetFullPath(Constants.PATH_WIN_OUTPUT) :
                                      Path.GetFullPath(Constants.PATH_LINUX_OUTPUT);

                // Create input and output folders: Stop execution if no input folder was detected
                if (!Directory.Exists(inputFolder))
                {
                    Directory.CreateDirectory(inputFolder);
                    Console.WriteLine($"Input folder created. Add all files to convert in:\n{inputFolder}");
                    return;
                }

                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                    Console.WriteLine($"Output folder created. All convert files will be created in:\n{outputFolder}");
                }
                else
                {
                    FileInfo[] oldFiles = new DirectoryInfo(outputFolder).GetFiles();

                    foreach (FileInfo file in oldFiles)
                    {
                        file.Delete();
                    }
                }

                int bitrate = 512;

                if (args.Length == 0)
                {
                    Console.WriteLine("No bitrate informed, using default value (512 kbps)");
                }
                else
                {
                    bool validBitrate = int.TryParse(args[0], out int parsedBitrate);
                    bitrate = validBitrate ? parsedBitrate : 512;
                    Console.WriteLine(validBitrate ? $"Using custom bitrate: {bitrate} kbps" : "No valid bitrate informed, using standard value (512 kbps)");
                }

                // Get all files from the input folder
                IEnumerable<string> foundFiles = new FileHandler().GetAllInputFilenames(inputFolder);

                // Convert each of the files
                foreach (string filename in foundFiles)
                {
                    FfmpegHandler handler = new FfmpegHandler(windowsMode);
                    handler.ConvertVideo(filename, bitrate);
                    Console.WriteLine("Video conversion finished.\n");
                }

                Console.WriteLine("PROCESS FINISHED.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex is NullReferenceException ?
                                  $"Unexpected Failure: {ex.Message}\n{ex.StackTrace}" :
                                  $"Failure: {ex.Message}");
            }
            finally
            {
                Console.Write("\nPress any key to exit.");
                Console.ReadKey();
            }
        }
    }
}