dotnet new sln
dotnet new console --output JokeGenerator --use-program-main
dotnet new mstest --output JokeGeneratorTests

dotnet sln add JokeGeneratorTests
dotnet sln add JokeGenerator


<ProjectReference Include="./../JokeGenerator/JokeGenerator.csproj"/>

dotnet add package Microsoft.Extensions.Hosting


application builder.
Configure services - Microsoft.Extensions.DependencyInjection



dotnet add package moq

test commit 
