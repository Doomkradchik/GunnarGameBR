using Characters;
using UnityEngine;
using LocationInfo = Scriptable_objects.LocationInfo;

namespace Interaction
{
    public class FinishTrigger : MonoBehaviour
    {
        [SerializeField] private LocationInfo locationInfo;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITriggerable triggerable))
            {
                locationInfo.AddProgressValue(1);
                triggerable.Finish();
            }
        }
    }
}