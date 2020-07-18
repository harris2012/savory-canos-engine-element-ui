dotnet restore

msbuild Savory.Canos.Engine.ElementUI.sln /p:configuration=release

nuget push Savory.Canos.Engine.ElementUI\bin\Release\Savory.Canos.Engine.ElementUI.*.nupkg -source https://package.savory.cn/repository/savory-nuget-framework/
move /Y Savory.Canos.Engine.ElementUI\bin\Release\Savory.Canos.Engine.ElementUI.*.nupkg D:\LocalSavoryNuget\

nuget push Savory.Canos.Engine.ElementUI.Resources\bin\Release\Savory.Canos.Engine.ElementUI.Resources.*.nupkg -source https://package.savory.cn/repository/savory-nuget-framework/
move /Y Savory.Canos.Engine.ElementUI.Resources\bin\Release\Savory.Canos.Engine.ElementUI.Resources.*.nupkg D:\LocalSavoryNuget\

nuget push Savory.Canos.Manager.ElementUI\bin\Release\Savory.Canos.Manager.ElementUI.*.nupkg -source https://package.savory.cn/repository/savory-nuget-framework/
move /Y Savory.Canos.Manager.ElementUI\bin\Release\Savory.Canos.Manager.ElementUI.*.nupkg D:\LocalSavoryNuget\

pause
