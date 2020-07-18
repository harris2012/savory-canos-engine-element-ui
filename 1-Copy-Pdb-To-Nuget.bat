dotnet restore

msbuild Savory.Canos.Engine.ElementUI.sln /p:configuration=debug

copy-pdb-to-nuget debug

pause