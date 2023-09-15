using Characters.Player.States;
using Characters.States;
using UnityEngine;

namespace Characters.Facades
{
    public class EnemyTransition : TransitionAndStates
    {
        public override void Initialize(TransitionAndStatesData data)
        {
            base.Initialize(data);
            StatesInit(data.Animator,data.AudioSource, data, data.AnimatorOverrideController, data.VFXTransforms);
            _stateCharacterKey.SetState(typeof(Attack));
            if (TryGetView(out var view)&& TryGetAbility(out var ability))
                data.CreateAttack(new Attack(_animation, _audio,view, false, data, data.VFXTransforms,ability.EffectData ));

            _stateCharacterKey.SetState(typeof(DieEnemy));
            if (TryGetView(out view))
                data.CreateDie(new DieEnemy(_animation, _audio, view, data.CharacterController, data.VFXTransforms));

            _attackState = data.Attack;
            DieEnemy dieState = (DieEnemy)data.Die;
            dieState.SetMoneyData(new MoneyData
            {
                prefab = data.MoneyPrefab,
                moneyAfterDeath = data.MoneyCountAfterDeath
            });
            _dieState = dieState;
            _attackState.SetThisCharacter(data.CharacterData.CurrentInteractable);
            TransitionInit(data.Transform, data.RunToPointData);
        }

        protected override void StatesInit(Animator animator, AudioSource audioSource, TransitionAndStatesData data,
            AnimatorOverrideController animatorOverrideController, VFXTransforms vfxTransforms)
        {
            base.StatesInit(animator, audioSource, data, animatorOverrideController, vfxTransforms);
            _stateCharacterKey.SetState(typeof(DieEnemy));
            if (TryGetView(out var view))
                _explosiveRecoilState =
                    new ExplosiveRecoil(_animation, _audio, view, vfxTransforms, data.CharacterController);
        }

        protected override void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
            base.TransitionInit(transform, runToPointData);
            _stateMachine.AddTransition(_idleState, _runToPointState, () =>
            {
                if (GetInteractable() == null) return GetInteractable() != null;
                var dieState = (DieEnemy)_dieState;
                dieState.SetPlayerTransform(GetInteractable().GetObject());
                _runToPointState.SetTarget(() => GetInteractable().GetObject().position);
                return GetInteractable() != null;
            });
            _stateMachine.AddTransition(_runToPointState, _idleState, () => GetInteractable() == null);
            _stateMachine.AddTransition(_runToPointState, _attackState, () => !IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _runToPointState, () => IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _idleState, (() => CanAttackTarget(transform, runToPointData) == false));
            _stateMachine.AddTransition(_attackState, _explosiveRecoilState, CanRecoil);
            _stateMachine.AddTransition(_runToPointState, _explosiveRecoilState, CanRecoil);
            _stateMachine.AddTransition(_idleState, _explosiveRecoilState, CanRecoil);
            _stateMachine.AddTransition(_explosiveRecoilState, _runToPointState, IsStoodUp);

            _stateMachine.ChangeState(_idleState);
        }

    }
}