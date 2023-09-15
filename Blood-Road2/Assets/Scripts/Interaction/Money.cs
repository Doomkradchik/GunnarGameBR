using System.Collections;
using System.Threading.Tasks;
using Characters;
using DG.Tweening;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Interaction
{
    public class Money : MonoBehaviour
    {
        [SerializeField] private int money;
        [Header("Spawn Drop Animation")]
        [SerializeField] private float verticalMax;
        [SerializeField] private AnimationCurve verticalCurve;
        [SerializeField] private float duration = 1f;
        [SerializeField] private float _maxDistanceFromSpawn = 3f;
        private Transform _myTransform;
        private Transform _player;
        private Sequence _sequence;
        private bool _isUsed;
        private bool _followToPlayer;
        private float _speed = 0.5f;
        private bool _inited;

        private Vector3 _startPosition;
        private Vector2 _randomXZTargetPosition;


        public void Init(Transform player, Vector2 direction)
        {
            _startPosition = transform.position;
            _randomXZTargetPosition = _startPosition.ToXZPlane() + direction * _maxDistanceFromSpawn * Random.Range(0.2f, 1f);
            _player = player;
            _myTransform = transform;
            _sequence = DOTween.Sequence()
                .Append
                (
                    _myTransform
                        .DOMoveY(_myTransform.position.y - 0.2f, 1)
                        .SetEase(Ease.InOutSine)
                )
                .Append
                (
                    _myTransform
                        .DORotate(new Vector3(0, 360, 0), 2f, RotateMode.WorldAxisAdd)
                        .SetEase(Ease.InOutSine)
                );
            StartCoroutine(CheckIsPlayer());
        }

        private IEnumerator CheckIsPlayer()
        {
            yield return StartCoroutine(DropAnimation());
            for (int i = 0;  _player != null; i++)
            {
                _sequence.Play();
                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator DropAnimation()
        {
            var targetPositionXZ = _randomXZTargetPosition;
            float progress = 0f;
            float expiredTime = 0f;
            while (progress < 1f)
            {
                expiredTime += Time.deltaTime;
                progress = expiredTime / duration;
                Vector2 currentXZ = Vector3.Lerp(_startPosition.ToXZPlane(), targetPositionXZ, progress);
                float vertical = verticalCurve.Evaluate(progress) * verticalMax;
                transform.position = new Vector3(currentXZ.x, _startPosition.y + vertical, currentXZ.y);
                yield return null;
            }
            _inited = true;
        }

        private void Update()
        {
            if (_player == null || _inited == false) return;
            var direction = transform.position - _player.position;
            transform.position -= direction * (_speed * Time.deltaTime);
            _speed += 0.2f;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_inited == false) { return; }
            if (!other.TryGetComponent(out ITriggerable triggerable)) return;
            if(_isUsed) { return; }
            triggerable.AddMoney(money);
            _sequence.Kill();
            transform.DOScale(0, 0.5f).OnComplete(() => Destroy(gameObject));
            _isUsed = true;
        }
    }
}