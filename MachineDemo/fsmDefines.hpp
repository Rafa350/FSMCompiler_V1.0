#ifndef __FSM_DEFINES_HPP
#define __FSM_DEFINES_HPP


class eosState;
class eosMachine;
class eosContext;


class eosContext {
};


class eosMachine {
    private:
        eosContext *context;
        eosState *state;
    public:
        eosMachine(eosContext *context);
        void raiseEvent(unsigned eventId, unsigned delay);
        void setState(unsigned stateId);
        void popState();
        void acceptEvent(unsigned eventId);
};


class eosState {
    private:
        eosMachine *machine;
    
    public:
        eosState(eosMachine *machine);
        inline eosMachine *getMachine() const { return machine; }

    protected:
        virtual void onEnter();
        virtual void onExit();
        virtual void onEvent(unsigned eventId);

        void raiseEvent(unsigned eventId, unsigned delay);
        void setState(unsigned stateId);
        void popState();

        void doPistonUp();
        void doPistonDown();
        void doPistonStop();
        void doAirAssistOn();
        void doAirAssistOff();
        void doAirJetOn();
        void doAirJetOff();
        void doVacuumOn();
        void doVacuumOff();
        void doSignalErrorOn();
        void doSignalErrorOff();
        void doSignalWorkingOn();
        void doSignalWorkingOff();
};


#endif
