using Characters.Animations;
using Characters.Sounds;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class InductionCoil : AbilityBase
    {
        public InductionCoil(){}
        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null) return;
            var effect = GameObject.Instantiate(_vfxEffect, _vfxTransforms.Down);
            effect.SetLifeTime(3f);
            CanSkip = true;
        }


        public override void Tick(float tickTime)
        {
        }

        public InductionCoil(IAnimationCommand animation, IAudioCommand audio,  MapSystem.Structs.View view, VFXTransforms vfxTransforms) : base(
            animation, audio, view, vfxTransforms)
        {
        }
    }
}