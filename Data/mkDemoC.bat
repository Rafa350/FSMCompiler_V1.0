@echo off
"..\bin\x86\Debug\FsmCompiler.exe" demoC.xsm /G:C /P:StateCodeFileName=fsmDemoC.c /P:StateHeaderFileName=fsmDemoC.h
gcc fsm.c fsmDemoC.c -o demo
pause