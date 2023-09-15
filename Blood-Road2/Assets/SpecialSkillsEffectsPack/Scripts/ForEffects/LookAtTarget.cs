using UnityEngine;

namespace Effects
{
    public class LookAtTarget : MonoBehaviour
    {
        public Transform Target;

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(Target);
        }
    }
}
