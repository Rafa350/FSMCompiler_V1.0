#include "stdafx.h"
#include "fsmDefines.hpp"


eosState::eosState(eosMachine *machine, eosState *parent) {

    this->machine = machine;
    this->parent = parent;
}


void eosState::onEnter() {
}


void eosState::onExit() {
}


void eosState::onEvent(unsigned eventId) {
}


void eosState::raiseEvent(unsigned eventId, unsigned delay) {
}


void eosState::setState(unsigned stateId) {
}


void eosState::popState() {
}
