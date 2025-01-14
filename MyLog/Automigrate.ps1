param (
    [string]$StartupProject = "Services/MyLog.Services.Api/MyLog.Services.Api.csproj", # Default startup project
    [string]$DataProject = "Data/MyLog.Data.DataAccess/MyLog.Data.DataAccess.csproj", # Default data project
    [string]$Context = "MyLogContext" # Default context
)

# Get the current timestamp
$timestamp = Get-Date -Format "yyyyMMddHHmmss"
$migrationName = "${timestamp}_AutoMig"

# Add the migration
dotnet ef migrations add $migrationName --startup-project $StartupProject --project $DataProject --context $Context

# Update the database
dotnet ef database update --startup-project $StartupProject --project $DataProject --context $Context

Write-Host "Migration $migrationName added and database updated successfully."
