using System;
using Characters.Animations;
using Characters.Sounds;
using JetBrains.Annotations;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.Player.States
{
    public class RunToPoint : BaseState
    {
        private readonly Transform _transform;
        private CharacterController _characterController;
        private float _speed;
        private float _stopingDistance;

        public GetTarget Target { get; private set; }

        public RunToPoint(){}
        public RunToPoint(IAnimationCommand animation, IAudioCommand audio, RunToPointData data, View view,
            VFXTransforms vfxTransforms) : base(animation, audio, view, vfxTransforms)
        {
            _characterController = data.CharacterController;
            _transform = data.ThisCharacter;
            _speed = data.Speed;
            _stopingDistance = data.StopDistance;
            _parameterName = "run";
        }

        public void SetParams(float speed, float stoppingDistance)
        {
            _speed = speed;
            _stopingDistance = stoppingDistance;
        }

        public void SetTarget([CanBeNull] GetTarget target)
        {
            Target = target;
        }

        public override void Enter()
        {
            base.Enter();
            _animation.SetAnimation(_parameterName);
            _characterController.enabled = true;
        }

        public override void Tick(float tickTime)
        {
            if (Target == null) return;
            if (CanRunTo(_transform, _stopingDistance, Target))
            {
                var direction = _transform.forward;
                direction.y = -0.9f;  
                _characterController.Move(direction * (_speed * tickTime));
            }
        }

        public bool CanRunTo(Transform transform, float stopDistance, [CanBeNull] GetTarget target)
        {
            if (target == null) return false;
            return Vector3.Distance(transform.position, target.Invoke()) >= stopDistance;
        }

        public bool CanRunTo(Transform transform, float stopDistance)
        {
            if (Target == null) return false;
            return Vector3.Distance(transform.position, Target.Invoke()) >= stopDistance;
        }
    }

    [Serializable]
    public struct RunToPointData
    {
        public CharacterController CharacterController;
        public float Speed;
        public float StopDistance;
        [HideInInspector] public Transform ThisCharacter;
    }
}