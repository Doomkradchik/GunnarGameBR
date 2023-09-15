using System.Collections;
using System.Threading.Tasks;
using Characters.Animations;
using DG.Tweening;
using Interaction;
using MapSystem.Structs;
using UnityEngine;
using Characters.Sounds;

namespace Characters.Player.States
{
    public class DieEnemy : Die
    {
        private Money _moneyPrefab;
        private Transform _player;
        private int _moneyCountAfterDeath;

        public DieEnemy()
        {
        }

        public DieEnemy(IAnimationCommand animation, IAudioCommand audio, View view,
            CharacterController characterController,
            VFXTransforms vfxTransforms) : base(
            animation, audio, view, characterController, vfxTransforms)
        {
            _animation = animation;
        }

        public void SetMoneyData(MoneyData data)
        {
            _moneyPrefab = data.prefab;
            _moneyCountAfterDeath = data.moneyAfterDeath;
        }

        public void SetPlayerTransform(Transform player)
        {
            _player = player;
        }

        public override void Enter()
        {
            base.Enter();
            characterController.GetComponent<MonoBehaviour>().StartCoroutine(DieTime());
        }

        private IEnumerator DieTime()
        {
            characterController.GetComponent<CapsuleCollider>().enabled = false;
            yield return new WaitForSeconds(_animation.LengthAnimation(_parameterName) / 3);
            if (_moneyPrefab != null)
                SpawnMoneysCount(_moneyCountAfterDeath);

            yield return new WaitForSeconds(_animation.LengthAnimation(_parameterName));
            characterController.transform.DOMoveY(characterController.transform.position.y - 2, 10f)
                .OnComplete(()=>Object.Destroy(characterController.gameObject));
        }

        private void SpawnMoneysCount(int amount)
        {
            var angle = 360f / amount;
            for (int i = 0; i < amount; i++)
            {
                var direction = new Vector2(Mathf.Cos(angle * i), Mathf.Sin(angle * i));
                var money = Object.Instantiate(_moneyPrefab, _vfxTransforms.Center.position, Quaternion.identity);
                money.Init(_player, direction);
            }
        }
    }
}
public struct MoneyData
{
    public Money prefab;
    public int moneyAfterDeath;
}