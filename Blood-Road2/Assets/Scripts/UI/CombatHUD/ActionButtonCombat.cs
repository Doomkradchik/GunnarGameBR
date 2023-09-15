using System.Collections;
using Banks;
using DG.Tweening;
using MapSystem.Structs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Characters.Facades;

namespace UI.CombatHUD
{
    
    public sealed class ActionButtonCombat : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private Image cooldownMask;

        [Header("optional field")] [SerializeField]
        private TextMeshProUGUI currentCountText;

        private int _currentCount;

        public void Initialize(UnityAction action, UIInfo info, CanAttack canInteract = null)
        {
            image.sprite = info.Sprite;
            image.color = Color.white;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => StartCoroutine(InteractableButton(info.Cooldown, action, canInteract)));
        }

        public void Initialize(UnityAction action, UIInfo info, Remove remove, CanAttack canInteract = null)
        {
            Initialize(action, info, canInteract);
            currentCountText.text = _currentCount.ToString();
            button.onClick.AddListener((() => { remove?.Invoke(1); }));
        }

        public void SetValue(int value)
        {
            _currentCount = value;
            currentCountText.text = _currentCount.ToString();
            button.interactable = _currentCount > 0;
        }

        private IEnumerator InteractableButton(float cooldown, UnityAction action, CanAttack canInteract)
        {
            if (button == null) yield break;
            if (canInteract?.Invoke() == false) { yield break; }
            action?.Invoke();
            if (cooldown != 0)
            {
                var tweenFinished = true;
                button.interactable = false;
                var scale = button.transform.localScale;
                var runTween = DOTween.Sequence()
                    .Append(cooldownMask.DOFillAmount(1, 0.2f))
                    .Append(cooldownMask.DOFillAmount(0, cooldown))
                    .Append(button.transform.DOScale(new Vector3(scale.x + 0.1f, scale.y + 0.1f, scale.z), 0.2f))
                    .Append(button.transform.DOScale(new Vector3(scale.x, scale.y, scale.z), 0.2f));
                runTween.Play();
                runTween.OnComplete((() => tweenFinished = false));
                yield return new WaitWhile(() => tweenFinished);
            }

            button.interactable = true;
        }
    }
}