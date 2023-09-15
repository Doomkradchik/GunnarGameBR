using Characters.EffectSystem;
using UnityEngine;

namespace Characters.WeaponSystem
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private VFXTransforms weaponVFXTransforms;
        public EffectData EffectData => new EffectData(damage,0,0,0,0,this.GetType());
        public VFXTransforms VFXTransforms => weaponVFXTransforms;
    }
}