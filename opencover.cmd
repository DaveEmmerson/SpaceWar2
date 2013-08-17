set nunit="C:\Program Files (x86)\NUnit 2.6.2\bin\nunit-console-x86.exe"
set opencover=".\packages\OpenCover.4.0.1118\OpenCover.Console.exe"
set reportGenerator=".\packages\ReportGenerator.1.9.0.0\ReportGenerator.exe"

%opencover% -register:user -target:%nunit% -targetargs:"".\SpaceWar2Tests\bin\Debug\DEMW.SpaceWar2Tests.dll" /noshadow" -filter:"+[*]* -[*]*XnaWrappers* -[*SpaceWar2Tests*]*" -output:".\Coverage Report\coverageCode.xml"
%opencover% -register:user -target:%nunit% -tar\getargs:"".\SpaceWar2Tests\bin\Debug\DEMW.SpaceWar2Tests.dll" /noshadow" -filter:+[*SpaceWar2Tests*]* -output:".\Coverage Report\coverageTests.xml"

%reportGenerator% "-reports:.\Coverage Report\coverageCode.xml" "-targetdir:.\Coverage Report\Code"
%reportGenerator% "-reports:.\Coverage Report\coverageTests.xml" "-targetdir:.\Coverage Report\Tests"

pause
