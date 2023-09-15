using Characters.Animations;
using Characters.Sounds;
using MapSystem.Structs;
using UnityEngine;
using System;
using Characters;

namespace Characters.Player.States
{
    public class Sleeping : BaseState
    {
        private readonly Transform transform;
        private readonly Action _addList;
        private readonly Action _removeList;
        private readonly Enable _enable;

        public Sleeping()
        {

        }

        public Sleeping(Transform transform, SleepingDelegates sleepingDelegates, Enable enable, IAnimationCommand animation, IAudioCommand audio, 
            View view, VFXTransforms vfxTransforms) : base(animation, audio, view, vfxTransforms)
        {
            _addList = sleepingDelegates.addList;
            _removeList = sleepingDelegates.removeList;
            _enable =  enable;
            _parameterName = "seating";
            this.transform = transform;
        }

        public async override void Enter()
        {
            base.Enter();
            _removeList.Invoke();
            _animation.SetAnimation(_parameterName);
            _animation.SetSpeedAnimation(1f);
            await System.Threading.Tasks.Task.Delay(100);
            _enable.Invoke(false);
        }

        public override void Tick(float tickTime)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0,0,0);
        }

        protected override void OnExit()
        {
            base.OnExit();
            _addList.Invoke();
            _enable.Invoke(true);
        }
    }
}

public struct SleepingDelegates
{
    public Action addList;
    public Action removeList;
}