using System;
using Characters.Player;

namespace Characters.EffectSystem
{
    [Serializable]
    public class Applyer
    {
        private CharacterData _characterData;
        public void RechangeCharacterDataValues(EffectData effectData)
        {
            _characterData.Damaged(effectData.HealthDamage);
            _characterData.AddEnergy(effectData.EnergyAdd);
            _characterData.AddHealth(effectData.HealthAdd);
            _characterData.AddMana(effectData.ManaAdd);
        }

        public void Initialize(CharacterData data)
        {
            _characterData = data;
        }
    }
}