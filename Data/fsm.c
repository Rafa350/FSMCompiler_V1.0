#include "fsm_StartControl.h"


extern const TransitionDescriptor transitions[];
extern const StateDescriptor states[];
extern const MachineDescriptor machine;

static State state;


State fsmHandleEvent(State state, Event event) {
    
    if ((state < MAX_STATES) && (event < MAX_EVENTS)) {

        StateDescriptor const *sd = &states[state];
        if (sd != NULL) {
            for (uint8_t i = sd->transitionOffset; 
                 i < sd->transitionOffset + sd->transitionCount; 
                 i++) {

                    TransitionDescriptor const *td = &transitions[i];
                    
                    if (td->event == event) {
                    
                        if ((td->guard == NULL) || 
                            ((td->guard != NULL) && td->guard(NULL))) {
                            
                            if (td->action != NULL)
                                td->action(NULL);
                            
                            return td->next;
                        }
                    }
            }
        }
    }
    
    return state;
}


void fsmInitialize() {
    
    state = machine.start;
}


void fsmRun(Event event) {
    
    State oldState = state;
    State newState = fsmHandleEvent(state, event);
    if (oldState != newState) {
        states[oldState].exit(NULL);
        states[newState].enter(NULL);
    }
}


