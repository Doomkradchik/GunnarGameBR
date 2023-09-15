using System.Collections.Generic;
using Characters.AbilitiesSystem.Declaration;
using Characters.InteractableSystems;
using MapSystem;
using MapSystem.Structs;
using UI.CombatHUD;
using UnityEngine;

namespace UI.AbilityChangedPanel
{
    public class AbilityVariantsPanel : MonoBehaviour, IInit<GamePanel>
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private AbilityInfo[] abilitiesInfo;
        [SerializeField] private Placeholder placeholder;
        private AbilitiesButtons _abilitiesButtons;
        private event GamePanel _gamePanel;
        public CanvasGroup CanvasGroup => canvasGroup;

        public void SetAbilitiesButtons(AbilitiesButtons abilitiesButtons)
        {
            _abilitiesButtons = abilitiesButtons;
        }

        public void SetPanelsInfo()
        {
            var abilities = placeholder.TryGetList(new StateCharacterKey(0, null, typeof(AllAbilities)),
                out List<Item> allAbilities);
            if (!abilities) return;
            var actualAbilities = _abilitiesButtons.GetCopy();
            foreach (var ability in actualAbilities)
            {
                allAbilities.Remove(ability);
            }

            foreach (var abilityInfo in abilitiesInfo)
            {
                var ability = allAbilities[Random.Range(1, allAbilities.Count)];

                allAbilities.Remove(ability);
                abilityInfo.SetInfo(ability.UIInfo, ability.Ability.Cost);
                abilityInfo.Button.Initialize(() => SetNewAbility(ability));
            }
        }

        private void SetNewAbility(Item ability)
        {
            _abilitiesButtons.AddAbility(ability);
            _gamePanel?.Invoke();
        }

        public void Subscribe(GamePanel subscriber)
        {
            _gamePanel += subscriber;
        }

        public void Unsubscribe(GamePanel unsubscriber)
        {
            _gamePanel -= unsubscriber;
        }
    }
}