using Characters.AbilitiesSystem;
using Characters.Facades;
using Characters.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters.Enemy.Bosses
{
    public sealed class DesertBoss : DefaultEnemy
    {
        [SerializeField] private int timeUnderground;
        [SerializeField] private float radius;
        private int _step = 1;

        private const int MAX_ULTIMATE_ABILITES_PERFORMACE_COUNT = 4;
        private bool _canGoUnder;


        private bool CanGoUnderground(bool use = false)
        {
            var result = _canGoUnder;
            if (use) _canGoUnder = false;
            return result;
        }

        protected override void OnSetCharacterData(CharacterData data)
        {
            base.Awake();
            InitializeTransition(new DesertBossTransition(CanGoUnderground, CalculateRandomPosition, timeUnderground),
                null, null, null, moneyPrefab, characterData.GetRecoilDelegate);
            InitializeAbility(new AbilityData(VFXTransforms, characterData.ImpenetrableDelegate,
                mapStates, iDCharacter));
            InitializeInteractionSystem(null);
            SubscribeDeath();
            SubscribeCharacterData();
            CharacterDataSubscriber.DieEvent += Die;
            CharacterDataSubscriber.HealthEvent += OnDamageRecevied;
            StatsCounter.Instance.InitCharacterDataSub(CharacterDataSubscriber);
        }

        private Vector3 CalculateRandomPosition(Vector3 playerPosition, Vector3 zone)
        {
            var direction = (Vector2)Random.insideUnitCircle.normalized;
            direction = direction.magnitude < 1f ? Vector2.one : direction;
            Vector2 xzPosition = zone.ToXZPlane() + direction * radius;
            xzPosition = Vector2.Distance(xzPosition, playerPosition.ToXZPlane()) < runToPointData.StopDistance ?
                zone.ToXZPlane() - direction * radius : xzPosition;
            return new Vector3(xzPosition.x, transform.position.y, xzPosition.y);
        }
       

        private void OnDamageRecevied(float currentHealth, float maxHealth)
        {
            var minHealthRequired = maxHealth - (maxHealth / MAX_ULTIMATE_ABILITES_PERFORMACE_COUNT) * _step;
            if (currentHealth <= minHealthRequired)
            {
                _canGoUnder = true;
                _step++;
            }     
        }       
    }
}


public static class Vector3Extension
{
    public static Vector2 ToXZPlane(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }
}