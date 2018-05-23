#ifndef __fsm_dump__
#define __fsm_dump__


typedef struct VCD *HVCD;

HVCD vcdOpen(const char *fileName);
void vcdClose(HVCD hVcd);

void vcdDefineSignal(HVCD hVcd, const char *id, const char *name);

void vcdWriteTime(HVCD hVcd, int time);
void vcdWriteSignal(HVCD hVcd, const char *id, int value);


#endif // __fsm_dump__