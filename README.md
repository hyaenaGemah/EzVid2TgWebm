# EzVid2TgWebm
### aka Easy Video to Telegram (ready) WebM Converter

*A program to trivialize video conversion to a Telegram Video Sticker compatible format.*

*This program is open-source, and due to the nature of relying on FFMPEG, it is currently made available under the [GPL v2 License](https://www.gnu.org/licenses/old-licenses/gpl-2.0.html)*

This is a lazy program for converting video files into WebM video files ready for creating Telegram Video Stickers.
For reference, Telegram Video Stickers require the input file to:

 - Have a resolution **512x512 pixels** (square ratio)
 - Be at most **3 seconds** long
 - Be at most **256 kilobytes** in size
 - Have 30 frames per second
 - Have no audio

You can read more about it here: [Telegram Stickers](https://core.telegram.org/stickers)

EzVid2TgWebm also relies on FFMPEG to properly function, as it lacks functionalities to manipulate videos, and FFMPEG already performs video manipulation/conversion reliably.
~~You'll need an executable binary in a folder named *"ffmpeg"* in the same directory as the program, or have the FFMPEG package installed (if you use a Linux based OS).~~ Current releases come with FFMPEG bundled.

---

## Preparing the input video files

Any video editing software can be used to create the input files. It is only necessary that the input video file obeys the requirements described above, so the program can generate the proper output to be used to create video stickers in Telegram.

Also, any video file format compatible with FFMPEG is accepted as input, though it is recommended that the input files are saved to mp4.

---

## How to use

EzVid2TgWebm is a console application written in .NET 6.0, being cable to run in both Microsoft Windows and Linux distributions.
Support for other OSes is not planned as I don't have access to those.

Both releases have a script (Bash - for Linux, or Powershell - for Windows) in order to run the program with ease. It also contains the following folders:

 - Core: Has the executable and all necessary libraries and binaries for proper functioning.
    - ffmpeg: Folder within *Core* containing the FFMPEG executable in order to perform the conversion.
 - Input: All the files to be converted must be placed in this folder.
 - Output: All the files resulting from the conversion will be saved to this folder.
 
In order to properly run the program, place all the video files in the *Input* folder and run the script (*from Console* if in Linux, *right-click and then click on "Run in Powershell" or similar option* if in Windows). If no issues happen during the process, the newly generated files will be in the *Output* folder.
 
**WARNING:** *Be sure to copy any converted video file from the __Output__ folder __before running the program again, or it'll delete the previously generated files.__*
 
The program function like this in order to minimize any specific configuration or fine tuning in order to achieve the result of generating video files ready to be made into stickers.

However, if you're a more advanced user, you can run the executables directly from Console, or edit the script files to add a custom bitrate in order to increase the overall quality of the resulting file (as long as it doesn't exceed 256 kB).

Custom bitrates are considered by adding an argument to the Console, or altering te script to have a custom bitrate value in kilobytes, for example:

`./EzVid2TgWebm.exe 400`

This command will generate WebM video files with the bitrate of 400 kB. Although, if the resulting file exceeds 256 kB, the program will regenerate the file again, by calculating another bitrate value in order to generate a file of 256 kB or smaller.

---

## tl;dr

1. Ensure your video complies with the [Telegram Video Stickers requirements](https://core.telegram.org/stickers)
2. Run in a terminal or command line (right-click option on Windows, if available): 

     - For Windows: `EzVid2TgWebm.ps1 [Custom Bitrate - Optional]`
     - For Linux: `EzVid2TgWebm.sh [Custom Bitrate - Optional]`

3. If it all goes well, there will be a files in the output folder, else there will be a failure message shown
