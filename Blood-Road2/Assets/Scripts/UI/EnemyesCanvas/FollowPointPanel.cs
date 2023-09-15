using UnityEngine;

namespace UI.EnemyesCanvas
{
    public class FollowPointPanel: MonoBehaviour
    {
        private Transform _point;
        private bool _hasPoint;
        public void SetPoint(Transform point) => _point = point;

        private void Update()
        {
            if (_point == null && _hasPoint)
            {
                Destroy(gameObject);
            }
            if (_point != null)
            {
                transform.position = _point.position;
                _hasPoint = true;
            }
        }
    }
}