using MapSystem.Structs;
using TMPro;
using UI.CombatHUD;
using UnityEngine;
using UnityEngine.UI;

namespace UI.AbilityChangedPanel
{
    public class AbilityInfo : MonoBehaviour
    {
        [SerializeField] private ActionButtonBase button;
        [Space] [SerializeField] private Image image;
        [SerializeField] private new TextMeshProUGUI name;
        [SerializeField] private TextMeshProUGUI description;

        public ActionButtonBase Button => button;

        private GUIKeyWorlds keyWorlds;
        

        public void SetInfo(UIInfo abilityUIInfo, int costMana)
        {
            image.sprite = abilityUIInfo.Sprite;
            name.text = abilityUIInfo.Name;
            var main = abilityUIInfo.Description;
            if (description == null || main == "") return;
            var update = $"Обновление: {abilityUIInfo.Cooldown} сек.";
            var cost = $"Стоимость: {costMana} маны.";
            description.text = $"{main}\n{update}\n{cost}";
        }
    }
}