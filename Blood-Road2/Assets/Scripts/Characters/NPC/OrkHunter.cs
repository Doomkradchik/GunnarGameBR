using Characters.AbilitiesSystem;
using Characters.Facades;
using Characters.Player;
using System.Collections;
using UnityEngine;
using System;

namespace Characters.Enemy
{
    public class OrkHunter : DefaultEnemy
    {
        [SerializeField] private JumpFromOrkDataFX _jumpFX;
        private bool _characterInvoked;

        [Serializable]
        public struct JumpFromOrkDataFX
        {
            public float duration;
            public AnimationCurve verticalCurve;
        }

        protected override void OnSetCharacterData(CharacterData data)
        {
            base.Awake();
            SleepingDelegates sleepingDelegates = new SleepingDelegates
            {
                addList = AddCharacter,
                removeList = RemoveCharacter,
            };
            InitializeTransition(new OrkHunterTransition(() => _characterInvoked, sleepingDelegates),
                null, null, null, moneyPrefab, characterData.GetRecoilDelegate);
            InitializeAbility(new AbilityData(VFXTransforms, characterData.ImpenetrableDelegate,
                mapStates, iDCharacter));
            InitializeInteractionSystem(null);
            SubscribeDeath();
            SubscribeCharacterData();
            CharacterDataSubscriber.DieEvent += Die;
            StatsCounter.Instance.InitCharacterDataSub(CharacterDataSubscriber);
        }

        public void PushInvoke() => StartCoroutine(InvokeHunterRoutine());

        private IEnumerator InvokeHunterRoutine()
        {
            if (_currentPoint == null)
                yield break;

            _characterInvoked = true;
            characterData.OnEnable(true);
            Vector2 direction = (_currentPoint.GetObject().position - transform.position).ToXZPlane().normalized;
            Vector2 expectedPositionXZ = transform.position.ToXZPlane() + direction * 0.5f;
            yield return StartCoroutine(JumpFromOrkAnimationRoutine(_jumpFX.duration, _jumpFX.verticalCurve, expectedPositionXZ));
        }

        private IEnumerator JumpFromOrkAnimationRoutine(float duration, AnimationCurve verticalCurve,
            Vector2 endXZposition)
        {
            float progress = 0f;
            float expiredTime = 0f;
            Vector3 startPosition = transform.position;
            while(progress < 1f)
            {
                expiredTime += Time.deltaTime;
                progress = expiredTime / duration;
                Vector2 currentXZ = Vector3.Lerp(startPosition.ToXZPlane(), endXZposition, progress);
                float vertical = verticalCurve.Evaluate(progress) * startPosition.y;
                transform.position = new Vector3(currentXZ.x, vertical, currentXZ.y);
                yield return null;
            }
            Debug.Log("POSY: " + transform.position.y);
        }
    }
}