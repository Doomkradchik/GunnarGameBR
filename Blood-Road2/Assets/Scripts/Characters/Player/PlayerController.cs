using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Banks;
using Characters.AbilitiesSystem;
using Characters.BottlesSystem;
using Characters.EffectSystem;
using Characters.Facades;
using Characters.InteractableSystems;
using Dreamteck.Splines;
using MapSystem;
using UI;
using UnityEngine;

namespace Characters.Player
{
    public delegate void ListOperation(IInteractable interactable);

    public delegate void AbilityTrigger();

    [RequireComponent(typeof(Banks))]
    public class PlayerController : BaseCharacter, 
        ITriggerable, 
        IInit<AbilityTrigger>,
        IAppendListProvider
    {
        [SerializeField] private CameraRay cameraRay;
        [SerializeField] private GameCanvasController canvasController;
        [SerializeField] private SplineFollower splineFollower;
        [SerializeField] private SplineProjector projector;
        [SerializeField] private AttackVariants attackVariants;
        [SerializeField] private Banks banks;
        private IInit<Attack> _initAttack;
        private IInit<SetAttackSpeed> _initSetAttackSpeed;
        private IInit<CanAttack> _canAttack;
        private List<IInteractable> _interactables;

        public event BottleUse BottleUseEvent;
        private event AbilityTrigger _abilityTrigger;


        #region Delegates

        private GetIsAttack _getIsAttack;

        #endregion

        #region ClassesNotSerializables

        private EnemyOutlineRechanger _enemyOutlineRechanger;

        #endregion

        private bool _isAttack;
        private bool GetIsAttack() => _isAttack;
        public override bool IsPlayer() => true;
        public BankDelegates MoneyBankDelegates => banks.MoneyBankDelegates;

        protected override void Awake()
        {
            banks.Initialize();
            _interactables = new List<IInteractable>();
            _enemyOutlineRechanger = new EnemyOutlineRechanger();
            _getIsAttack = GetIsAttack;
            base.Awake();
            SetCharacterData(characterData);
        }


        private void Start()
        {
            InitializeTransition(new PlayerTransition(), _getIsAttack, splineFollower, projector);
            InitializeInteractionSystem(cameraRay);
            SubscribeCharacterData();
            InitializeAbility(new AbilityData(VFXTransforms, characterData.ImpenetrableDelegate,
                mapStates, iDCharacter));
            SubscribeDeath();
            InitializeAttackDelegates();
        }

        private void InitializeAttackDelegates()
        {
            _initAttack = attackVariants;
            _canAttack = attackVariants;
            _initSetAttackSpeed = attackVariants;
            _initAttack.Subscribe(Attack);
            _initAttack.Subscribe(_transitionAndStates.Attack);
            _initSetAttackSpeed.Subscribe(_transitionAndStates.SetAttackSpeed);
            _canAttack.Subscribe(_transitionAndStates.CanAttack);
        }

        private void UnsubscibeAttackDelegates()
        {
            _initAttack.Unsubscribe(Attack);
            _initAttack.Unsubscribe(_transitionAndStates.Attack);
            _initSetAttackSpeed.Unsubscribe(_transitionAndStates.SetAttackSpeed);
            _canAttack.Unsubscribe(_transitionAndStates.CanAttack);
        }

        protected override void InitializeAbility(AbilityData abilityData)
        {
            var data = _transitionAndStates.ReturnReadyData(abilityData);
            _transitionAndStates.InitializeAbilities(new AbilitiesSystem.Player(data));
        }

        protected override void SubscribeDeath()
        {
            base.SubscribeDeath();
            characterData.DieEvent += canvasController.Death;
        }

        public override void Finish()
        {
            canvasController.OnLevelCompleted();
            _transitionAndStates.IsStoped = true;
        }

        public void AbilityTrigger()
        {
            _abilityTrigger?.Invoke();
            _transitionAndStates.IsStoped = true;
        }

        public void AddMoney(int value)
        {
            Debug.Log("Add money");
            banks.MoneyBankDelegates.Add?.Invoke(value);
            StatsCounter.Instance.AddMoney(value);
        }

        public void OnAbilityTrigger()
        {
            _transitionAndStates.IsStoped = false;
        }

        public void UseBottle(EffectData data)
        {
            if (!_hasCharacter) return;
            characterData.AddEnergy(data.EnergyAdd);
            characterData.AddHealth(data.HealthAdd);
            characterData.AddMana(data.ManaAdd);
        }

        protected override void ClearPoint(IInteractable interactable)
        {
            if (!_hasCharacter || _currentPoint == null) return;
            characterData.DieInteractable -= interactable.GetDieCharacterDelegate;
            _interactables.Remove(interactable);
            if (_currentPoint != interactable) return;
            _currentPoint = null;
            _enemyOutlineRechanger.SetEnemy(null);
            StartCoroutine(RechangeCurrentPoint());
        }

        protected override void RemoveList(IInteractable enemy)
        {
            if (!_hasCharacter) return;

            if (_interactables.Contains(enemy))
            {
                _interactables.Remove(enemy);
                enemy.InitDie().Unsubscribe(characterData.DieInteractable);
            }
        }

        protected override void AddList(IInteractable enemy)
        {
            if (!_hasCharacter) return;

            if (_interactables.Contains(enemy))
            {
                _interactables.Add(enemy);
                enemy.InitDie().Subscribe(characterData.DieInteractable);
            }
        }

        protected override void StartRCP(List<IInteractable> points)
        {
            if (!_hasCharacter) return;
            AddToListEnemy(points);
        }

        private void AddToListEnemy(List<IInteractable> enemies)
        {
            if (!_hasCharacter) return;
            foreach (var enemy in enemies)
            {
                if (!enemy.HasCharacter()) continue;
                if (_interactables.Contains(enemy)) continue;
                if (enemy.IsPlayer()) continue;
                if (enemy.Enabled == false) continue;
                characterData.DieInteractable += enemy.GetDieCharacterDelegate;
                _interactables.Add(enemy);
            }

            StartCoroutine(RechangeCurrentPoint());
        }

        private IEnumerator RechangeCurrentPoint()
        {
            if (!_hasCharacter) yield break;
            if (_currentPoint == null)
            {
                if (_interactables.Count == 0) yield break;
                int indx;
                IInteractable point = null;
                for (int i = 0; i < _interactables.Count; i++)
                {
                    if (!_interactables[i].IsPlayer())
                    {
                        indx = i;
                        for (int j = 0; j < _interactables.Count; j++)
                        {
                            if (Vector3.Distance(transform.position, _interactables[j].GetObject().position) <=
                                Vector3.Distance(transform.position, _interactables[indx].GetObject().position))
                            {
                                indx = j;
                            }
                        }

                        point = _interactables[indx];
                    }
                }

                if (point != null && point.HasCharacter())
                {
                    SetCurrentPoint(point);
                }

                yield return new WaitForSeconds(0);
            }
        }

        protected override void SetCurrentPoint(IInteractable point)
        {
            if (!_hasCharacter) return;
            if (point.IsPlayer()) return;

            if (_currentPoint == point) return;
            _currentPoint = point;
            _enemyOutlineRechanger.SetEnemy(_currentPoint);
            base.SetCurrentPoint(point);
        }

        private async void Attack(Item info)
        {
            if (characterData.Energy < 15) return;
            _isAttack = true;
            await Task.Delay(SecondToMilliseconds(info.View.Animation.length / 10));
            _isAttack = false;
        }

        private const int SECONDS = 1000;

        protected static int SecondToMilliseconds(float second)
        {
            var result = Mathf.RoundToInt(second * SECONDS);
            return result;
        }

        public override void SetOutline(bool value)
        {
        }

        public override void UseAbility(IAbilityCommand abilityCommand, int value)
        {
            if (!_hasCharacter) return;
            if (characterData.Mana <= value) return;
            characterData.UseMana(value);
            base.UseAbility(abilityCommand, value);
        }

        public void Subscribe(AbilityTrigger subscriber)
        {
            _abilityTrigger += subscriber;
        }

        public void Unsubscribe(AbilityTrigger unsubscriber)
        {
            _abilityTrigger -= unsubscriber;
        }

        protected override void OnDestroy()
        {
            UnsubscibeAttackDelegates();
            base.OnDestroy();
        }

    }
}