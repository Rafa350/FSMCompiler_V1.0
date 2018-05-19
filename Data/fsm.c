#include "fsmDemoC.h"


extern MachineDescriptor machine;


State fsmHandleEvent(State state, Event event) {
    
    if ((state < MAX_STATES) && (event < MAX_EVENTS)) {
       
        int action = machine.states[state].events[event].action;
    }
    
    return state;
}


