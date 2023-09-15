using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Characters.AbilitiesSystem;
using Characters.EffectSystem;
using Characters.Facades;
using Characters.InteractableSystems;
using Characters.Player;
using Characters.Player.States;
using Characters.States;
using Dreamteck.Splines;
using Interaction;
using JetBrains.Annotations;
using MapSystem;
using UnityEngine;

namespace Characters
{
    public delegate bool GetIsAttack();

    public delegate void StartRechangeCurrentPoint(List<IInteractable> points);

    public delegate IInteractable GetCurrentPoint();

    public delegate bool HasCharacter();

    public delegate Vector3 GetTarget();

    public delegate void Enable(bool enable);

    public delegate void Look(Vector3 target);

    public abstract class BaseCharacter : MonoBehaviour, IInteractable, IInit<DieInteractable>
    {
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private VFXTransforms vfxTransforms;
        [SerializeField] protected Placeholder mapStates;
        [SerializeField] protected RunToPointData runToPointData;
        [SerializeField] protected Animator animator;
        [SerializeField] protected AudioSource audioSource;
        [SerializeField] protected Eyes eyesCharacters;
        [SerializeField] protected CharacterData characterData;
        [SerializeField] protected Linker linker;
        [SerializeField] protected float rotationSpeed = 1f;
        [SerializeField] protected int iDCharacter;
        [HideInInspector] [SerializeField] public Sender Sender;
        protected bool _hasCharacter = true;

        protected IInteractable _currentPoint;

        private SetCurrentPoint _setCurrentPoint;
        private StartRechangeCurrentPoint _startRechangeCurrentPoint;
        private GetCurrentPoint getCurrentPoint;
        private DieInteractable _characterPointDie;
        private HasCharacter _hasCharacterDelegate;

        public event AttackedAbility AttackAbility;
        public event AttackedWeapon AttackWeapon;


        private InteractionSystem _interactionSystem;
        protected TransitionAndStates _transitionAndStates;
        private IInteractable GetInteractable() => _currentPoint;
        public ListOperation GetRemoveList() => RemoveList;
        public ListOperation GetAddList() => AddList;

        public bool HasCharacter() => _hasCharacter;
        public Receiver Receiver => linker.Receiver;
        public IInit<DieInteractable> InitDie() => this;
        public ICharacterDataSubscriber CharacterDataSubscriber;

        public VFXTransforms VFXTransforms => vfxTransforms;

        public void SetPlaceholder(Placeholder placeholder) => mapStates = placeholder;

        public virtual void Finish()
        {
        }


        public Transform GetObject() => this.transform;
        public abstract bool IsPlayer();
        public DieInteractable GetDieCharacterDelegate => _characterPointDie;

        public bool Enabled { get; private set; } = true;

        public void SetCharacterController()
        {
            runToPointData.CharacterController = GetComponent<CharacterController>();
        }

        public void BindStates(List<MapperItem> states)
        {
            if (mapStates == null) mapStates = gameObject.GetComponent<Placeholder>();
            mapStates.BindStates(states);
        }

        protected virtual void Awake()
        {
            runToPointData.ThisCharacter = transform;
            _setCurrentPoint = SetCurrentPoint;
            _startRechangeCurrentPoint = StartRCP;
            getCurrentPoint = GetInteractable;

            _hasCharacterDelegate = HasCharacter;

            _characterPointDie = ClearPoint;
            characterData.SetInteractable(this);
            if (CharacterDataSubscriber == null) CharacterDataSubscriber = characterData;
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        }

        public virtual void OnCreated() { }

        protected virtual void RemoveList(IInteractable enemy)
        {
        }

        protected virtual void AddList(IInteractable interactable)
        {    
        }

        public void SetCharacterData(CharacterData data)
        {
            characterData = data.Copy();
            CharacterDataSubscriber = characterData;
            OnSetCharacterData(data);
        }

        protected virtual void OnSetCharacterData(CharacterData data) { }

        public virtual void SubscribeCharacterData()
        {
            CharacterDataSubscriber = characterData;
            CharacterDataSubscriber.DieEvent += () => _hasCharacter = false;
            CharacterDataSubscriber.Damage += _transitionAndStates.Damage;
        }

