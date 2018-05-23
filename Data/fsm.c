#include "fsm.h"
#include "fsm_StartControl_events.h"
#include <stdlib.h>
#include <stdio.h>


extern const MachineDescriptor machine;


/// ----------------------------------------------------------------------
/// \brief Comprova si pot procesar la transicio.
/// \param td: Descriptor de la transicio.
/// \param context: Context de la maquina.
/// \return True si es pot procesar, false en cas contrari.
///
static bool CheckGuard(const TransitionDescriptor *td, Context *context) {
    
    if (td->guard == NULL)
        return true;
    else 
        return td->guard(context);
}


/// ----------------------------------------------------------------------
/// \brief Executa l'accio d'entrada del estat.
/// \param sd: Descriptor de l'estat.
/// \param context: Contex de la maquina.
///
static void DoEnterAction(const StateDescriptor *sd, Context *context) {
    
    if (sd->enterAction != NULL)
        sd->enterAction(context);
}

/// ----------------------------------------------------------------------
/// \brief Executa l'accio de sortida del estat.
/// \param sd: Descriptor de l'estat.
/// \param context: Contex de la maquina.
///
static void DoExitAction(const StateDescriptor *sd, Context *context) {
    
    if (sd->exitAction != NULL)
        sd->exitAction(context);
}


/// ----------------------------------------------------------------------
/// \brief Executa l'accio de la transicio.
/// \param td: Descriptor de la transicio.
/// \param context: Contex de la maquina.
///
static void DoTransitionAction(const TransitionDescriptor *td, Context *context) {
    
    if (td->action != NULL)
        td->action(context);
}


/// ----------------------------------------------------------------------
/// \brief Inicialitza el bloc de control de la maquina.
/// \param: El bloc de control a inicialitzar.
/// \return Adressa del bloc de control.
///
FSM *fsmInitialize(FSM *fsm) {
    
    fsm->machine = &machine;
    fsm->state = machine.start;
    
    return fsm;
}


/// ----------------------------------------------------------------------
/// \brief Procesa un event.
/// \param FSM: Bloc de control de la maquina.
/// \param event: L'event a p`rocesar.
///
void fsmHandleEvent(FSM *fsm, Event event) {
    
    if ((fsm->state < fsm->machine->maxStates) && (event < fsm->machine->maxEvents)) {

        StateDescriptor const *sd = &fsm->machine->states[fsm->state];
        if (sd != NULL) {
            for (uint8_t i = sd->transitionOffset; 
                 i < sd->transitionOffset + sd->transitionCount; 
                 i++) {

                    TransitionDescriptor const *td = &fsm->machine->transitions[i];
                    
                    if ((td->event == event) && CheckGuard(td, NULL)) {

                        State nextState = td->nextState;
                        
                        if (fsm->state != nextState)
                            DoExitAction(sd, NULL);

                        DoTransitionAction(td, NULL);

                        if (fsm->state != nextState)
                            DoEnterAction(&fsm->machine->states[nextState], NULL);
                        
                        fsm->state = nextState;
                        return;
                    }
            }
        }
    }
}


// -----------------------------------------------------------------------


typedef struct {
    int tick;
    uint8_t event;
} Tick;

static Tick ticks[] = {
    {-1, 0}
};


uint16_t varGet(uint8_t var) {
    
    return 0;
}

void varSet(uint8_t var, uint16_t value) {
    
}

void timStart(uint8_t tim, uint16_t time) {
    
}

void timStop(uint8_t tim) {
    
}

uint8_t trigger;


int main(char *argv[], int argc) {

    FSM fsm;
    
    fsmInitialize(&fsm);
    
    for (int tick = 0; ticks[tick].tick != -1; tick++) {
       if (ticks[0].tick == tick) {
           fsmHandleEvent(&fsm, ticks[tick].event);
       }
    }
        
    return 0;
}
