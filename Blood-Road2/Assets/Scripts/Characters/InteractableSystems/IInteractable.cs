using Characters.EffectSystem;
using Characters.InteractableSystems;
using Characters.Player;
using Characters.States;
using UnityEngine;

namespace Characters
{
    public interface IAppendListProvider
    {
        public ListOperation GetAddList();
    }

    public interface IInteractable : IAppendListProvider
    {
        public bool Enabled { get; }
        public void GetRecoil(Vector3 origin, ExplosionParameters parameters);
        public bool TryAttackByWeapon(EffectData effectData, int costAttack, int delay);
        public void TakeDamage(EffectData effectData);
        public void SetOutline(bool value);
        public Transform GetObject();
        public bool IsPlayer();
        public DieInteractable GetDieCharacterDelegate { get; }
        public ListOperation GetRemoveList();
        public bool HasCharacter();
        public Receiver Receiver { get; }
        public IInit<DieInteractable> InitDie();

    }

    public interface ITriggerable
    {
        public void Finish();
        public void AbilityTrigger();
        public void AddMoney(int value);
    }
}