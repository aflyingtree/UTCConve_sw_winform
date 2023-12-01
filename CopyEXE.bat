@echo off
chcp 65001

setlocal

set exe_name=UTC时间戳互转.exe
set new_exe_name=检验GPRS数据透传.exe
set bin_dir=%~dp0bin\Debug
set parent_dir=%~dp0



cd /d "%~dp0"

rem Copy the compiled binary file to the parent directory
copy /y "%bin_dir%\%exe_name%" "%parent_dir%"





echo Upgrade completed successfully. & pause & exit /b 0
