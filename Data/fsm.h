#ifndef __fsm__
#define __fsm__

#include <stdint.h>
#include <stdbool.h>
#include <stdlib.h>

typedef uint8_t State;
typedef uint8_t Event;
typedef void *Context;
typedef void (*Action)(Context *context);
typedef bool (*Guard)(Context *context);

typedef struct {
    Event event;
    State nextState;
    const Guard guard;
    const Action action;
} TransitionDescriptor;

typedef struct {
    State state;
    const Action enterAction;
    const Action exitAction;
    uint8_t transitionOffset;
    uint8_t transitionCount;
} StateDescriptor;

typedef struct {
    State start;
    uint8_t maxStates;
    uint8_t maxEvents;
    const StateDescriptor *states;
    const TransitionDescriptor *transitions;
} MachineDescriptor;

typedef struct {
    const MachineDescriptor *machine;
    State state;
} FSM;

#endif // __fsm__