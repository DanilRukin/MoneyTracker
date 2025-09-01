@echo off
setlocal enabledelayedexpansion

:: Конфигурация
set "PROJECT_PATH=src/MoneyTracker.Infrastructure/MoneyTracker.Infrastructure.csproj"
set "OUTPUT_DIR=./artifacts"
set "NUGET_SOURCE=https://nuget.pkg.github.com/DanilRukin/index.json"
set "MAX_RETRIES=3"
set "RETRY_DELAY=5"

:: Проверка переменных окружения
if "%GITHUB_TOKEN%"=="" (
    echo [ERROR] Environment variable GITHUB_TOKEN not found
    pause
    exit /b 1
)

:: Очистка предыдущих артефактов
if exist "%OUTPUT_DIR%" (
    echo [INFO] Cleaning artifacts directory...
    rmdir /s /q "%OUTPUT_DIR%"
)
mkdir "%OUTPUT_DIR%"

:: Сборка пакета
echo [INFO] Building and packing the project...
dotnet pack "%PROJECT_PATH%" --configuration Release --output "%OUTPUT_DIR%" --include-symbols -p:SymbolPackageFormat=snupkg

if %errorlevel% neq 0 (
    echo [ERROR] Failed to build and pack the project
    pause
    exit /b %errorlevel%
)

:: Публикация пакетов с повторными попытками
set "SUCCESS=0"

for %%f in ("%OUTPUT_DIR%\*.nupkg") do (
    set "PACKAGE_PATH=%%f"
    
    echo [INFO] Publishing package: %%~nxf
    set "ATTEMPT=1"
    
    :retry_publish
    dotnet nuget push "!PACKAGE_PATH!" --source "%NUGET_SOURCE%" --api-key %GITHUB_TOKEN% --skip-duplicate
    
    if %errorlevel% equ 0 (
        set "SUCCESS=1"
        echo [SUCCESS] Package published successfully
    ) else if %errorlevel% equ 1 (
        echo [WARNING] Package might be a duplicate or other non-critical error occurred
        set "SUCCESS=1"
    ) else (
        echo [ERROR] Attempt !ATTEMPT! of %MAX_RETRIES% failed
        set /a "ATTEMPT+=1"
        
        if !ATTEMPT! leq %MAX_RETRIES% (
            timeout /t %RETRY_DELAY% /nobreak >nul
            goto retry_publish
        )
    )
)

:: Публикация символов (только если основной пакет успешно опубликован)
if %SUCCESS% equ 1 (
    for %%f in ("%OUTPUT_DIR%\*.snupkg") do (
        echo [INFO] Publishing symbols: %%~nxf
        dotnet nuget push "%%f" --source "%NUGET_SOURCE%" --api-key %GITHUB_TOKEN% --skip-duplicate
    )
)

:: Итоговый статус
if %SUCCESS% equ 1 (
    echo [SUCCESS] All packages published successfully!
) else (
    echo [ERROR] Failed to publish packages after %MAX_RETRIES% attempts
)

pause