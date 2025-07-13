@echo off
setlocal enabledelayedexpansion

:: ========================
:: Конфигурация
:: ========================
set "PROJECT_PATH=src/MoneyTracker.Infrastructure/MoneyTracker.Infrastructure.csproj"
set "OUTPUT_DIR=./artifacts"
set "NUGET_SOURCE=https://nuget.pkg.github.com/DanilRukin/index.json"
set "MAX_RETRIES=3"
set "RETRY_DELAY=5"

:: ========================
:: Функция для увеличения версии
:: ========================
:IncrementVersion
set "VERSION_FILE=%PROJECT_PATH%"
set "NEW_VERSION="

:: Парсим текущую версию из .csproj
for /f "tokens=3 delims=<>" %%a in ('type "%VERSION_FILE%" ^| find "<VersionPrefix"') do (
    set "CURRENT_VERSION=%%a"
    for /f "tokens=1-3 delims=." %%b in ("%%a") do (
        set /a "MAJOR=%%b"
        set /a "MINOR=%%c"
        set /a "PATCH=%%d+1"  # Увеличиваем PATCH на 1
        set "NEW_VERSION=!MAJOR!.!MINOR!.!PATCH!"
    )
)

:: Обновляем .csproj (если версия найдена)
if defined NEW_VERSION (
    powershell -Command "(Get-Content '%VERSION_FILE%') -replace '<VersionPrefix>!CURRENT_VERSION!<\/VersionPrefix>', '<VersionPrefix>!NEW_VERSION!<\/VersionPrefix>' | Set-Content '%VERSION_FILE%'"
    echo [INFO] Version updated from !CURRENT_VERSION! to !NEW_VERSION!
) else (
    echo [ERROR] Failed to parse/update version
    exit /b 1
)
goto :EOF

:: ========================
:: Основной скрипт
:: ========================
:: Проверка переменных окружения
if "%GITHUB_TOKEN%"=="" (
    echo [ERROR] GITHUB_TOKEN environment variable not found
    pause
    exit /b 1
)

:: Очистка артефактов
if exist "%OUTPUT_DIR%" rmdir /s /q "%OUTPUT_DIR%"
mkdir "%OUTPUT_DIR%"

:: Шаг 1: Увеличиваем версию
call :IncrementVersion

:: Шаг 2: Сборка пакета
echo [INFO] Building package v!NEW_VERSION!...
dotnet pack "%PROJECT_PATH%" --configuration Release --output "%OUTPUT_DIR%" --include-symbols -p:SymbolPackageFormat=snupkg
if %errorlevel% neq 0 (
    echo [ERROR] Build failed
    pause
    exit /b 1
)

:: Шаг 3: Публикация с повторами
set "SUCCESS=0"
for %%f in ("%OUTPUT_DIR%\*.nupkg") do (
    set "ATTEMPT=1"
    :retry_push
    echo [ATTEMPT !ATTEMPT!] Publishing %%~nxf...
    dotnet nuget push "%%f" --source "%NUGET_SOURCE%" --api-key %GITHUB_TOKEN% --skip-duplicate
    
    if %errorlevel% equ 0 (
        set "SUCCESS=1"
        echo [SUCCESS] Published successfully
    ) else if !ATTEMPT! lss %MAX_RETRIES% (
        set /a "ATTEMPT+=1"
        timeout /t %RETRY_DELAY% /nobreak >nul
        goto retry_push
    )
)

:: Финал
if %SUCCESS% equ 1 (
    echo [SUCCESS] Package v!NEW_VERSION! published to GitHub Packages!
) else (
    echo [ERROR] Failed after %MAX_RETRIES% attempts
)
pause