using Characters.Player.States;
using UnityEngine;

namespace Characters.Facades
{
    public class WormTransition : EnemyTransition
    {
        protected WormPerformance _performance;
        protected override void StatesInit(Animator animator, AudioSource audioSource, TransitionAndStatesData data, AnimatorOverrideController animatorOverrideController, VFXTransforms vfxTransforms)
        {
            base.StatesInit(animator, audioSource, data, animatorOverrideController, vfxTransforms);
            _stateCharacterKey.SetState(typeof(WormPerformance));
            if (TryGetList(out var list) && list.Count == 1 && TryGetView(out var view))
            {
                _performance = new WormPerformance(data.Enable, data.Model, _animation, _audio, view, vfxTransforms);
                _performance.SetAppearingClip(list[0].View.Animation);
            }
        }
        protected override void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
            //_stateMachine.AddTransition(_performance, _attackState, () => 
            //  _performance.Appear(() => CanAttackTarget(transform, runToPointData)));
            //_stateMachine.ChangeState(_performance);

            _stateMachine.AddTransition(_idleState, _attackState, () => CanAttackTarget(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _idleState, () => CanAttackTarget(transform, runToPointData) == false);
            _stateMachine.ChangeState(_idleState);
        }
    }
}
