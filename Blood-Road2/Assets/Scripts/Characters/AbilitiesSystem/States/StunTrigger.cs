using Characters.Animations;
using Characters.Sounds;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class StunTrigger : AbilityBase
    {
        public StunTrigger(){}
        public StunTrigger(IAnimationCommand animation, IAudioCommand audio, View view, VFXTransforms vfxTransforms) : base(animation, audio, view, vfxTransforms)
        {
        }

        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null) return;
            var effect = GameObject.Instantiate(_vfxEffect,_vfxTransforms.Down);
            effect.SetLifeTime(1);
            CanSkip = true;
        }

        public override void Tick(float tickTime)
        {
            
        }

    }
}