using System.Threading.Tasks;
using Characters.Sounds;
using Better.UnityPatterns.Runtime.StateMachine;
using Characters.Animations;
using JetBrains.Annotations;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class Stun : AbilityBase
    {
        private BaseState _idleState;
        private StateMachine<BaseState> _stateMachine;

        public Stun()
        {
        }

        public Stun([CanBeNull] IAnimationCommand animation, IAudioCommand audioCommand,  View view, [CanBeNull] VFXTransforms transform) :
            base(animation, audioCommand, view, transform)
        {
            _parameterName = "stun";
        }

        public override void Enter()
        {
            base.Enter();
            _animation.SetAnimation(_parameterName);
            Wait();
        }

        private async void Wait()
        {
            CanSkip = false;
            var effect = Object.Instantiate(_vfxEffect, _vfxTransforms.Up);
            effect.transform.position = _vfxTransforms.Up.position;
            effect.SetLifeTime(6);
            var milliseconds = SecondToMilliseconds(6);
            await Task.Delay(milliseconds);
            CanSkip = true;
        }

        public override void Tick(float tickTime)
        {
        }
    }
}