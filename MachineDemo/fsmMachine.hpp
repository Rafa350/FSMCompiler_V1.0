#ifndef __FSMMACHINE_HPP
#define __FSMMACHINE_HPP


#include "fsmDefines.hpp"


class MyMachine: public eosMachine {
    private:
        eosState *states[21];
    public:
        MyMachine(eosContext *context);
        eosState *getState(unsigned stateId) const { return state[stateId]; }
};


#endif
