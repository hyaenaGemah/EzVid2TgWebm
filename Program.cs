using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using EzVid2TgWebm.Const;

namespace EzVid2TgWebm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0 || string.IsNullOrEmpty(args.FirstOrDefault()))
                {
                    Console.WriteLine(Constants.HelpMessage());
                    Console.ReadKey();
                    return;
                }

                string fileFullPath = args[0];
                int bitrate = 512;

                if (args.Length < 2)
                {
                    Console.WriteLine("No bitrate informed, using standard value (512 kbps)");
                }
                else
                {
                    bool validBitrate = int.TryParse(args[1], out int parsedBitrate);
                    bitrate = validBitrate ? parsedBitrate : 512;
                    Console.WriteLine(validBitrate ? $"Using custom bitrate: {bitrate} kbps" : "No bitrate informed, using standard value (512 kbps)");
                }

                if (!File.Exists(fileFullPath))
                {
                    throw new FileNotFoundException("File not found!");
                }

                string fileExtension = Path.GetExtension(fileFullPath).ToUpperInvariant().Substring(1);

                if (!fileExtension.Equals("MP4"))
                {
                    throw new FormatException($"File is not compatible: {fileExtension}");
                }

                bool isWin = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                string outputPath = (isWin ? ".\\output" : "./output");
                string outputFilePath = $"{outputPath}.webm";

                if (File.Exists(outputFilePath))
                {
                    File.Delete(outputFilePath);
                }

                Console.WriteLine("Starting video file conversion...");
                RunConversionProcess(isWin, fileFullPath, bitrate, outputPath);
                bool fileConverted = File.Exists(outputFilePath);
                long fileSize = new FileInfo(outputFilePath).Length;

                if (fileSize > Constants.MAX_FILE_SIZE)
                {
                    do
                    {
                        bitrate = (int)(0.9 * bitrate);
                        Console.WriteLine($"Generated file is bigger than 256 kilobytes. Reattempting conversion with smaller bitrate: {bitrate}.");
                        RunConversionProcess(isWin, fileFullPath, bitrate, outputPath);
                        fileConverted = File.Exists(outputFilePath);
                        fileSize = new FileInfo(outputFilePath).Length;
                    }
                    while (fileSize > Constants.MAX_FILE_SIZE);
                }

                Console.WriteLine($"Process finished. Result: {(!fileConverted ? "FAILURE" : "SUCCESS")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex is NullReferenceException ?
                                  $"Unexpected Failure: {ex.Message}\n{ex.StackTrace}" :
                                  $"Failure: {ex.Message}");
            }
        }

        private static void RunConversionProcess(bool isWin, string fileFullPath, int bitrate, string outputPath)
        {
            string executablePath = isWin ? Constants.FFMPEG_WIN_PATH : Constants.FFMPEG_LINUX_PATH;

            if ((!File.Exists(executablePath) && isWin))
            {
                throw new FileNotFoundException("Windows FFMPEG executable was not found!");
            }
            else if ((!File.Exists(executablePath) && !isWin))
            {
                Console.WriteLine("Linux FFMPEG binary not detected, attempting package installation instead.");
                executablePath = "ffmpeg";
            }

            string filename = Path.GetFileNameWithoutExtension(fileFullPath);
            string filePathAndName = Path.GetDirectoryName(fileFullPath) + (isWin ? '\\' : '/') + filename;
            string cmdArgs = string.Format(Constants.FFMPEG_COMMAND_TEMPLATE, filePathAndName, bitrate, outputPath);
            ProcessStartInfo process = new ProcessStartInfo(executablePath, cmdArgs)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                UseShellExecute = false
            };

            using (Process? current = Process.Start(process))
            {
                while (!(current?.HasExited).GetValueOrDefault())
                {
                    // Await...
                }
            }
        }
    }
}