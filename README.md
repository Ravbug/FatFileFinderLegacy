# FatFileFinder

## Information
This program sweeps a folder or drive and displays the size of each item in the folder, to assist the user in cleaning their drive.
This program uses the .NET framework and runs on Windows only.
Similar to [WinDirStat](https://windirstat.net/), but lighter and uses a background thread.

## How to Download
1. Ensure you have the .NET Framework 4.6.1 or later installed. Windows 10 has it installed by default. You can [download it from Microsoft here.](https://www.microsoft.com/en-us/download/details.aspx?id=49981)
2. Download the executable from the [releases](https://github.com/Ravbug/FatFileFinder/releases) tab. Your antivirus might not like unsigned EXEs, you might have to pause it.
3. Double click it to run it. 

## How to Size a Folder
1. Click the folder button (📁) in the toolbar to choose a folder or drive.
2. Press OK. The program will begin sizing the folder and updating the view.
3. To stop sizing early, press the `Stop` button at the bottom of the window. 

## Other Usage info
* Single click rows in the table to view their properties in the sidebar. Double click a row to open the folder to the right. 
You can also use the up and down arrows to move the selection. 
* To refresh a folder, select it in the table and press the refresh button (🔁). The program will refresh the contents of that folder.
If you want to reload the root folder, you will have to re-size it using the 📁 button.
* To view an item in the File Explorer, select it in the table and either press the ➡ toolbar button, or press `Reveal in Explorer` in the sidebar.
* To copy the full path to an item, select it in the table and press `Copy Path` in the sidebar.

## Screenshots
![Main Window](https://i.imgur.com/19IEXtf.png)

## Compiling it yourself
This program was compiled with Visual Studio Community 2017. Older versions of Visual Studio are not guarenteed to work.
1. Clone the repo as a ZIP file using the green `Clone or Download` button.
2. Unzip the downloaded file, and open `FatFileFinder.sln`
3. In Visual Studio, change the ``Debug`` dropdown in the toolbar to ``Release`` if it's not set already.
3. In Visual Studio, press ``Build -> Build Solution`` or press Ctrl+Shift+B
4. Navigate to ``FatFileFinder\FatFileFinder\bin\Release`` and double-click `FatFileFinder.exe` to run it.

Compiling it yourself usually prevents your antivirus from deleting the exe, so if you have no way to control your antivirus, this could be the way to go.

## Reporting bugs
To report a bug, use the [Issues](https://github.com/Ravbug/FatFileFinder/issues) tab on this github page.

## Todo
- [ ] Fix progress bar not resizing properly
- [ ] There's probably some edge case somewhere that makes it crash
- [x] Adjust column view sizing
