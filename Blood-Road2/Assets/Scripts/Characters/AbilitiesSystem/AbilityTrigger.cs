using UnityEngine;

namespace Characters.AbilitiesSystem
{
    public class AbilityTrigger : CameraTrigger
    {
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            _triggerable.AbilityTrigger();
        }
    }
}