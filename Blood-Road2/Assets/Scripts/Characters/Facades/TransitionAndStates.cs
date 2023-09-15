using System;
using System.Collections.Generic;
using Better.UnityPatterns.Runtime.StateMachine;
using Characters.AbilitiesSystem;
using Characters.Animations;
using Characters.Player;
using Characters.Player.States;
using Characters.States;
using Characters.Sounds;
using MapSystem;
using MapSystem.Structs;
using UnityEngine;
using Attack = Characters.Player.Attack;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Characters.Facades
{
    public delegate bool CanAttack();
    public abstract class TransitionAndStates : IAnimatableEffect, ISerializableStates
    {
        protected Placeholder _mapStates;
        protected int _idCharacter;
        protected StateCharacterKey _stateCharacterKey;
        private IRunAbility _runAbility;
        protected Vector3 _origin;
        protected Look _look;

        protected IAnimationCommand _animation;
        protected IAudioCommand _audio;
        private VFXTransforms _vfxTransforms;
        private List<VFXEffect> _damageParticles;

        private static readonly List<Vector3> DAMAGED_VECTORS = new()
        {
            Vector3.forward,
            Vector3.left,
            Vector3.right,
            Vector3.forward + Vector3.left / 2,
            Vector3.forward - Vector3.left / 2,
            Vector3.forward + Vector3.right / 2,
            Vector3.forward - Vector3.right / 2,
            Vector3.right + Vector3.left / 2,
            Vector3.right - Vector3.left / 2,
            Vector3.left + Vector3.right / 2,
            Vector3.left - Vector3.right / 2,
        };

        #region states

        protected RunToPoint _runToPointState;
        protected Idle _idleState;
        protected Player.States.Attack _attackState;
        protected Shield _shieldState;
        protected Die _dieState;
        protected ExplosiveRecoil _explosiveRecoilState;

        protected StateMachine<BaseState> _stateMachine;

        #endregion

        #region events

        protected event GetIsAttack GetIsAttack;
        protected event GetCurrentPoint CurrentPoint;

        #endregion

        #region delegates

        private Action _dieDelegate;
        protected Attack _attack;
        protected CanAttack _canAttack;
        protected SetAttackSpeed _setAttackSpeed;
        protected GetRecoil _getRecoil;

        #endregion


        public bool IsStoped;

        #region publicVariables

        public Action DieDelegate => _dieDelegate;

        public Attack Attack => _attack;
        public CanAttack CanAttack => _canAttack;
        public SetAttackSpeed SetAttackSpeed => _setAttackSpeed;
        public IRunAbility RunAbility => _runAbility;

        #endregion

        public virtual void Initialize(TransitionAndStatesData data)
        {
            _dieDelegate = Death;
            if (data.GetIsAttack != null)
            {
                GetIsAttack += data.GetIsAttack;
            }

            _idCharacter = data.ID;
            _stateCharacterKey = new StateCharacterKey();
            _stateCharacterKey.SetID(_idCharacter);
            CurrentPoint += data.GetCurrentPoint;
            _mapStates = data.MapStates;
            _getRecoil = data.GetRecoil;
        }

        public void SetRecoilData(Vector3 origin, ExplosionParameters parameters)
        {
            _origin = origin;
            _explosiveRecoilState.SetOrigin(origin);
            _explosiveRecoilState.SetParameters(parameters);
        }

        public void SetLook(Look look) => _look = look;

        protected virtual void StatesInit(Animator animator, AudioSource audioSource, TransitionAndStatesData data,
            AnimatorOverrideController animatorOverrideController,
            VFXTransforms vfxTransforms)
        {
            _animation = new AnimatorController(animator);
            _animation.CreateAnimationChanger(animatorOverrideController);
            _audio = new AudioController(audioSource);
            _vfxTransforms = vfxTransforms;


            _stateCharacterKey.SetState(typeof(RunToPoint));
            if (TryGetView(out var view))
                _runToPointState = new RunToPoint(_animation, _audio, data.RunToPointData, view, vfxTransforms);

            _stateCharacterKey.SetState(typeof(Idle));
            if (TryGetView(out view))
                _idleState = new Idle(_animation, _audio, view, vfxTransforms);
            _stateCharacterKey.SetID(0);
            _stateCharacterKey.SetState(typeof(Damaged));
            TryGetView(out view);
            _damageParticles = new List<VFXEffect>();
            _damageParticles.Add(Object.Instantiate(view.Effect, _vfxTransforms.Center));
            _damageParticles.Add(Object.Instantiate(view.Effect, _vfxTransforms.Center));
            _damageParticles.Add(Object.Instantiate(view.Effect, _vfxTransforms.Center));
            _damageParticles.Add(Object.Instantiate(view.Effect, _vfxTransforms.Center));
            _damageParticles.Add(Object.Instantiate(view.Effect, _vfxTransforms.Center));
            _damageParticles.Add(Object.Instantiate(view.Effect, _vfxTransforms.Center));
            _stateCharacterKey.SetID(0);
            _stateCharacterKey.SetState(typeof(Shield));
            if (TryGetView(out view))
                _shieldState = new Shield(_animation, _audio, view, vfxTransforms);
            _stateCharacterKey.SetID(_idCharacter);

            _stateMachine = new StateMachine<BaseState>();
        }

        protected bool TryGetView(out View view)
        {
            return _mapStates.TryGetView(_stateCharacterKey, out view);
        }

        protected bool TryGetList(out List<Item> items)
        {
            return _mapStates.TryGetList(_stateCharacterKey, out items);
        }

        protected bool TryGetAbility(out Ability ability)
        {
            return _mapStates.TryGetAbility(_stateCharacterKey, out ability);
        }

        public void SetPoint(IInteractable point)
        {
            _runToPointState.SetTarget(() => point.GetObject().position);
        }

        public void Damage()
        {
            var position = _vfxTransforms.Center.position;
            var vectorIndex = 0;
            foreach (var damageParticle in _damageParticles)
            {
                vectorIndex = Random.Range(0, DAMAGED_VECTORS.Count);
                StartDamageParticlePlay(damageParticle, position, DAMAGED_VECTORS[vectorIndex]);
            }
        }

        private void StartDamageParticlePlay(VFXEffect effect, Vector3 position, Vector3 rotation)
        {
            if (effect == null) return;
            effect.SetPosition(position);
            effect.SetRotation(Quaternion.LookRotation(rotation));
            effect.StartPlayback();
        }

        public void SetCurrentEffectID(Type type)
        {
            if (_runAbility != null) _runAbility.SetTypeAbility(type);
        }

        public void InitializeAbilities(Abilities abilities)
        {
            _runAbility = abilities;
        }

        public AbilityData ReturnReadyData(AbilityData abilityData)
        {
            abilityData.SetAnimationCommand(_animation);
            abilityData.SetIdleState(_idleState);
            abilityData.SetStateMachine(_stateMachine);
            return abilityData;
        }

        public void Destroy()
        {
            _stateMachine.ChangeState(_idleState);
        }

        protected virtual void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
        }

        protected virtual bool IsRunningCondition(Transform transform, RunToPointData runToPointData)
        {
            Transform currentPoint = CurrentPoint?.Invoke().GetObject();
            _runToPointState.SetTarget(() => currentPoint.position);
            var position = CurrentPoint?.Invoke().GetObject().position;
            return position != null &&
                   Vector3.Distance(transform.position, (Vector3)position) >= runToPointData.StopDistance;
        }

        protected bool IsRuning(Transform transform, RunToPointData runToPointData)
        {
            if (CurrentPoint?.Invoke() == null) return false;
            _runToPointState.SetParams(runToPointData.Speed, runToPointData.StopDistance);
            return IsRunningCondition(transform, runToPointData);
        }

        protected virtual bool CanAttackTarget(Transform transform, RunToPointData runToPointData)
        {
            var target = CurrentPoint?.Invoke();
            if (target == null) return false;
            var close = Vector3.Distance(transform.position, target.GetObject().position) <= runToPointData.StopDistance * 1.12f;
            return close;
        }

        protected bool isWeak(CharacterData data) => !(_attackState.CanUseEnergy() || data.HasEnoughEnergy(5));


        protected virtual bool IsStoodUp()
        {
            return _explosiveRecoilState.IsRecoiled;
        }

        protected bool CanRecoil() => _getRecoil.Invoke();

        public void Update()
        {
            _stateMachine.Tick(Time.deltaTime);
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            if (GetInteractable() != null && _look != null)
                _look(GetInteractable().GetObject().position);
        }


        protected IInteractable GetInteractable()
        {
            return CurrentPoint?.Invoke();
        }

        protected bool IsAttack()
        {
            return GetIsAttack.Invoke();
        }

        private void Death()
        {
            _stateMachine.ChangeState(_dieState);
            _look = null;
        }
    }
}