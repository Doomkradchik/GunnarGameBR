using System;

namespace Characters.AbilitiesSystem
{
    public interface IRunAbility{
        public void RunAbility(IAbilityCommand command);
        public void SetTypeAbility(Type type);
    }
}