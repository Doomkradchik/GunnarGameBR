using System.Threading.Tasks;
using Characters.Animations;
using Characters.Sounds;
using Characters.InteractableSystems;
using Characters.Player;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class ManaShield : AbilityBase, IInit<Impenetrable>
    {
        private event Impenetrable _impenetrable;
        public ManaShield(){}
        public ManaShield(IAnimationCommand animation, IAudioCommand audio, View view, VFXTransforms vfxTransforms) : base(
            animation, audio, view, vfxTransforms)
        {
        }

        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null) return;
            var effect = GameObject.Instantiate(_vfxEffect, _vfxTransforms.Center);
            effect.SetLifeTime(6f);
            OnImpenetrable();
            CanSkip = true;
        }

        private async void OnImpenetrable()
        {
            _impenetrable?.Invoke(true);
            await Task.Delay(SecondToMilliseconds(6));
            _impenetrable?.Invoke(false);
        }

        public override void Tick(float tickTime)
        {
        }

        public void Subscribe(Impenetrable subscriber)
        {
            _impenetrable += subscriber;
        }

        public void Unsubscribe(Impenetrable unsubscriber)
        {
            _impenetrable -= unsubscriber;
        }
    }
}