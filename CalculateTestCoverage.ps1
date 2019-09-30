$solution = Get-ChildItem -Name *.sln
dotnet restore $solution
dotnet build $solution --configuration release /p:CopyLocalLockFileAssemblies=true
dotnet test  $solution --no-build --no-restore --configuration release --logger:trx -v minimal /p:CollectCoverage=true /p:Exclude=[xunit.*]* /p:CoverletOutputFormat=opencover
dotnet tool install -g dotnet-reportgenerator-globaltool # Use update in the future https://github.com/dotnet/cli/issues/11259
reportgenerator "-reports:.\**\*.opencover.xml" "-targetdir:coveragereport" -reporttypes:HTMLInline

. .\coveragereport\index.htm