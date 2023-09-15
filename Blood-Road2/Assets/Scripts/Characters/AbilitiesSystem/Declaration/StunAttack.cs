using Characters.AbilitiesSystem.States;
using Characters.EffectSystem;

namespace Characters.AbilitiesSystem.Declaration
{
    public class StunAttack : IAbilityCommand
    {
        public EffectData GetEffectData()
        {
            return new EffectData( 0, 0,0,0,0, typeof(AttackStun));
        }
        public void Apply(Abilities script){
            script.StunAttack();
        }
    }
}