using Characters.EffectSystem;

namespace Characters.AbilitiesSystem
{
    
    public interface IAbilityCommand
    {
        public EffectData GetEffectData();
        public void Apply(Abilities script);
    }
}