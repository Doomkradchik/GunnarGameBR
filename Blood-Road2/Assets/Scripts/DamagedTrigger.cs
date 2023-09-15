using System.Collections.Generic;
using System.Threading.Tasks;
using Characters;
using Characters.EffectSystem;
using Characters.States;
using UnityEngine;

public class DamagedTrigger : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int waitFromActivated;
    [SerializeField] private int repeatActivate;
    [SerializeField] private ExplosionParameters explosionParameters;
    [SerializeField] private bool drone;
    [SerializeField] private bool isExplosion;
    private List<IInteractable> _interactables;
    private const float DEFAULT_SPEED = 25f;
    private const float DEFAULT_RADIUS = 8f;

    private void Awake()
    {
        _interactables = new List<IInteractable>();
        Active();
    }

    private ExplosionParameters Validate(ExplosionParameters explosionParameters)
    {
        explosionParameters.Radius = explosionParameters.Radius == 0f ? DEFAULT_RADIUS : explosionParameters.Radius;
        explosionParameters.Speed = explosionParameters.Speed == 0f ? DEFAULT_SPEED : explosionParameters.Speed;
        return explosionParameters;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out IInteractable interactable)) return;
        if (_interactables.Contains(interactable)) return;
        if (interactable.IsPlayer()) return;
        if (interactable.HasCharacter())
            if (drone)
                return;
        _interactables.Add(interactable);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out IInteractable interactable)) return;
        if (interactable.IsPlayer()) return;
        if (drone) interactable.TakeDamage(new EffectData(damage, 0, 0, 0, 0, null));
        if (_interactables.Contains(interactable)) return;
        if (interactable.HasCharacter())
            _interactables.Add(interactable);
    }

    private async void Active()
    {
        for (var i = 0; i < repeatActivate; i++)
        {
            await Task.Delay(waitFromActivated);
            foreach (var interactable in _interactables)
            {
                interactable.TakeDamage(new EffectData(damage, 0, 0, 0, 0, null));
               if(isExplosion) interactable.GetRecoil(transform.position, Validate(explosionParameters));
            }
        }
    }
}