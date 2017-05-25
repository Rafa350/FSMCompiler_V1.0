#include "stdafx.h"
#include "fsmDefines.hpp"
#include "fsmEvent.hpp"
#include "fsmMachine.hpp"
#include "fsmState.hpp"


int _tmain(int argc, _TCHAR* argv[]) {

    // Crea la cua d'events
    //
    std::queue<unsigned> eventQueue;

    // Crea la maquina d'estat
    //
    eosContext *context = new eosContext();
    eosMachine *machine = new MyMachine(context);

    bool done = false;
    while (!done) {

        // Genera els events extern
        //


        // Procesa la cua d'events
        //
        if (!eventQueue.empty()) {
            unsigned ev = eventQueue.front();
            eventQueue.pop();

            machine->acceptEvent(ev);
        }
    }

	return 0;
}

