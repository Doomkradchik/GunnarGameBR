using System.Collections.Generic;
using System.Linq;
using Better.Attributes.Runtime.Select;
using Characters.AbilitiesSystem;
using Characters.Facades;
using Characters.Player;
using Dreamteck.Splines;
using Interaction;
using JetBrains.Annotations;
using MapSystem;
using UnityEngine;

namespace Characters.Enemy
{
    public interface IMoneyHandler
    {
        void InitMoney(int count);
    }

    [RequireComponent(typeof(Placeholder))]
    public class DefaultEnemy : BaseCharacter, IMoneyHandler
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [Header("Money")]
        [SerializeField] protected Money moneyPrefab;
        [Header("Outline")]
        [SerializeField] private float outlineValue=0.5f;

        [field: SerializeReference] [field: SelectImplementation(typeof(TransitionAndStates))]
        protected TransitionAndStates _transition;

        public override bool IsPlayer() => false;
        protected event ListOperation _removeList;
        protected event ListOperation _addList;
        protected int _moneyAfterDeath = 1;

        public void InitMoney(int count)
        {
            _moneyAfterDeath = count;
        }

        protected override void OnSetCharacterData(CharacterData data)
        {
            base.Awake();
            InitializeTransition(_transition == null ? new EnemyTransition() : _transition, null, null, null,
                moneyPrefab, characterData.GetRecoilDelegate);
            InitializeAbility(new AbilityData(VFXTransforms, characterData.ImpenetrableDelegate,
                mapStates, iDCharacter));
            InitializeInteractionSystem(null);
            SubscribeDeath();
            SubscribeCharacterData();
            CharacterDataSubscriber.DieEvent += Die;
            StatsCounter.Instance.InitCharacterDataSub(CharacterDataSubscriber);
        }

        protected override TransitionAndStatesData GetData(TransitionAndStates transitionAndStates, [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] SplineFollower splineFollower = null, [CanBeNull] SplineProjector splineProjector = null, [CanBeNull] Money money = null, [CanBeNull] GetRecoil getRecoil = null)
        {
            return base.GetData(transitionAndStates, getIsAttack, splineFollower, splineProjector, money, getRecoil)
                .OverrideMoneyCount(_moneyAfterDeath);
        }

        protected override void InitializeAbility(AbilityData abilityData)
        {
            var data = _transitionAndStates.ReturnReadyData(abilityData);
            _transitionAndStates.InitializeAbilities(new AbilitiesSystem.Enemy(data));
        }

        protected override void ClearPoint(IInteractable interactable)
        {
            if (_currentPoint == null) return;
            characterData.DieInteractable -= _currentPoint.GetDieCharacterDelegate;
            _currentPoint = null;
        }

        protected override void StartRCP(List<IInteractable> points)
        {
            foreach (var point in points.Where(point => point.IsPlayer()))
            {
                SetCurrentPoint(point);
            }
        }

        protected override void SetCurrentPoint(IInteractable point)
        {
            if (_currentPoint != null || _currentPoint == point || !point.HasCharacter()) return;
            _currentPoint = point;
            characterData.DieInteractable += _currentPoint.GetDieCharacterDelegate;
            _removeList += _currentPoint.GetRemoveList();
            _addList += _currentPoint.GetAddList();
            base.SetCurrentPoint(point);
        }

        public override void SetOutline(bool value)
        {
            for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
            {
                skinnedMeshRenderer.materials[i].SetFloat("Vector1_2A6393C8", value ? outlineValue : 2f);
            }
        }

        protected void Die()
        {
            //   CharacterDataSubscriber.DieEvent -= Die;
            _removeList?.Invoke(this);
        }

        protected void RemoveCharacter() => _removeList?.Invoke(this);
        protected void AddCharacter() => _addList?.Invoke(this);


        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnsubscribeDeath();
        }

    }
}