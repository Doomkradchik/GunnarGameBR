using System.Collections.Generic;
using Characters.Facades;
using Characters.InteractableSystems;
using MapSystem;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.Player
{
    public delegate void Attack(Item info);

    public delegate void SetAttackSpeed(float value);

    public delegate void OverrideAttack(View view, bool state);

    public class AttackVariants : MonoBehaviour, IInit<Attack>, IInit<SetAttackSpeed>, IInit<CanAttack>
    {
        [SerializeField] private Placeholder placeholder;
        private List<Item> _attackVariants;
        private event Attack _attack;
        private event SetAttackSpeed _setAttackSpeed;
        private event CanAttack _canAttack;

        private void Start()
        {
            var key = new StateCharacterKey(0, typeof(States.Attack), null);

            if (!placeholder.TryGetList(key, out List<Item> items)) return;
            if (!placeholder.TryGetUIInfo(key, out UIInfo info)) return;
            if (!placeholder.TryGetAbility(key, out Ability ability)) return;
            if (!placeholder.TryGetView(key, out View view)) return;
            items.Add(new Item(ability, info, view));
            _attackVariants = items;
        }

        public bool Attack(int index)
        {
            if (_canAttack?.Invoke() == false) return false;
            _setAttackSpeed?.Invoke(_attackVariants[index].View.Speed);
            if (_attackVariants.Count <= index) return false;
            _attack?.Invoke(_attackVariants[index]);
            Debug.Log(_attackVariants[index].View.ID);
            return true;
        }

        public bool CanAttack()
        {
            if (_canAttack == null)
                throw new System.InvalidOperationException();

            return _canAttack.Invoke();
        }

        public void Subscribe(Attack subscriber)
        {
            _attack += subscriber;
        }

        public void Unsubscribe(Attack unsubscriber)
        {
            _attack -= unsubscriber;
        }

        public void Subscribe(SetAttackSpeed subscriber)
        {
            _setAttackSpeed += subscriber;
        }

        public void Unsubscribe(SetAttackSpeed unsubscriber)
        {
            _setAttackSpeed -= unsubscriber;
        }

        public void Subscribe(CanAttack subscriber)
        {
            _canAttack += subscriber;
        }

        public void Unsubscribe(CanAttack unsubscriber)
        {
            _canAttack -= unsubscriber;
        }
    }
}