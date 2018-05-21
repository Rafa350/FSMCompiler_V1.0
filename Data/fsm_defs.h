#ifndef __fsm_defs__
#define __fsm_defs__


#define VAR_TEST_ACTIVE       1
#define VAR_TEST_INTERVAL     2
#define VAR_TR_DELAY          3
#define VAR_IH_ONDELAY        4
#define VAR_IH_OFFDELAY       5

#define TIM_TEST_INTERVAL          6
#define TIM_TR_DELAY               7
#define TIM_IH_DISABLE_ON_DELAY    8
#define TIM_IH_DISABLE_OFF_DELAY   9

#define varGet(a)             __varGet(a)
#define varSet(a, b)          __varSet(a, b)

extern uint8_t trigger;

extern void timStart(uint8_t id, uint16_t time);
extern void timStop(uint8_t id);

extern uint16_t varGet(uint8_t var);
extern void varSet(uint8_t, uint16_t);


#endif // __fsm_defs__