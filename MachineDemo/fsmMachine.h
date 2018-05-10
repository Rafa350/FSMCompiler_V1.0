#ifndef __FSMMACHINE_H
#define __FSMMACHINE_H


#include "fsmDefines.h"


class MyMachine: public EosMachine {
    private:
        EosState *states[21];
    public:
        MyMachine(EosContext *context);
        EosState *getState(unsigned stateId) const { return state[stateId]; }
};


#endif
