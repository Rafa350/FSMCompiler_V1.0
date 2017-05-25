#include "fsmDefines.hpp"
#include "fsmEventId.hpp"
#include "fsmStateId.hpp"
#include "fsmMachine.hpp"
#include "fsmState.hpp"


RestartState::RestartState(eosMachine *machine):
    eosState(machine) {
}

void RestartState::onEnter() {

    doPistonUp();
    doAirJetOff();
    doAirAssistOff();
    doVacuumOff();
    doSignalErrorOff();
    doSignalWorkingOff();
}

WaitTriggerStartState::WaitTriggerStartState(eosMachine *machine):
    eosState(machine) {
}

void WaitTriggerStartState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_TriggerChanged:
            raiseEvent(EV_TriggerDelayDone, VAR_TriggerDelay);
            setState(ST_WaitTriggerDelay);
            break;
    }
}

WaitTriggerDelayState::WaitTriggerDelayState(eosMachine *machine):
    eosState(machine) {
}

void WaitTriggerDelayState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_TriggerDelayDone:
            setState(ST_PrintLabelStart);
            break;
    }
}

WaitTriggerState::WaitTriggerState(eosMachine *machine):
    eosState(machine) {
}

ArmUpStartState::ArmUpStartState(eosMachine *machine):
    eosState(machine) {
}

void ArmUpStartState::onEnter() {

    notify(SIG_ArmUpStartActivity);
    raiseEvent(EV_ArmUpPreDelayDone, VAR_ArmUpPreDelay);
    ***ON_ENTER Comu per tots el subestats***
}

void ArmUpStartState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_ArmUpPreDelayDone:
            setState(ST_ArmUpMove);
            break;
    }
}

ArmUpMoveState::ArmUpMoveState(eosMachine *machine):
    eosState(machine) {
}

void ArmUpMoveState::onEnter() {

    doPistonUp();
    raiseEvent(EV_ArmUpTimeOutDone, VAR_ArmUpTimeout);
    ***ON_ENTER Comu per tots el subestats***
}

void ArmUpMoveState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_PistonTopChanged:
            setState(ST_ArmUpEnd);
            break;

        case EV_ArmUpTimeOutDone:
            notify(SIG_ArmUpTimeout);
            setState(ST_ErrorStart);
            break;
    }
}

ArmUpEndState::ArmUpEndState(eosMachine *machine):
    eosState(machine) {
}

void ArmUpEndState::onEnter() {

    raiseEvent(EV_ArmUpPostDelayDone, VAR_ArmUpPostDelay);
    ***ON_ENTER Comu per tots el subestats***
}

void ArmUpEndState::onExit() {

    notify(SIG_ArmUpEndActivity);
}

void ArmUpEndState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_ArmUpPostDelayDone:
            setState(ST_WaitTriggerStart);
            break;
    }
}

ArmUpState::ArmUpState(eosMachine *machine):
    eosState(machine) {
}

void ArmUpState::onEnter() {

    ***ON_ENTER Comu per tots el subestats***
}

ArmDownStartState::ArmDownStartState(eosMachine *machine):
    eosState(machine) {
}

void ArmDownStartState::onEnter() {

    notify(SIG_ArmDownStartActivity);
    raiseEvent(EV_ArmDownPreDelayDone, VAR_ArmDownPreDelay);
}

void ArmDownStartState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_ArmDownPreDelayDone:
            setState(ST_ArmDownEnd);
            break;
    }
}

ArmDownEndState::ArmDownEndState(eosMachine *machine):
    eosState(machine) {
}

void ArmDownEndState::onEnter() {

    doPistonDown();
    raiseEvent(EV_ArmDownPostDelayDone, VAR_ArmDownPostDelay);
}

void ArmDownEndState::onExit() {

    notify(SIG_ArmDownEndActivity);
}

void ArmDownEndState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_ArmDownPostDelayDone:
            setState(ST_ApplyByContactStart);
            break;
    }
}

ArmDownState::ArmDownState(eosMachine *machine):
    eosState(machine) {
}

PrintLabelStartState::PrintLabelStartState(eosMachine *machine):
    eosState(machine) {
}

