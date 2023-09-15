using Characters.Player.States;
using UnityEngine;

namespace Characters.Facades
{
    public class SupporterTransition : TransitionAndStates
    {
        public SupporterTransition(float maxDistance)
        {
            _maxDistanceValue = maxDistance;
        }

        protected readonly float _maxDistanceValue;

        protected Parameters _followPlayerParam;
        protected Parameters _followEnemyParam;

        protected struct Parameters
        {
            public float Speed;
            public float StoppingDistance;
        }

        public enum FollowMode : int
        {
            Player = 0,
            Enemy = 1
        }

        public override void Initialize(TransitionAndStatesData data)
        {
            base.Initialize(data);
            StatesInit(data.Animator, data.AudioSource, data, data.AnimatorOverrideController, data.VFXTransforms);

            _stateCharacterKey.SetState(typeof(Attack));
            if (TryGetView(out var view)&& TryGetAbility(out var ability))
                data.CreateAttack(new Attack(_animation, _audio, view, false, data, data.VFXTransforms, ability.EffectData));

            _stateCharacterKey.SetState(typeof(DieEnemy));
            if (TryGetView(out view))
                data.CreateDie(new DieEnemy(_animation, _audio, view, data.CharacterController, data.VFXTransforms));

            _attackState = data.Attack;
            _dieState = (DieEnemy)data.Die;
            _attackState.SetThisCharacter(data.CharacterData.CurrentInteractable);
            TransitionInit(data.Transform, data.RunToPointData);

            _followPlayerParam = new Parameters { Speed = 5f, StoppingDistance = 3f };
            _followEnemyParam = new Parameters { Speed = 12f, StoppingDistance = 2.5f };
        }

        public void SetMode(FollowMode mode)
        {
            Parameters parameters;
            switch (mode)
            {
                case FollowMode.Player:
                    parameters = _followPlayerParam;
                    break;
                case FollowMode.Enemy:
                    parameters = _followEnemyParam;
                    break;
                default:
                    throw new System.InvalidOperationException();
            }

            _runToPointState.SetParams(parameters.Speed, parameters.StoppingDistance);
        }

        protected override void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
            base.TransitionInit(transform, runToPointData);
            _stateMachine.AddTransition(_idleState, _runToPointState, () =>
            {
                var point = GetInteractable();

                if (point == null)
                {
                    return false;
                }

                if (point.IsPlayer())
                    return Vector3.Distance(transform.position, GetInteractable().GetObject().position) >=
                           _maxDistanceValue;

                var dieState = (DieEnemy)_dieState;
                dieState.SetPlayerTransform(GetInteractable().GetObject());
                _runToPointState.SetTarget(() => GetInteractable().GetObject().position);

                return true;
            });
            _stateMachine.AddTransition(_runToPointState, _idleState,
                () => GetInteractable() == null ||
                      !IsRuning(transform, runToPointData) && GetInteractable().IsPlayer());
            _stateMachine.AddTransition(_runToPointState, _attackState,
                () => !IsRuning(transform, runToPointData) && GetInteractable().IsPlayer() == false);
            _stateMachine.AddTransition(_attackState, _runToPointState,
                () => IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _idleState, (() => GetInteractable() == null));
            _stateMachine.ChangeState(_idleState);
        }
    }
}