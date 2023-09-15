using Characters;
using Cinemachine;
using UnityEngine;

public class TriggerAddPriority : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CameraController _controller;
    public void Initialize(CameraController controller) => _controller = controller;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            if (interactable.IsPlayer())
            {
                _controller.SetCamera(cinemachineVirtualCamera);
            }
        }
    }
}