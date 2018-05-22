#include "fsm.h"


extern const MachineDescriptor machine;

static State state;


static bool CheckGuard(TransitionDescriptor *td, Context *context) {
    
    if (td->guard == NULL)
        return true;
    else 
        return td->guard(context);
}

static void DoEnterAction(StateDescriptor *sd, Context *context) {
    
    if (sd->enterAction != NULL)
        sd->enterAction(context);
}

static void DoExitAction(StateDescriptor *sd, Context *context) {
    
    if (sd->exitAction != NULL)
        sd->exitAction(context);
}

static void DoTransitionAction(TransitionDescriptor *td, Context *context) {
    
    if (td->action != NULL)
        td->action(context);
}

State fsmHandleEvent(State state, Event event) {
    
    if ((state < machine.maxStates) && (event < machine.maxEvents)) {

        StateDescriptor const *sd = &machine.states[state];
        if (sd != NULL) {
            for (uint8_t i = sd->transitionOffset; 
                 (i < sd->transitionOffset + sd->transitionCount) && !done; 
                 i++) {

                    TransitionDescriptor const *td = &machine.transitions[i];
                    
                    if ((td->event == event) && CheckGuard(td, NULL)) {

                        nextState = td->nextState;
                        
                        if (state != nextState)
                            DoExitAction(sd, NULL);

                        DoTransitionAction(td, NULL);

                        if (state != nextState)
                            DoEnterAction(&states[nextState]);
                        
                        return nextState;
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
    
    state = fsmHandleEvent(state, event);
}


