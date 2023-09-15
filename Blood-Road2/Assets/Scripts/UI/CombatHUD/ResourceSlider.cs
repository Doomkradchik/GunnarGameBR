using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CombatHUD
{
    [RequireComponent(typeof(Slider))]
    public class ResourceSlider : MonoBehaviour
    {
        [SerializeField] [NotNull] private Slider resourceSlider;

        private void Start()
        {
            if (resourceSlider == null) resourceSlider = GetComponent<Slider>();
        }

        public void SetValue(float value, float maxValue)
        {
            if (resourceSlider == null) return;
            resourceSlider.maxValue = maxValue;
            resourceSlider.DOValue(value, 0.2f);
        }
    }
}