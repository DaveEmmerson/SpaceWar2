This is a C# version of the Space War game we (Matthew Watson and I) wrote
a long time ago in Pascal.

Setting up a development environment
1) Install Visual Studio 2010 SP1 (works in Express Edition too)

2) For Windows 8 install only you will need to have installed of Games for Windows Marketplace before step 3 will succeed: 
http://www.xbox.com/en-US/LIVE/PC/DownloadClient)

3) Install XNA GameStudio 4
http://www.microsoft.com/en-us/download/details.aspx?id=23714

4) Open SpaceWar.sln in Visual Studio 2010

Setting up Visual Studio 2012 development environment
1) Set up VS 2010 environment as above

2) Copy the XNA Game Extension from VS10 to VS11:
From: C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\Extensions\Microsoft\XNA Game Studio 4.0
To C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\Extensions\Microsoft\XNA Game Studio 4.0

3) Edit the extension.vsixmanifest file and change the <VisualStudio Version=10.0> tag to 11.0. I had to grant myself modify permissions on this file.

4) You may have to run the command to tell Visual Studio that new extensions are available. If you see an 'access denied' message, try launching the console as an administrator. 
	"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\devenv.exe" /setup
