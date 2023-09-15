using System;
using Better.Attributes.Runtime.Select;
using Characters.AbilitiesSystem;
using Characters.EffectSystem;
using UnityEngine;

namespace MapSystem.Structs
{
    [Serializable]
    public struct Ability
    {
        [field: SelectImplementation(typeof(IAbilityCommand))] [field: SerializeReference] [field: SerializeField] public IAbilityCommand AbilityCommand { get; private set; }

        [field: SerializeField] public int Cost { get; private set; }

        [field: SerializeField] public EffectData EffectData { get; private set; }


        public Ability(Ability ability)
        {
            Cost = ability.Cost;
            EffectData = new EffectData(ability.EffectData);
            AbilityCommand = ability.AbilityCommand;
        }

        public Ability(int cooldown, int cost, EffectData effectData, IAbilityCommand abilityCommand)
        {
            Cost = cost;
            EffectData = new EffectData(effectData);
            AbilityCommand = abilityCommand;
        }
    }
}