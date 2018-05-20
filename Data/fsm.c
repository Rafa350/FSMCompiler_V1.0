#include "fsmDemoC.h"


extern TransitionDescriptor transitions[];
extern StateDescriptor states[];


static StateDescriptor *findStateDescriptor(State state) {
    
    return &states[state];
}

State fsmHandleEvent(State state, Event event) {
    
    if ((state < MAX_STATES) && (event < MAX_EVENTS)) {

        StateDescriptor *sd = findStateDescriptor(state);
        if (sd != NULL) {
            for (uint8_t i = sd->transitionOffset; 
                 i < sd->transitionOffset + sd->transitionCount; 
                 i++) {
                
            }
        }
    }
    
    return state;
}


