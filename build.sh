dotnet restore --source https://api.nuget.org/v3/index.json --source https://www.myget.org/F/warden/api/v3/index.json --no-cache
dotnet pack "Warden.Services.Users.Shared" -o .
