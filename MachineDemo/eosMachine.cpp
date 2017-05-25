#include "stdafx.h"
#include "fsmDefines.hpp"


eosMachine::eosMachine(eosContext *context) {

    this->context = context;
}


void eosMachine::raiseEvent(unsigned eventId, unsigned delay) {
}


void eosMachine::setState(unsigned stateId) {
}


void eosMachine::popState(){
}


void eosMachine::raiseEvent(unsigned eventId, unsigned delay) {
}


void eosMachine::acceptEvent(unsigned eventId) {

    unsigned currentStateId = 0;
    eosState *state = states[currentStateId];
    state->onEvent(eventId);
}
