@echo off
set addOnRegDataFolder=C:\Proxys\AddOnRegDataGen
set repositoryRootFolder=C:\Proxys\Projetos\Addons\sps-consultoria-curitiba
set sevenZipRootFolder=C:\Program Files\7-Zip
ECHO Version Generator.
set /p versionBuild=Version Number [y/n]?: 
"%addOnRegDataFolder%\AddOnRegDataGen.exe" "%repositoryRootFolder%\sps-consultoria-curitiba\Lumix\Nordware.AddOn.Lumix\Setup\AddOnInfo.xml" "%versionBuild%" "%repositoryRootFolder%\sps-consultoria-curitiba\Lumix\Nordware.AddOn.Lumix\Setup\setup.exe" "%repositoryRootFolder%\sps-consultoria-curitiba\Lumix\Nordware.AddOn.Lumix\Setup\setup.exe" "%repositoryRootFolder%\sps-consultoria-curitiba\Lumix\Nordware.AddOn.Lumix\Nordware.AddOn.Lumix\bin\Debug\Nordware.AddOn.Lumix.exe"
"%sevenZipRootFolder%\7z.exe" a -tzip "%repositoryRootFolder%\sps-consultoria-curitiba\Lumix\Nordware.AddOn.Lumix\Setup\Output\Nordware.AddOn.Lumix_v%versionBuild%.zip" @listFile.txt