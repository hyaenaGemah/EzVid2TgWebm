# EzVid2TgWebm
### aka Easy Video to Telegram (ready) WebM Converter

*A program to trivialize video conversion to a Telegram Video Sticker compatible format.*

***This program requires the [.NET 6.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) in order to work.***

This is a lazy program for converting MP4 video files into WebM video files ready for making Telegram Video Stickers.
For reference, Telegram Video Stickers require the input file to:

 - Have a resolution **512x512 pixels** (square ratio)
 - Be at most **3 seconds** long
 - Be at most **256 kilobytes** in size
 - Have 30 frames per second
 - Have no audio

You can read more about it here: [Telegram Stickers](https://core.telegram.org/stickers)

EzVid2TgWebm also relies on FFMPEG to properly function, as it lacks functionalities to manipulate videos, and FFMPEG already performs video manipulation/conversion reliably.
You'll need an executable binary in a folder named *"ffmpeg"* in the same directory as the program, or have the FFMPEG package installed (if you use a Linux based OS).

You can get FFMPEG for your operating system and architecture (32 or 64-bits) here: [FFMPEG download options](https://ffmpeg.org/download.html)

---

## How to use

EzVid2TgWebm is a console application written in .NET 6.0, being cable to run in both Microsoft Windows and Linux distributions.
Support for other OSes is not planned as I don't have access to those.

Since this program was coded in Visual Studio Code in a machine running Linux, there were no simple ways to code graphical interfaces, and so this program doesn't have one for the time being.

This program can be ran by itself, but since it requires the path of the file to be converted and a target bitrate (optional), it is mandatory you use a console of any type (Like *Command Line* or *PowerShell* in Windows, or any terminal running *Bash* in Linux).

Considering you have a FFMPEG binary or installation (Linux only), and a MP4 video file complying with the Telegram Stickers requirements described above, you should be able to use the program without any issues.

In the command line, type the executable name, followed by a space and the full path to the file, and if desired, followed by another space and a bitrate value. For example:

```
EzVid2TgWebm.exe "C:\Users\[Your user name here]\Videos\funny video.mp4" 1024
```

Assuming you're running in Windows, the previous command will run the app targeting the file in the path, with a bitrate of 1024 kilobytes per second (which will set the quality of the output, but also will increase file size).
In this example, the path is put between quote marks (") because it contain white spaces.

If you're running any Linux based OS, the following command will do the same, but using the default bitrate of 512 kbit/s instead:

```
EzVid2TgWebm /home/vid/file_to_convert.mp4
```

Though, in this case, the path doesn't have white spaces, so quote marks aren't necessary.

The program will then verify the existence of the file to convert, and the operating system in order to seek the proper executables and will attempt to convert the file.

If there are any failures, it'll point them out with a failure message, otherwise it'll output the file.
However, in cases the file is bigger than 256 kB, it'll reattempt to convert it again with 90% the specified bitrate until it generates a 256 kB file.

The file will be output in the same directory as the program, with the name *"output.webm"*

---

## tl;dr

1. Ensure your video complies with the [Telegram Video Stickers requirements](https://core.telegram.org/stickers)
2. Ensure there's a [FFMPEG executable](https://ffmpeg.org/download.html) in a folder called *"ffmpeg"* in the directory where the program is (unless you are running Linux with a package installed)
3. Run in a terminal or command line: 

     - For Windows: `EzVid2TgWebm.exe [Full path to your video] [Custom Bitrate - Optional]`
     - For Linux: `EzVid2TgWebm [Full path to your video] [Custom Bitrate - Optional]`

4. If it all goes alright, there will be a file ready named *"output.webm"* in the same folder as the program, else there will be a failure message shown
