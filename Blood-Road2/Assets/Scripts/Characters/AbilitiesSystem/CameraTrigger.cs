using Cinemachine;
using UnityEngine;

namespace Characters.AbilitiesSystem
{
    public class CameraTrigger : MonoBehaviour
    {
        [SerializeField] private bool bossCamera;
        private CameraController _cameraController;
        protected ITriggerable _triggerable;

        public void SetCameraController(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out ITriggerable triggerable)) return;
            _triggerable = triggerable;
            if(_cameraController==null)return;
            if (bossCamera) _cameraController.BossCamera(true);
            else _cameraController.AbilityCamera(true);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out ITriggerable _)) return;
            if(_cameraController==null)return;
            if (bossCamera) _cameraController.BossCamera(false);
            else _cameraController.AbilityCamera(false);
            gameObject.SetActive(false);
        }
    }
}