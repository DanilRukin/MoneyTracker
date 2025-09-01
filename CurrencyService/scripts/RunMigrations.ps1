Start-Transcript -Path "migrations.log"

param(
    [string]$MigrationName,
    [string]$Context = "CurrencyServiceContext",
    [string]$StartupProject = "../srs/MoneyTracker.CurrencyService.Api",
    [string]$MigrationsProject = "../srs/MoneyTracker.CurrencyService.Data.MSSQL",
    [string]$OutputDir = "Migrations/CurrencyServiceDb"
)

# Шаг 1: Добавление миграции (если указано имя)
if (-not [string]::IsNullOrEmpty($MigrationName)) {
    Write-Host "Adding migration '$MigrationName'..." -ForegroundColor Cyan
    dotnet ef migrations add $MigrationName `
        --context $Context `
        --project $MigrationsProject `
        --startup-project $StartupProject `
        --output-dir $OutputDir
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Migration creation failed!" -ForegroundColor Red
        exit 1
    }
}

# Шаг 2: Применение миграций
Write-Host "Applying migrations..." -ForegroundColor Cyan
dotnet ef database update `
    --context $Context `
    --project $MigrationsProject `
    --startup-project $StartupProject

if ($LASTEXITCODE -ne 0) {
    Write-Host "Database update failed!" -ForegroundColor Red
    exit 1
}

Write-Host "Migrations successfully applied!" -ForegroundColor Green

Stop-Transcript
