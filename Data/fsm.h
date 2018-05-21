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
  State next;
  const Guard guard;
  const Action action;
} TransitionDescriptor;

typedef struct {
  State state;
  const Action enter;
  const Action exit;
  uint8_t transitionOffset;
  uint8_t transitionCount;
} StateDescriptor;

typedef struct{
  State start;
} MachineDescriptor;


#endif // __fsm__