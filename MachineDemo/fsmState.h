#ifndef __FSMSTATE_H
#define __FSMSTATE_H


#include "fsmDefines.h"


class RestartState: public EosState {
    public:
        RestartState(EosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class WaitTriggerStartState: public EosState {
    public:
        WaitTriggerStartState(EosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};

class WaitTriggerDelayState: public EosState {
    public:
        WaitTriggerDelayState(EosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};

class WaitTriggerState: public EosState {
    public:
        WaitTriggerState(EosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};

class ArmUpStartState: public EosState {
    public:
        ArmUpStartState(EosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ArmUpMoveState: public EosState {
    public:
        ArmUpMoveState(EosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ArmUpEndState: public EosState {
    public:
        ArmUpEndState(EosMachine *machine);
    protected:
        void onEnter();
        void onExit();
        void onEvent(unsigned eventId);
};

class ArmUpState: public EosState {
    public:
        ArmUpState(EosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ArmDownStartState: public EosState {
    public:
        ArmDownStartState(EosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ArmDownEndState: public EosState {
    public:
        ArmDownEndState(EosMachine *machine);
    protected:
        void onEnter();
        void onExit();
        void onEvent(unsigned eventId);
};

class ArmDownState: public EosState {
    public:
        ArmDownState(EosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};

class PrintLabelStartState: public EosState {
    public:
        PrintLabelStartState(EosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class PrintLabelPrintState: public EosState {
    public:
        PrintLabelPrintState(EosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class PrintLabelEndState: public EosState {
    public:
        PrintLabelEndState(EosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class PrintLabelState: public EosState {
    public:
        PrintLabelState(EosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};

class ApplyByContactStartState: public EosState {
    public:
        ApplyByContactStartState(EosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ApplyByContactApplyState: public EosState {
    public:
        ApplyByContactApplyState(EosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ApplyByContactEndState: public EosState {
    public:
        ApplyByContactEndState(EosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ApplyByContactState: public EosState {
    public:
        ApplyByContactState(EosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};

class ErrorStartState: public EosState {
    public:
        ErrorStartState(EosMachine *machine);
    protected:
        void onEnter();
        void onEvent(unsigned eventId);
};

class ErrorState: public EosState {
    public:
        ErrorState(EosMachine *machine);
    protected:
        void onEvent(unsigned eventId);
};



#endif
