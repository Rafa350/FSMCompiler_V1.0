@echo off
"..\bin\x86\Debug\FsmCompiler.exe" demoC.xsm /G:C 
gcc fsm.c fsm_dump.c fsm_StartControl.c -o demo
pause