using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.CombatHUD
{
    public class ActionButtonBase : MonoBehaviour
    {
        [SerializeField] private Button button;

        public void Initialize(UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => StartCoroutine(InteractableButton(action)));
        }

        private IEnumerator InteractableButton(UnityAction action)
        {
            if (button == null) yield break;
            var scale = button.transform.localScale;
            var runTween = DOTween.Sequence()
                .Append(button.transform.DOScale(new Vector3(scale.x - 0.1f, scale.y - 0.1f, scale.z), 0.1f))
                .Append(button.transform.DOScale(new Vector3(scale.x, scale.y, scale.z), 0.1f));
            runTween.Play();
            runTween.OnComplete(() => action?.Invoke());
        }
    }
}