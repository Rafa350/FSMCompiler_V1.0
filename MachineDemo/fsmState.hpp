#ifndef __FSMSTATE_HPP
#define __FSMSTATE_HPP


#include "fsmDefines.hpp"


class RestartState: public eosState {
    public:
        RestartState(eosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class WaitTriggerStartState: public eosState {
    public:
        WaitTriggerStartState(eosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};

class WaitTriggerDelayState: public eosState {
    public:
        WaitTriggerDelayState(eosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};

class WaitTriggerState: public eosState {
    public:
        WaitTriggerState(eosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};

class ArmUpStartState: public eosState {
    public:
        ArmUpStartState(eosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ArmUpMoveState: public eosState {
    public:
        ArmUpMoveState(eosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ArmUpEndState: public eosState {
    public:
        ArmUpEndState(eosMachine *machine);
    protected:
        void onEnter();
        void onExit();
        void onEvent(unsigned eventId);
};

class ArmUpState: public eosState {
    public:
        ArmUpState(eosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ArmDownStartState: public eosState {
    public:
        ArmDownStartState(eosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ArmDownEndState: public eosState {
    public:
        ArmDownEndState(eosMachine *machine);
    protected:
        void onEnter();
        void onExit();
        void onEvent(unsigned eventId);
};

class ArmDownState: public eosState {
    public:
        ArmDownState(eosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};

class PrintLabelStartState: public eosState {
    public:
        PrintLabelStartState(eosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class PrintLabelPrintState: public eosState {
    public:
        PrintLabelPrintState(eosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class PrintLabelEndState: public eosState {
    public:
        PrintLabelEndState(eosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class PrintLabelState: public eosState {
    public:
        PrintLabelState(eosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};

class ApplyByContactStartState: public eosState {
    public:
        ApplyByContactStartState(eosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ApplyByContactApplyState: public eosState {
    public:
        ApplyByContactApplyState(eosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ApplyByContactEndState: public eosState {
    public:
        ApplyByContactEndState(eosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ApplyByContactState: public eosState {
    public:
        ApplyByContactState(eosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};

class ErrorStartState: public eosState {
    public:
        ErrorStartState(eosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ErrorState: public eosState {
    public:
        ErrorState(eosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};



#endif
