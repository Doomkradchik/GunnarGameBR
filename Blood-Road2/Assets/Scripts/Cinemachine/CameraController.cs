using UnityEngine;

namespace Cinemachine
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private TriggerAddPriority[] triggerAddPriorities;
        [SerializeField] private CinemachineVirtualCamera abilityCamera;
        [SerializeField] private CinemachineVirtualCamera bossCamera;
        private CinemachineVirtualCamera _currentCinemachineVirtualCamera;

        private void Start()
        {
            foreach (var trigger in triggerAddPriorities)
            {
                trigger.Initialize(this);
            }
        }

        public void SetCamera(CinemachineVirtualCamera newCinemachineVirtualCamera)
        {
            if (_currentCinemachineVirtualCamera != null)
            {
                _currentCinemachineVirtualCamera.Priority = 0;
            }

            _currentCinemachineVirtualCamera = newCinemachineVirtualCamera;
            _currentCinemachineVirtualCamera.Priority = 1;
        }

        public void AbilityCamera(bool value)
        {
            if (value)
            {
                if (_currentCinemachineVirtualCamera != null) _currentCinemachineVirtualCamera.Priority = 0;
                if (abilityCamera != null) abilityCamera.Priority = 1;
            }
            else
            {
                if (_currentCinemachineVirtualCamera != null) _currentCinemachineVirtualCamera.Priority = 1;
                if (abilityCamera != null) abilityCamera.Priority = 0;
            }
        }

        public void BossCamera(bool value)
        {
            if (value)
            {
                if (_currentCinemachineVirtualCamera != null) _currentCinemachineVirtualCamera.Priority = 0;
                if (bossCamera != null) bossCamera.Priority = 1;
            }
            else
            {
                if (_currentCinemachineVirtualCamera != null) _currentCinemachineVirtualCamera.Priority = 1;
                if (bossCamera != null) bossCamera.Priority = 0;
            }
        }
    }
}