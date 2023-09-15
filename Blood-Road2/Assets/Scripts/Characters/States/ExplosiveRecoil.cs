using System;
using System.Threading.Tasks;
using Characters.Animations;
using Characters.Sounds;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.States
{
    public class ExplosiveRecoil : BaseState
    {
        protected float _speed;
        protected float _radius;
        protected float _distance;
        protected Vector3 _direction;
        protected int _normalMilesecondsInDieAnim;
        protected CharacterController _controller;
        protected bool _stoodUp;

        public bool IsRecoiled => _distance >= _radius && _stoodUp;

        public ExplosiveRecoil(IAnimationCommand animation, IAudioCommand audio, View stateInfo, VFXTransforms vfxTransforms,
            CharacterController characterController) : base(animation, audio, stateInfo, vfxTransforms)
        {
            _parameterName = "die";
            _controller = characterController;
        }

        public override void Enter()
        {
            base.Enter();
            _distance = 0f;
            _stoodUp = false;
            _animation.SetAnimation(_parameterName);
            _animation.SetSpeedAnimation(1f);
            _normalMilesecondsInDieAnim = SecondToMilliseconds(_animation.LengthAnimation(_parameterName));
        }

        public void SetOrigin(Vector3 origin)
        {
            _direction = (_controller.transform.position - origin).normalized;
        }

        public void SetParameters(ExplosionParameters parameters)
        {
            _radius = parameters.Radius;
            _speed = parameters.Speed;
        }

        private async void StandUp()
        {
            await Task.Delay((int)(_normalMilesecondsInDieAnim * 0.75));
            _animation.SetSpeedAnimation(-1f);
            await Task.Delay(_normalMilesecondsInDieAnim);
            _stoodUp = true;
        }

        public override void Tick(float tickTime)
        {
            var offset = _speed * tickTime;
            if (!(_distance < _radius)) return;
            _controller.Move(_direction * _speed * tickTime);
            _distance += offset;

            if (_distance >= _radius)
                StandUp();
        }
    }

    [Serializable]
    public struct ExplosionParameters
    {
        public float Speed;
        public float Radius;
    }
}
