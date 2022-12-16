using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace EzVid2TgWebm
{
    public class Program
    {
        private const string FFMPEG_COMMAND = "-y -i \"{0}.mp4\" -r 30 -t 2.99 -an -c:v libvpx-vp9 -pix_fmt yuva420p -s 512x512 -b:v {1}K {2}.webm";
        private const string FFMPEG_LINUX_X64_PATH = @"./ffmpeg/linux_x64/ffmpeg";
        private const string FFMPEG_LINUX_X86_PATH = @"./ffmpeg/linux_x86/ffmpeg";
        private const string FFMPEG_WIN_X86_PATH = @".\ffmpeg\win_x86\ffmpeg.exe";

        public static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0 || string.IsNullOrEmpty(args.FirstOrDefault()))
                {
                    StringBuilder howToMsg = new StringBuilder().AppendLine("=== HOW TO USE ===\n")
                                                                .Append("1. Use a command line tool and type the executable name, ")
                                                                .Append("followed by the full path to the file and the expected bit rate. ")
                                                                .AppendLine("If the full path contains any white spaces, include it between quotations (\"\").")
                                                                .AppendLine("\n    Example:\n\t\t * EzVid2TgWebm.exe \"C:\\video to convert\\file.mp4\" 600")
                                                                .AppendLine("\t\t * EzVid2TgWebm /home/videos/file.mp4 800\n")
                                                                .Append("2. If the file is not within specifications, either the program won't perform the ")
                                                                .AppendLine("conversion or the outcome may not be what is expected.\n")
                                                                .Append("3. This program expects a mp4 video file with 512x512 resolution, max of 3 secs. duration ")
                                                                .AppendLine("and 30 fps at most.\n")
                                                                .Append("4. The program will output the file in the WEBM format, compatible with the Telegram ")
                                                                .Append("sticker bot. The bot is the recommended tool for creating the actual stickers, this ")
                                                                .AppendLine("program merely trivializes the video conversion step.\n");
                    Console.WriteLine(howToMsg.ToString());
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

                bool isWin = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                bool isX64 = RuntimeInformation.OSArchitecture == Architecture.X64;
                string executablePath = FFMPEG_WIN_X86_PATH;

                if (!isWin)
                {
                    executablePath = isX64 ? FFMPEG_LINUX_X64_PATH : FFMPEG_LINUX_X86_PATH;
                }

                string fileExtension = Path.GetExtension(fileFullPath).ToUpperInvariant().Substring(1);

                if (!fileExtension.Equals("MP4"))
                {
                    throw new FormatException($"File is not compatible: {fileExtension}");
                }

                string outputPath = (isWin ? ".\\output" : "./output");

                if (File.Exists($"{outputPath}.webm"))
                {
                    File.Delete($"{outputPath}.webm");
                }

                string filename = Path.GetFileNameWithoutExtension(fileFullPath);
                string filePathAndName = Path.GetDirectoryName(fileFullPath) + (isWin ? '\\' : '/') + filename;
                string cmdArgs = string.Format(FFMPEG_COMMAND, filePathAndName, bitrate, outputPath);
                ProcessStartInfo process = new ProcessStartInfo(executablePath, cmdArgs)
                {
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = false,
                    UseShellExecute = false
                };
                bool fileConverted;

                using (Process? current = Process.Start(process))
                {
                    while (!(current?.HasExited).GetValueOrDefault())
                    {
                        // Await...
                    }
                    fileConverted = File.Exists($"{outputPath}.webm");
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
    }
}