void PrintLabelStartState::onEnter() {

    raiseEvent(EV_PrintLabelPreDelayDone, VAR_PrintLabelPreDelay);
}

void PrintLabelStartState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_PrintLabelPreDelayDone:
            setState(ST_PrintLabelPrint);
            break;
    }
}

PrintLabelPrintState::PrintLabelPrintState(eosMachine *machine):
    eosState(machine) {
}

void PrintLabelPrintState::onEnter() {

    if (!inpGet(INP_PrinterError)) {
        doPrintLabel();
    }
    if (!inpGet(INP_PrinterError)) {
        raiseEvent(EV_AirAssistDelayDone, VAR_AirAssistDelay);
    }
    if (!inpGet(INP_PrinterError)) {
        raiseEvent(EV_VacuumDelayDone, VAR_VacuumDelay);
    }
    if (!inpGet(INP_PrinterError)) {
        raiseEvent(EV_PrintLabelTimeOutDone, VAR_PrintLabelTimeOut);
    }
}

void PrintLabelPrintState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_AirAssistDelayDone:
            doAirAssistOn();
            break;

        case EV_VacuumDelayDone:
            doVacuumOn();
            break;

        case EV_PrintLabelTimeOutDone:
            doAirAssistOff();
            doVacuumOff();
            setState(ST_ErrorStart);
            break;

        case EV_INP_LabelReady:
            doAirAssistOff();
            doVacuumOn();
            setState(ST_PrintLabelEnd);
            break;

        case EV_INP_PrinterError:
            doAirAssistOff();
            doVacuumOff();
            setState(ST_ErrorStart);
            break;
    }
}

PrintLabelEndState::PrintLabelEndState(eosMachine *machine):
    eosState(machine) {
}

void PrintLabelEndState::onEnter() {

    raiseEvent(EV_PrintLabelPostDelayDone, VAR_PrintLabelPostDelay);
}

void PrintLabelEndState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_PrintLabelPostDelayDone:
            setState(ST_ArmDownStart);
            break;
    }
}

PrintLabelState::PrintLabelState(eosMachine *machine):
    eosState(machine) {
}

ApplyByContactStartState::ApplyByContactStartState(eosMachine *machine):
    eosState(machine) {
}

void ApplyByContactStartState::onEnter() {

    raiseEvent(EV_ApplyByContactPreDelayDone, VAR_ApplyByContactPreDelay);
}

void ApplyByContactStartState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_ApplyByContactPreDelayDone:
            setState(ST_ApplyByContactApply);
            break;
    }
}

ApplyByContactApplyState::ApplyByContactApplyState(eosMachine *machine):
    eosState(machine) {
}

void ApplyByContactApplyState::onEnter() {

    raiseEvent(EV_ApplyByContactTimeOutDone, VAR_ApplyByContactTimeOut);
}

void ApplyByContactApplyState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_ApplyByContactTimeOutDone:
            doVacuumOff();
            setState(ST_ErrorStart);
            break;

        case EV_PistonBottomChanged:
            doSignalWorkingPulse();
            doVacuumOff();
            doAirJetPulse();
            setState(ST_ApplyByContactEnd);
            break;
    }
}

ApplyByContactEndState::ApplyByContactEndState(eosMachine *machine):
    eosState(machine) {
}

void ApplyByContactEndState::onEnter() {

    raiseEvent(EV_ApplyByContactPostDelayDone, VAR_ApplyByContactPostDelay);
}

void ApplyByContactEndState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_ApplyByContactPostDelayDone:
            setState(ST_ArmUpStart);
            break;
    }
}

ApplyByContactState::ApplyByContactState(eosMachine *machine):
    eosState(machine) {
}

ErrorStartState::ErrorStartState(eosMachine *machine):
    eosState(machine) {
}

void ErrorStartState::onEnter() {

    doSignalErrorOn();
    doSignalWorkingOff();
}

void ErrorStartState::onEvent(unsigned eventId) {

    switch (eventId) {

        case EV_INP_Restart:
            doSignalErrorOff();
            doPistonUp();
            setState(ST_WaitTriggerStart);
            break;
    }
}

ErrorState::ErrorState(eosMachine *machine):
    eosState(machine) {
}