        protected virtual void SubscribeDeath()
        {
            CharacterDataSubscriber.DieEvent += _transitionAndStates.DieDelegate;
            CharacterDataSubscriber.DieEvent += vfxTransforms.DieDelegate;
        }

        protected virtual void UnsubscribeDeath()
        {
            _hasCharacter = false;
            CharacterDataSubscriber.DieEvent -= _transitionAndStates.DieDelegate;
        }

        private void Enable(bool enable)
        {
            characterData.OnEnable(enable);
            Enabled = enable;
        }

        protected virtual TransitionAndStatesData GetData(TransitionAndStates transitionAndStates,
            [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] SplineFollower splineFollower = null,[CanBeNull] SplineProjector splineProjector =null,
            [CanBeNull] Money money = null, [CanBeNull] GetRecoil getRecoil = null)
        {
            return new TransitionAndStatesData(animator, audioSource, getCurrentPoint, transform, animator.gameObject,
                runToPointData, getIsAttack, characterData, getRecoil, Enable,
                mapStates, iDCharacter, _hasCharacterDelegate,
                _animatorOverrideController, vfxTransforms,
                splineFollower,splineProjector, money);
        }

        protected void InitializeTransition(TransitionAndStates transitionAndStates,
            [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] SplineFollower splineFollower = null,[CanBeNull] SplineProjector splineProjector = null,
            [CanBeNull] Money money = null, [CanBeNull] GetRecoil getRecoil = null
        )
        {
            _transitionAndStates = transitionAndStates;
            linker.Initialize(_transitionAndStates, characterData);
            _transitionAndStates.Initialize(GetData(transitionAndStates, getIsAttack, splineFollower,splineProjector, money, getRecoil));
            _transitionAndStates.SetLook(Look);
        }

        public virtual void GetRecoil(Vector3 origin, ExplosionParameters parameters)
        {
            characterData.DoRecoil();
            _transitionAndStates.SetRecoilData(origin, parameters);
        }

        protected virtual void InitializeAbility(AbilityData abilityData)
        {
        }

        protected void InitializeInteractionSystem([CanBeNull] CameraRay cameraRay)
        {
            _interactionSystem = new InteractionSystem();
            _interactionSystem.Initialize(cameraRay, eyesCharacters, transform, _setCurrentPoint,
                _startRechangeCurrentPoint);
        }

        protected abstract void ClearPoint(IInteractable interactable);


        private void Update()
        {
            _transitionAndStates.Update();
        }

        protected virtual void Look(Vector3 turnTowardNavSteeringTarget)
        {
            var direction = (turnTowardNavSteeringTarget - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        protected abstract void StartRCP(List<IInteractable> points);

        protected virtual void SetCurrentPoint(IInteractable point)
        {
            _transitionAndStates.SetPoint(point);
            //            CharacterDataSubscriber.DieEvent += (() => _currentPoint.GetDieCharacterDelegate?.Invoke(this));
        }

        public void TakeDamage(EffectData effectData)
        {
            AttackWeapon?.Invoke(Receiver, effectData);
        }

        public abstract void SetOutline(bool value);


        public virtual bool TryAttackByWeapon(EffectData effectData, int costAttack, int delay)
        {
            if (_currentPoint?.Receiver == null|| !characterData.HasEnoughEnergy(costAttack) 
                || _currentPoint.Enabled == false) return false;
                characterData.UseEnergy(costAttack);
                Task.Delay(delay);
                AttackWeapon?.Invoke(_currentPoint.Receiver, effectData);
            return true;
        }

        public void TakeAbility(IAbilityCommand command)
        {
            AttackAbility?.Invoke(Receiver, command);
        }

        public virtual void UseAbility(IAbilityCommand abilityCommand, int value)
        {
            _transitionAndStates.RunAbility.RunAbility(abilityCommand);
            if (_currentPoint != null) AttackAbility?.Invoke(_currentPoint.Receiver, abilityCommand);
        }

        protected virtual void OnDestroy()
        {
            _transitionAndStates.Destroy();

            UnsubscribeDeath();
        }

        public void Subscribe(DieInteractable subscriber)
        {
            characterData.DieInteractable += subscriber;
        }

        public void Unsubscribe(DieInteractable unsubscriber)
        {
            characterData.DieInteractable -= unsubscriber;
        }
    }
}