del *.nupkg
.\.nuget\NuGet.exe pack .\ActIt\ActIt.csproj -Build -Prop Configuration=Release
.\.nuget\NuGet.exe push *.nupkg
pause