using System;

namespace MapSystem.Structs
{
    public struct StateCharacterKey
    {
        public int ID { get; private set; }
        public Type State { get; private set; }
        public Type AbilityCommand { get; private set; }

        public StateCharacterKey(int id, Type state, Type abilityCommand)
        {
            ID = id;
            State = state;
            AbilityCommand = abilityCommand;
        }

        public void SetID(int id) => ID = id;


        public void SetState(Type state) => State = state;
        public void SetAbilityCommand(Type command) => AbilityCommand = command;
    }
}