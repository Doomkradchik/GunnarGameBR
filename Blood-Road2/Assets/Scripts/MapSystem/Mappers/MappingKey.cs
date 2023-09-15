using System.Collections.Generic;
using MapSystem.Structs;

namespace MapSystem.Mappers
{
    public abstract class MappingKey<TValue> : MapperBase<StateCharacterKey, TValue>,
        IEqualityComparer<StateCharacterKey>
    {
        public MappingKey()
        {
            _dictionary = new Dictionary<StateCharacterKey, TValue>(this);
        }

        public bool Equals(StateCharacterKey x, StateCharacterKey y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(StateCharacterKey obj)
        {
            int ability = obj.AbilityCommand != null ? obj.AbilityCommand.GetHashCode() : 1;
            int state = obj.State != null ? obj.State.GetHashCode() : 1;
            var id = obj.ID.GetHashCode();
            return ability * state * id;
        }
    }
}