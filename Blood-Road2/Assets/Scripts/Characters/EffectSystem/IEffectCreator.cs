using UnityEngine;

namespace Characters.EffectSystem
{
    public interface IEffectCreator
    {
        public void Instantiate(VFXEffect vfxEffect, Vector3 position, Transform parent, int lifetime);
        public void AddPassiveSkills(int value);
        public void AddNegativeSkills();
        public void Triggered();
    }
}