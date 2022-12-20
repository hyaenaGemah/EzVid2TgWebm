using System.Diagnostics;
using System.Management.Automation;
using EzVid2TgWebm.Const;

namespace EzVid2TgWebm.Core
{
    /// <summary>
    /// Class that implements the logic that will handle calling the FFMPEG executable binary.
    /// </summary>
    public class FfmpegHandler
    {
        /// <summary>
        /// <c>true</c> if the OS this app is being run in is Windows, <c>false</> otherwise.
        /// </summary>
        private bool WindowsMode { get; set; }

        public FfmpegHandler(bool windowsMode)
        {
            WindowsMode = windowsMode;
        }

        public void ConvertVideo(string filename, int bitrate)
        {
            if (WindowsMode)
            {
                ConvertVideoWin(filename, bitrate);
            }
            else
            {
                bool ffmpegBinFound = File.Exists(Constants.FFMPEG_LINUX_PATH);
                string binaryCommand = !ffmpegBinFound ? "ffmpeg" : Path.GetFullPath(Constants.FFMPEG_LINUX_PATH);
                ConvertVideoLinux(filename, bitrate, binaryCommand);
            }
        }

        /// <summary>
        /// Runs the Linux FFMPEG binary to convert the video to the Telegram Sticker ready format.
        /// </summary>
        /// <param name="filename">The input file name.</param>
        /// <param name="bitrate">The bitrate to consider when converting.</param>
        /// <param name=executableIsPresent">If the executable binary is present or not.</param>
        private void ConvertVideoLinux(string filename, int bitrate, string binaryCommand)
        {
            string fileNameFormatless = Path.GetFileNameWithoutExtension(filename);
            string fullPathOutput = $"{Path.GetFullPath(Constants.PATH_LINUX_OUTPUT)}/{fileNameFormatless}.webm";
            string cmdArgs = string.Format(Constants.FFMPEG_COMMAND_TEMPLATE, filename, bitrate, fullPathOutput);

            Console.WriteLine($"Using bitrate {bitrate}k for: {filename}");
            RunLinuxFfmpegProcess(binaryCommand, cmdArgs);
            long fileSize = new FileInfo(fullPathOutput).Length;

            if (fileSize > Constants.MAX_FILE_SIZE)
            {
                File.Delete(fullPathOutput);
                int optimalBitrate = (int)Math.Floor(0m + Constants.MAX_FILE_SIZE * bitrate / fileSize);
                Console.WriteLine($"Converted file exceeds 256kB, trying with optimal calculated bitrate: {optimalBitrate}");
                cmdArgs = string.Format(Constants.FFMPEG_COMMAND_TEMPLATE, filename, optimalBitrate, fullPathOutput);
                RunLinuxFfmpegProcess(binaryCommand, cmdArgs);
            }
        }

        /// <summary>
        /// Runs the Windows FFMPEG executable to convert the video to the Telegram Sticker ready format.
        /// </summary>
        /// <param name="filename">The input file name.</param>
        /// <param name="bitrate">The bitrate to consider when converting.</param>
        private void ConvertVideoWin(string filename, int bitrate)
        {
            string fileNameFormatless = Path.GetFileNameWithoutExtension(filename);
            string fullPathOutput = $"{Path.GetFullPath(Constants.PATH_WIN_OUTPUT)}\\{fileNameFormatless}.webm";
            string cmdArgs = string.Format(Constants.FFMPEG_COMMAND_TEMPLATE, filename, bitrate, fullPathOutput);

            Console.WriteLine($"Using bitrate {bitrate}k for: {filename}");
            RunWinFfmpegProcess(cmdArgs);
            long fileSize = new FileInfo(fullPathOutput).Length;

            if (fileSize > Constants.MAX_FILE_SIZE)
            {
                File.Delete(fullPathOutput);
                int optimalBitrate = (int)Math.Floor(0m + Constants.MAX_FILE_SIZE * bitrate / fileSize);
                Console.WriteLine($"Converted file exceeds 256kB, trying with optimal calculated bitrate: {optimalBitrate}");
                cmdArgs = string.Format(Constants.FFMPEG_COMMAND_TEMPLATE, filename, optimalBitrate, fullPathOutput);
                RunWinFfmpegProcess(cmdArgs);
            }
        }

        private void RunLinuxFfmpegProcess(string command, string args)
        {
            ProcessStartInfo process = new ProcessStartInfo(command, args)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                UseShellExecute = false
            };

            using (Process? current = Process.Start(process))
            {
                current?.WaitForExit();
            }
        }

        private void RunWinFfmpegProcess(string args)
        {
            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript($"{Path.GetFullPath(Constants.FFMPEG_WIN_PATH)} {args}");
                ps.Invoke();
            }
        }
    }
}