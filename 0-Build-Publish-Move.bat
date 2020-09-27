dotnet restore

msbuild Savory.Canos.Engine.ElementUI.sln /p:configuration=release

nuget push Savory.Canos.Engine.ElementUI\bin\Release\Savory.Canos.Engine.ElementUI.*.nupkg -source https://package.savory.cn/v3/index.json
nuget push Savory.Canos.Engine.ElementUI.Resources\bin\Release\Savory.Canos.Engine.ElementUI.Resources.*.nupkg -source https://package.savory.cn/v3/index.json
nuget push Savory.Canos.Manager.ElementUI\bin\Release\Savory.Canos.Manager.ElementUI.*.nupkg -source https://package.savory.cn/v3/index.json

move /Y Savory.Canos.Engine.ElementUI\bin\Release\Savory.Canos.Engine.ElementUI.*.nupkg D:\LocalSavoryNuget\
move /Y Savory.Canos.Engine.ElementUI.Resources\bin\Release\Savory.Canos.Engine.ElementUI.Resources.*.nupkg D:\LocalSavoryNuget\
move /Y Savory.Canos.Manager.ElementUI\bin\Release\Savory.Canos.Manager.ElementUI.*.nupkg D:\LocalSavoryNuget\

pause
