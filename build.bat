dotnet restore --source "https://api.nuget.org/v3/index.json" --no-cache
dotnet pack "Warden.Services.Users.Shared" -o .
