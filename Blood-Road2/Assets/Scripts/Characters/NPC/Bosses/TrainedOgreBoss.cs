using Spawners;
using UnityEngine;
using Characters.Player;
using MapSystem.Placeholders;

namespace Characters.Enemy.Bosses
{

    public sealed class TrainedOgreBoss : DefaultEnemy, IInitableSpawn
    {
        [Header("Hunter")]
        [SerializeField] private Transform root;
        [SerializeField] private EnemyData hunterData;
        private BaseCharacter _hunter;
        private SpawnCharacter _spawn;

        public void Init(SpawnCharacter spawnRoutine)
        {
            _spawn = spawnRoutine;
        }

        protected override void OnSetCharacterData(CharacterData data)
        {
            base.OnSetCharacterData(data);
            CreateAndInitHunter();
        }

        private void CreateAndInitHunter()
        {
            _hunter = _spawn.Invoke(hunterData, Vector3.zero, hunterData.States);
            _hunter.transform.parent = root;
            _hunter.transform.localPosition = Vector3.zero;
            _hunter.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        protected override void SubscribeDeath()
        {
            base.SubscribeDeath();
            CharacterDataSubscriber.DieEvent += InvokeHunter;
        }

        protected override void UnsubscribeDeath()
        {
            base.UnsubscribeDeath();
            CharacterDataSubscriber.DieEvent -= InvokeHunter;
        }

        private void InvokeHunter()
        {
            _hunter.transform.SetParent(null);
            if (_hunter is not OrkHunter oh)
                throw new System.InvalidOperationException();

            oh.PushInvoke();
        }

    }
}
