using Characters.Player.States;
using UnityEngine;

namespace Characters.Facades
{
    public delegate bool InvokeCharacter();

    public sealed class OrkHunterTransition : EnemyTransition
    {
        public OrkHunterTransition(InvokeCharacter invokeCharacter, SleepingDelegates sleepingDelegates)
        {
            _invokeCharacter = invokeCharacter;
            this.sleepingDelegates = sleepingDelegates;
        }
        private Sleeping _seatingState;
        private readonly InvokeCharacter _invokeCharacter;
        private readonly SleepingDelegates sleepingDelegates;

        protected override void StatesInit(Animator animator, AudioSource audioSource, TransitionAndStatesData data, AnimatorOverrideController animatorOverrideController, VFXTransforms vfxTransforms)
        {
            base.StatesInit(animator, audioSource, data, animatorOverrideController, vfxTransforms);
            _stateCharacterKey.SetState(typeof(Sleeping));

            if (TryGetView(out var view))
                _seatingState = new Sleeping(data.Transform, sleepingDelegates, data.Enable, 
                    _animation, _audio, view, vfxTransforms);
        }

        protected override void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
            base.TransitionInit(transform, runToPointData);
            _stateMachine.AddTransition(_seatingState, _idleState, _invokeCharacter.Invoke);
            _stateMachine.ChangeState(_seatingState);
        }
    }
}
