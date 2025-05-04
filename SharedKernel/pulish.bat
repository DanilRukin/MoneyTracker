@echo off
setlocal

:: Проверяем наличие переменной окружения
if "%GITHUB_TOKEN%"=="" (
    echo Error: environment variable GITHUB_TOKEN not found
    pause
    exit /b 1
)

:: Собираем пакет
dotnet pack src/SharedKernel/SharedKernel.csproj --configuration Release --output ./artifacts

if %errorlevel% neq 0 (
    echo Error during packing of project
    pause
    exit /b %errorlevel%
)

:: Публикуем пакет с использованием переменной окружения
dotnet nuget push artifacts\*.nupkg --source "https://nuget.pkg.github.com/DanilRukin/index.json" --api-key %GITHUB_TOKEN%

if %errorlevel% neq 0 (
    echo Error during publishing of project
    pause
    exit /b %errorlevel%
)

echo Package published successfully!
pause
