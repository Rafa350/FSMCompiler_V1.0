@echo off
"..\bin\x86\Debug\FsmCompiler.exe" demoC.xsm /G:C 
"C:\Program Files\Graphviz2.38\bin\dot.exe" -Tpng -oStartControl.png fsm_StartControl.dot
//gcc fsm.c fsm_dump.c fsm_StartControl.c -o demo
pause