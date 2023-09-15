using System.Collections;
using System.Collections.Generic;
using Characters.Enemy;
using Characters.Facades;
using Characters.InteractableSystems;
using Characters.Player;
using UnityEngine;

namespace Characters.NPC
{
    public class WolfGhost : BaseCharacter
    {
        [SerializeField] private bool isPlayer;
        [SerializeField] private VFXEffect effect;
        private List<IInteractable> _interactables;
        protected event ListOperation _removeList;
        protected IInteractable _main;
        protected SupporterTransition _supporterTransitionAndStates;

        protected const float MAX_DISTANCE = 7.5f;
        protected const float ANGLE = 25f;


        protected override void Awake()
        {
            _main = FindObjectOfType<PlayerController>();
            _interactables = new List<IInteractable>();
            base.Awake();
            SetCharacterData(characterData);
            InitializeTransition(_supporterTransitionAndStates = new SupporterTransition(MAX_DISTANCE), null);
            SubscribeCharacterData();
            InitializeInteractionSystem(null);
            FollowEntity();
        }

        private void Start()
        {
            SubscribeDeath();
            CharacterDataSubscriber.DieEvent += Die;
            effect.SetCharacter(this);
        }

        protected override void Look(Vector3 turnTowardNavSteeringTarget)
        {
            var direction = (_currentPoint.GetObject().position - transform.position).normalized;
            var y = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)).eulerAngles.y;
            y = _currentPoint.IsPlayer() ? y + ANGLE * CalculateRatio(_currentPoint.GetObject()) : y;
            var resRotation = Quaternion.Euler(0f, y, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, resRotation, Time.deltaTime * rotationSpeed);
        }

        public void SetMainInteractable(IInteractable interactable) => _main = interactable;

        private float CalculateRatio(Transform towardTarget)
        {
            var posOnForwardTarget = Vector3.Project(transform.position, towardTarget.forward.normalized);
            var distance = Mathf.Abs(towardTarget.position.magnitude - posOnForwardTarget.magnitude);
            if (distance < 0.1f)
                return 0f;
            var ratio = MAX_DISTANCE / distance;
            return ratio > 1f ? 1f : ratio;
        }

        protected void FollowEntity()
        {
            if (!_hasCharacter) return;

            _supporterTransitionAndStates.SetMode(SupporterTransition.FollowMode.Player);
            SetCurrentPoint(_main);
        }

        protected void FollowEntity(IInteractable interactable)
        {
            if (!_hasCharacter) return;

            _supporterTransitionAndStates.SetMode(SupporterTransition.FollowMode.Enemy);
            SetCurrentPoint(interactable);
        }

        public override void SetOutline(bool value)
        {
        }

        public override bool IsPlayer() => isPlayer;

        protected override void ClearPoint(IInteractable interactable)
        {
            if (!_hasCharacter) return;

            if (_currentPoint == null) return;
            characterData.DieInteractable -= interactable.GetDieCharacterDelegate;
            _interactables.Remove(interactable);
            if (_currentPoint != interactable) return;
            _currentPoint = null;
            StartCoroutine(RechangeCurrentPoint());
        }

        protected override void RemoveList(IInteractable enemy)
        {
            if (!_hasCharacter) return;

            if (_interactables.Contains(enemy)) _interactables.Remove(enemy);
        }

        protected override void AddList(IInteractable interactable)
        {
            if (!_hasCharacter) return;

            if (_interactables.Contains(interactable)) _interactables.Add(interactable);
        }

        protected override void StartRCP(List<IInteractable> points)
        {
            if (!_hasCharacter) return;
            ClearPoint(_main);
            AddToListEnemy(points);
        }

        private void AddToListEnemy(List<IInteractable> enemies)
        {
            if (!_hasCharacter) return;
            foreach (var enemy in enemies)
            {
                if (!enemy.HasCharacter() || _interactables.Contains(enemy) || enemy.IsPlayer()) continue;
                characterData.DieInteractable += enemy.GetDieCharacterDelegate;
                _removeList += enemy.GetRemoveList();
                IInit<DieInteractable> initDie = (DefaultEnemy)enemy;
                initDie.Subscribe(GetDieCharacterDelegate);
                _interactables.Add(enemy);
            }

            StartCoroutine(RechangeCurrentPoint());
        }

        private IEnumerator RechangeCurrentPoint()
        {
            if (!_hasCharacter) yield break;
            if (_currentPoint == null)
            {
                if (_interactables.Count == 0)
                {
                    FollowEntity();
                    yield break;
                }

                int indx;
                IInteractable point = null;
                for (int i = 0; i < _interactables.Count; i++)
                {
                    if (_interactables[i] == null)
                    {
                        _interactables.Remove(_interactables[i]);
                    }

                    else if (_interactables[i].IsPlayer()) continue;

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

                if (point != null && point.HasCharacter())
                {
                    FollowEntity(point);
                }

                yield return new WaitForSeconds(0);
            }
        }

        protected override void SetCurrentPoint(IInteractable point)
        {
            if (!_hasCharacter) return;
            if (_currentPoint != null || _currentPoint == point || !point.HasCharacter()) return;

            _currentPoint = point;
            base.SetCurrentPoint(point);
        }

        private void Die()
        {
            if (!_hasCharacter) return;
            foreach (var interactable in _interactables)
            {
                characterData.DieInteractable -= interactable.GetDieCharacterDelegate;
            }

            _removeList?.Invoke(this);
        }
    }
}