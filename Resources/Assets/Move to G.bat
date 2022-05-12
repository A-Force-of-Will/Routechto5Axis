REM Copying Net-Hooks To G:
@Echo Off
xcopy "C:\Users\zstockton\Documents\Nhooks\RoutechToFiveAxis\bin\x64\Debug\RoutechToFiveAxis.dll" "\\thomas\eng\CAD Support\CNC\(00) NET-HOOKS\Hooks\" /E /C /I /Q /H /R /K /Y
xcopy "C:\Users\zstockton\Documents\Nhooks\RoutechToFiveAxis\Resources\FunctionTable\RoutechToFiveAxis.ft" "\\thomas\eng\CAD Support\CNC\(00) NET-HOOKS\Hooks\" /E /C /I /Q /H /R /K /Y
PAUSE