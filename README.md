# FTCBuild
> A tool to build FTC projects on Windows.
## Building
Open this in Visual Studio (tested in 2019 Preview 6.10), and build it the normal way. You should then have an item called `FTCBuild (Package)` in your Start menu.

## Running
Either run it from VS, or use the aforementioned launcher.

## Usage
First, download the Windows Zip file from [here](https://developer.android.com/studio/releases/platform-tools) and unzip it somewhere. \
Enter your project location and Android SDK Platform Tools location (where you unzipped the Zip file), and choose a build variant from the menu. Then, choose either Build (builds your RC code), Install (builds & installs it on the robot), or Run (builds & installs your code, then re-launches the Robot Controller app to reflect your changes).