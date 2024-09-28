@echo off
title Instalador de Dependencias PetaFly

:: Crear directorios si no existen
echo Verificando Integridad de los Archivos de Petafly...
echo.
set "DATA_DIR=%~dp0.DATA\memory"
if not exist "%DATA_DIR%" (
    mkdir "%DATA_DIR%"
)

:: Siempre iniciar el instalador de Python
echo Iniciando Python...
start "" ".pflyreqs\python.exe"
echo.
echo Por favor, completa la instalación de Python o cierra la ventana si ya lo tienes instalado, y dale al enter.
pause
cls

:: Abrir una nueva ventana cmd en el directorio .pflyreqs
echo Iniciando Ngrok... Si ya está instalado, ciérralo cuando termines.
cd /d ".pflyreqs"
start cmd /k "ngrok.exe"
echo.
echo Por favor, completa la instalación de Ngrok o cierra la ventana si ya lo tienes instalado, y dale al enter.
echo.
echo Si no lo tienes instalado, inicia sesión en Ngrok, y pon tu clave en la consola que acaba de aparecer, y una vez lo tengas instalado todo puedes cerrar todo.
pause
cls

:: Crear el archivo de registro una vez que todo esté listo
echo Dependencias instaladas > "%DATA_DIR%\pfly_dependencies_installed.txt"