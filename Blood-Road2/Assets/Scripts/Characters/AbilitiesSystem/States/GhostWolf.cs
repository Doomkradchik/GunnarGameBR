using Characters.Animations;
using Characters.Sounds;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class GhostWolf : AbilityBase
    {
        public GhostWolf(){}

        public GhostWolf(IAnimationCommand animation, IAudioCommand audio, View view, VFXTransforms vfxTransforms) : base(animation, audio, view, vfxTransforms)
        {
        }

        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null) return;
            var effect = GameObject.Instantiate(_vfxEffect,_vfxTransforms.Down.position + (Vector3.left*2), Quaternion.identity);
            effect.SetLifeTime(23);
          //  effect.GetComponent<WolfGhost>().SetMainInteractable(_vfxTransforms.Character);
            CanSkip = true;
        }

        public override void Tick(float tickTime)
        {
            
        }
    }
}