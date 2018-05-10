#include "fsmDefines.h"
#include "fsmStateId.h"
#include "fsmMachine.h"
#include "fsmState.h"


MyMachine::MyMachine(EosContext *context):
    EosMachine(context) {

    states[ST_Restart] = new RestartState(this);
    states[ST_WaitTriggerStart] = new WaitTriggerStartState(this);
    states[ST_WaitTriggerDelay] = new WaitTriggerDelayState(this);
    states[ST_WaitTrigger] = new WaitTriggerState(this);
    states[ST_ArmUpStart] = new ArmUpStartState(this);
    states[ST_ArmUpMove] = new ArmUpMoveState(this);
    states[ST_ArmUpEnd] = new ArmUpEndState(this);
    states[ST_ArmUp] = new ArmUpState(this);
    states[ST_ArmDownStart] = new ArmDownStartState(this);
    states[ST_ArmDownEnd] = new ArmDownEndState(this);
    states[ST_ArmDown] = new ArmDownState(this);
    states[ST_PrintLabelStart] = new PrintLabelStartState(this);
    states[ST_PrintLabelPrint] = new PrintLabelPrintState(this);
    states[ST_PrintLabelEnd] = new PrintLabelEndState(this);
    states[ST_PrintLabel] = new PrintLabelState(this);
    states[ST_ApplyByContactStart] = new ApplyByContactStartState(this);
    states[ST_ApplyByContactApply] = new ApplyByContactApplyState(this);
    states[ST_ApplyByContactEnd] = new ApplyByContactEndState(this);
    states[ST_ApplyByContact] = new ApplyByContactState(this);
    states[ST_ErrorStart] = new ErrorStartState(this);
    states[ST_Error] = new ErrorState(this);

    start(states[ST_ArmUpStart]);
}
