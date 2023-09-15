using Characters.EffectSystem;

namespace Characters.AbilitiesSystem.Declaration
{
    public class Fury : IAbilityCommand
    {
        public EffectData GetEffectData()
        {
            return new EffectData( 0, 0,0,0,0, this.GetType());
        }
        public void Apply(Abilities script)
        {
            script.Fury();
        }
    }
}