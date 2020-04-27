# Compilador de XSM a C
cgen = C:\Users\Rafael\Documents\Projectes\Net\FSMCompiler\bin\Debug\FsmCompiler.exe
cgen_opt = /G:C 
cgen_opt_code = $(cgen_opt) /P:OutputType=MachineCode
cgen_opt_header = $(cgen_opt) /P:OutputType=MachineHeader /P:IncludeFileName=Services/Engine/appMachine.h

# Compilador de XSM a DOT
dotgen = C:\Users\Rafael\Documents\Projectes\Net\FSMCompiler\bin\Debug\FsmCompiler.exe
dotgen_opt = /G:DOT

# Compilador de DOT a PDF: 
dot = C:\PROGRA~2\Graphviz2.38\bin\dot.exe
dot_opt = -Tpdf

targets = \
	MainMachine.c \
	MainMachine.h \
	MainMachine.pdf


.SUFFIXES: .xsm .dot .c .pdf
.PHONY: all clean

all: $(targets)

clean:
	rm -rf *.pdf *.dot *.c *.h

%Machine.c: %.xsm
	$(cgen) $(cgen_opt_code) /P:MachineCodeFileName=$*Machine.c /P:MachineHeaderFileName=$*Machine.h $*.xsm
	
%Machine.h: %.xsm
	$(cgen) $(cgen_opt_header) /P:MachineHeaderFileName=$*Machine.h $*.xsm

%Machine.dot: %.xsm
	$(dotgen) $(dotgen_opt)	$*.xsm
	
.dot.pdf:
	$(dot) $(dot_opt) $*.dot -o $*.pdf
