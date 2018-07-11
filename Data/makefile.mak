# Compilador de XSM a C
cgen = ..\bin\x86\Release\FsmCompiler.exe
cgen_options = /G:C

# Compilador de XSM a DOT
dotgen = ..\bin\x86\Release\FsmCompiler.exe
dotgen_options = /G:DOT

# Compilador de DOT a PDF: 
dot = $(ProgramFiles)\Graphviz2.38\bin\dot.exe
dot_options = -Tpdf

targets = \
       fsm_Demo.c \
	   fsm_Demo.pdf

.SUFFIXES: .xsm .dot .c .pdf
.PHONY: all clean

all: $(targets)

clean:
	rm *.pdf

fsm_%.c: %.xsm
	$(cgen) $(cgen_options) $*.xsm
	
fsm_%.dot: %.xsm
	$(dotgen) $(dotgen_options)	$*.xsm
	
.dot.pdf:
	$(dot) $(dot_options) $*.dot -o $*.pdf
