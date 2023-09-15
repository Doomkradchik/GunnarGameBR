using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.InteractableSystems
{
    public delegate void SetPoint(List<IInteractable> point);

    public class Eyes : MonoBehaviour, IInit<SetPoint>
    {
        private event SetPoint _setPoint;
        private List<IInteractable> _interactables = new();
        private Coroutine _notification;

        public void Subscribe(SetPoint setPointDelegate)
        {
            _setPoint = setPointDelegate;
            setPointDelegate += _setPoint;
            StartCoroutine(Notification());
        }

        public void Unsubscribe(SetPoint unsubscriber)
        {
            _setPoint -= unsubscriber;
            StopCoroutine(_notification);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out IInteractable interactable)) return;
            if (_interactables.Contains(interactable)) return;
            if (interactable.HasCharacter())
                _interactables.Add(interactable);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out IInteractable interactable)) return;
            if (_interactables.Contains(interactable))
            {
                _interactables.Remove(interactable);
            }
        }

        private IEnumerator Notification()
        {
            for (;;)
            {
                for (var j = 0; j < _interactables.Count; j++)
                {
                    if (!_interactables[j].HasCharacter())
                        _interactables.Remove(_interactables[j]);
                }

                yield return new WaitForSeconds(0.1f);
                _setPoint?.Invoke(_interactables);
            }
        }
    }
}