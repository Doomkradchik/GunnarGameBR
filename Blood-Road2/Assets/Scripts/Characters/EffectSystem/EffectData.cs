using System;
using UnityEngine;

namespace Characters.EffectSystem
{
    [Serializable]
    public struct EffectData
    {
        [field: SerializeField] public int HealthDamage { get; private set; }
        [field: SerializeField] public int MagicDamage { get; private set; }
        [field: SerializeField] public int HealthAdd { get; private set; }
        [field: SerializeField] public int ManaAdd { get; private set; }
        [field: SerializeField] public int EnergyAdd { get; private set; }
        [field: SerializeField] public Type Type { get; private set; }

        public EffectData(int healthDamage, int magicDamage,
            int healthAdd, int manaAdd, int energyAdd, Type type)
        {
            HealthDamage = healthDamage;
            MagicDamage = magicDamage;
            HealthAdd = healthAdd;
            ManaAdd = manaAdd;
            EnergyAdd = energyAdd;
            Type = type;
        }

        public EffectData(EffectData data)
        {
            HealthDamage = data.HealthDamage;
            MagicDamage = data.MagicDamage;
            Type = data.Type;
            HealthAdd = data.HealthAdd;
            ManaAdd = data.ManaAdd;
            EnergyAdd = data.EnergyAdd;
        }

        public static EffectData From(PassiveData data, Type type = null)
        {
            return new EffectData( 0, 0, data.HealthAdd, data.ManaAdd, data.EnergyAdd, type);
        }
    }
}
