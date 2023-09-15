using Characters.InteractableSystems;
using MapSystem;
using UI.CombatHUD;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.AbilityChangedPanel
{
    public class AbilityRemovePanel : MonoBehaviour, IInit<UnityAction>, IInit<GamePanel>
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private AbilityInfo[] abilitiesInfo;
        [SerializeField] private ActionButtonBase continueButton;
        private AbilitiesButtons _abilitiesButtons;
        private UnityAction _abilityVariants;
        private event GamePanel _gamePanel;
        public CanvasGroup CanvasGroup => canvasGroup;

        private void Start()
        {
            continueButton.Initialize(OnContinue);
        }

        private void OnContinue()
        {
            _gamePanel?.Invoke();
        }

        public void SetAbilitiesButtons(AbilitiesButtons abilitiesButtons)
        {
            _abilitiesButtons = abilitiesButtons;
        }

        private void RemoveAbility(Item abilitySo)
        {
            _abilitiesButtons.RemoveAbility(abilitySo);
            _abilityVariants?.Invoke();
        }

        public void ViewActualAbilities()
        {
            var abilityList = _abilitiesButtons.GetCopy();
            for (int i = 0; i < abilitiesInfo.Length; i++)
            {
                var info = abilityList[i].UIInfo;
                var ability = abilityList[i];
                abilitiesInfo[i].SetInfo(info, ability.Ability.Cost);
                abilitiesInfo[i].Button.Initialize(() => RemoveAbility(ability));
            }
        }

        public void Subscribe(UnityAction subscriber)
        {
            _abilityVariants = subscriber;
        }

        public void Unsubscribe(UnityAction unsubscriber)
        {
            _abilityVariants = null;
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