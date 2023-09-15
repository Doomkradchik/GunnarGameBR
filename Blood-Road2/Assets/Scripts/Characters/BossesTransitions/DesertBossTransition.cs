using Characters.Player.States;
using UnityEngine;


namespace Characters.Facades
{
    public delegate bool CanGoUnderground(bool use = false);
    public delegate Vector3 CalculatePosition(Vector3 player, Vector3 zone);
    public sealed class DesertBossTransition : WormTransition
    {
        private readonly CanGoUnderground _canGoUnderground;
        private readonly CalculatePosition _positionProvider;
        private readonly float _timeInSecondsUnderground;
        private Vector3? _zone;
        private float _startTime = 0;
        public DesertBossTransition(CanGoUnderground canGoUnderground,
            CalculatePosition positionProvider, float timeInSecondsUnderground)
        {
            _canGoUnderground = canGoUnderground;
            _positionProvider = positionProvider;
            _timeInSecondsUnderground = timeInSecondsUnderground;
        }
        protected override void StatesInit(Animator animator, AudioSource audioSource, TransitionAndStatesData data, AnimatorOverrideController animatorOverrideController, VFXTransforms vfxTransforms)
        {
            base.StatesInit(animator, audioSource, data, animatorOverrideController, vfxTransforms);
            _performance.SetTimeInSecondsUnderground(_startTime);
        }

        protected override void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
            _stateMachine.AddTransition(_performance, _attackState, () => StartAppearance(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _idleState, () => CanAttackTarget(transform, runToPointData) == false);
            _stateMachine.AddTransition(_idleState, _attackState, () => CanAttackTarget(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _performance, () => 
            {
                var result = _performance.Appear(() => _canGoUnderground.Invoke());
                if (result) _canGoUnderground.Invoke(true);
                return result;
            });
            _stateMachine.ChangeState(_performance);
        }

        private Vector3 GetNewPosition()
        {
            var playerPosition = GetInteractable().GetObject().position;
            if (_zone is null)
                _zone = playerPosition;

            return _positionProvider.Invoke(playerPosition, _zone.Value);
        }

        private bool StartAppearance(Transform transform, RunToPointData runToPointData)
        {
            bool result = _performance.Appear(() => CanAttackTarget(transform, runToPointData), GetNewPosition);
            if (result && _startTime == 0)
            {
                _performance.SetTimeInSecondsUnderground(_timeInSecondsUnderground);
                _startTime = _timeInSecondsUnderground;
            }
            return result;
        }
    }
}