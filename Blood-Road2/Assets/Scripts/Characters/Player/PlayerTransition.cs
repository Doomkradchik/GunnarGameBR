using Characters.Player;
using Characters.Player.States;
using Dreamteck.Splines;
using Attack = Characters.Player.States.Attack;
using UnityEngine;

namespace Characters.Facades
{
    public class PlayerTransition : TransitionAndStates
    {
        private FolowSpline _folowSplineState;
        private SplineFollower _splineFollower;
        private Weakness _weaknessState;
        private IInteractable _interactable;

        private CharacterData _characterData;
        private float _stoppingDistance;
        private Transform _transform;

        public override void Initialize(TransitionAndStatesData data)
        {
            base.Initialize(data);

            _characterData = data.CharacterData;
            _interactable = data.CharacterData.CurrentInteractable;
            _splineFollower = data.SplineFollower;
            StatesInit(data.Animator, data.AudioSource, data, data.AnimatorOverrideController, data.VFXTransforms);

            _stateCharacterKey.SetState(typeof(Attack));
            if (TryGetView(out var view) && TryGetAbility(out var ability))
                data.CreateAttack(new Attack(_animation, _audio, view, true, data,
                    data.VFXTransforms, ability.EffectData));

            _stateCharacterKey.SetState(typeof(Die));
            if (TryGetView(out view))
                data.CreateDie(new Die(_animation, _audio, view, data.CharacterController, data.VFXTransforms));

            _attackState = data.Attack;
            _dieState = data.Die;
            _setAttackSpeed = _attackState.SetAnimationSpeed;
            _canAttack = () => {
                return CanAttackTarget(data.Transform, data.RunToPointData) && GetInteractable().Enabled;
            }; 
            _attack = _attackState.SetStateInfo;
            _attack += info =>
            {
                if (CanAttackTarget(data.Transform, data.RunToPointData)
                && isWeak(_characterData) == false)
                    _stateMachine.ChangeState(_attackState);
            };
           
            _attackState.SetThisCharacter(_interactable);
            _transform = data.Transform;
            _stoppingDistance = data.RunToPointData.StopDistance;
            TransitionInit(data.Transform, data.RunToPointData);
        }

        protected override void StatesInit(Animator animator, AudioSource audio, TransitionAndStatesData data,
            AnimatorOverrideController animatorOverrideController,
            VFXTransforms vfxTransforms)
        {
            base.StatesInit(animator, audio, data, animatorOverrideController, vfxTransforms);
            _stateCharacterKey.SetState(typeof(RunToPoint));
            if (TryGetView(out var view))
                _folowSplineState = new FolowSpline(_animation, _audio, view, vfxTransforms, _splineFollower, data.SplineProjector);

            _stateCharacterKey.SetState(typeof(Weakness));
            if (TryGetView(out view))
                _weaknessState = new Weakness(_animation, _audio, view, vfxTransforms);
        }

        protected override void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
            base.TransitionInit(transform, runToPointData);

            _stateMachine.AddTransition(_folowSplineState, () => CanFollowSpline &&
            CanRunToSpline(transform, FolowSpline.MIN_FROM_SPLINE_DISTANCE, _folowSplineState.Project) == false);
            _stateMachine.AddTransition(_runToPointState, () => CanFollowSpline &&
            CanRunToSpline(transform, FolowSpline.MIN_FROM_SPLINE_DISTANCE, _folowSplineState.Project));

            _stateMachine.AddTransition(_folowSplineState, _runToPointState, () => IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_idleState, _runToPointState, () => IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_idleState, _shieldState,
                () =>
                {
                    var point = GetInteractable();
                    if (point == null)
                    {
                        return false;
                    }

                    var objectPoint = point.GetObject();
                    return Vector3.Distance(transform.position, objectPoint.position) <=
                           runToPointData.StopDistance + .3f;
                });
            _stateMachine.AddTransition(_runToPointState, _idleState, () =>
            isWeak(_characterData) == false && GetInteractable() == null &&
            _runToPointState.CanRunTo(transform, FolowSpline.MIN_FROM_SPLINE_DISTANCE, _folowSplineState.Project) == false
            || GetInteractable() != null && IsRuning(transform, runToPointData) == false);
            _stateMachine.AddTransition(_runToPointState, _shieldState, () => isWeak(_characterData) == false &&
              _runToPointState.CanRunTo(transform, FolowSpline.MIN_FROM_SPLINE_DISTANCE, _folowSplineState.Project) == false &&
                Vector3.Distance(transform.position, GetInteractable().GetObject().position) <=
                runToPointData.StopDistance + .1f);


            _stateMachine.AddTransition(_shieldState, _idleState, () => GetInteractable() == null);
            _stateMachine.AddTransition(_shieldState, _runToPointState, () => IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _runToPointState,
                () => _attackState.CanSkip && IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _shieldState, () => isWeak(_characterData) == false && _attackState.CanSkip);
            _stateMachine.AddTransition(_attackState, _idleState,
               () => isWeak(_characterData) == false && _attackState.CanSkip && GetInteractable() == null);

            _stateMachine.AddTransition(_attackState, _weaknessState, () => _attackState.CanSkip && isWeak(_characterData));
            _stateMachine.AddTransition(_shieldState, _weaknessState, () => isWeak(_characterData));
            _stateMachine.AddTransition(_idleState, _weaknessState, () => IsStoped && isWeak(_characterData) || isWeak(_characterData));
            _stateMachine.AddTransition(_runToPointState, _weaknessState, () => isWeak(_characterData) && !IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_weaknessState, _runToPointState, () => IsRuning(transform, runToPointData));

            _stateMachine.AddTransition(_idleState, () => IsStoped && isWeak(_characterData) == false);
            _stateMachine.ChangeState(_idleState);
        }

        private bool CanFollowSpline => GetInteractable() == null && !IsStoped && _attackState.CanSkip && _splineFollower.follow == false;

        private bool CanRunToSpline(Transform transform, float stoppingDistance, GetTarget target)
        {
            var canRun = _runToPointState.CanRunTo(transform, FolowSpline.MIN_FROM_SPLINE_DISTANCE, target);
            if(canRun)
            {
                _runToPointState.SetTarget(target);
                _runToPointState.SetParams(3f, FolowSpline.MIN_FROM_SPLINE_DISTANCE);
                return true;
            }
            return canRun;
        }

        protected override void OnUpdate()
        {
            if (_look == null) return;
            GetTarget target;

            if (_runToPointState.CanRunTo(_transform, _stoppingDistance))
                target = _runToPointState.Target;
            else
            {
                if (GetInteractable() == null) return;
                target = () => GetInteractable().GetObject().position;
            }

            _look(target.Invoke());
        }

        protected override bool IsRunningCondition(Transform transform, RunToPointData runToPointData)
        {
            if (GetInteractable() != null)
            {
                var position = GetInteractable().GetObject().position;
                if (position != null &&
                    Vector3.Distance(transform.position, (Vector3)position) >= runToPointData.StopDistance + .2f)
                {
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }
    }
}