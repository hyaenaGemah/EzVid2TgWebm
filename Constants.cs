using System.Text;

namespace EzVid2TgWebm.Const
{
    public class Constants
    {
        public static int MAX_FILE_SIZE = 262144; // 256 Kilobytes.
        public static readonly string FFMPEG_COMMAND_TEMPLATE = "-y -i \"{0}.mp4\" -r 30 -t 2.99 -an -c:v libvpx-vp9 -pix_fmt yuva420p -s 512x512 -b:v {1}K \"{2}.webm\"";
        public static readonly string FFMPEG_LINUX_PATH = @"./ffmpeg/ffmpeg";
        public static readonly string FFMPEG_WIN_PATH = @".\ffmpeg\ffmpeg.exe";

        public static string HelpMessage()
        {
            StringBuilder howToMsg = new StringBuilder().AppendLine("EzVid2TgWeb v0.2 - by Hyaena L. Gemah 2022")
                                                        .AppendLine("\n=== HOW TO USE ===\n")
                                                        .Append("1. Use a command line tool and type the executable name, ")
                                                        .Append("followed by the full path to the file and the expected bit rate, if desired. ")
                                                        .AppendLine("If the full path contains any white spaces, include it between quotations (\"\").")
                                                        .AppendLine("\n    Example:\n\t\t * EzVid2TgWebm.exe \"C:\\video to convert\\file.mp4\" 600")
                                                        .AppendLine("\t\t * EzVid2TgWebm /home/videos/file.mp4 800\n")
                                                        .Append("2. If the file is not within specifications, either the program won't perform the ")
                                                        .AppendLine("conversion or the outcome may not be what is expected.\n")
                                                        .Append("3. This program expects a mp4 video file with 512x512 resolution, max of 3 secs. duration ")
                                                        .AppendLine("and 30 fps at most.\n")
                                                        .Append("4. The program will output the file in the WEBM format, compatible with the Telegram ")
                                                        .Append("sticker bot. The bot is the recommended tool for creating the actual stickers, this ")
                                                        .AppendLine("program merely trivializes the video conversion step.\n")
                                                        .Append("5. The converted file will be output on the same directory where this program is. ")
                                                        .AppendLine("If there is an output file, it'll be deleted before the next file will be output.\n")
                                                        .Append("\nPress any key to exit...");
            return howToMsg.ToString();
        }
    }
}