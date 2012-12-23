set nunit="C:\Program Files (x86)\NUnit 2.6.2\bin\nunit-console-x86.exe"

C:\Users\%username%\AppData\Local\Apps\OpenCover\OpenCover.Console.exe -register:user -target:%nunit% -targetargs:"".\SpaceWar2Tests\bin\Debug\DEMW.SpaceWar2Tests.dll" /noshadow" -filter:+[*]* -output:coverageCode.xml
C:\Users\%username%\AppData\Local\Apps\OpenCover\OpenCover.Console.exe -register:user -target:%nunit% -targetargs:"".\SpaceWar2Tests\bin\Debug\DEMW.SpaceWar2Tests.dll" /noshadow" -filter:+[*]* -output:coverageTests.xml
pause
