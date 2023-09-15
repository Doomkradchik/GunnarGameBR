using Characters.InteractableSystems;
using UI.CombatHUD;
using UnityEngine;
using UnityEngine.Events;

namespace UI.AbilityChangedPanel
{
    public class AbilityChanged : MonoBehaviour, IInit<GamePanel>
    {
        [SerializeField] private AbilitiesButtons abilitiesButtons;
        [SerializeField] private AbilityVariantsPanel abilityVariantsPanel;
        [SerializeField] private AbilityRemovePanel abilityRemovePanel;
        private IInit<GamePanel> _initGamePanel;
        private IInit<UnityAction> _initUnityAction;
        private RechangePanel _rechangePanel;

        private void Start()
        {
            _rechangePanel = new RechangePanel();
            _initUnityAction = abilityRemovePanel;
            _initUnityAction.Subscribe(Activate);
            abilityVariantsPanel.SetAbilitiesButtons(abilitiesButtons);
            abilityRemovePanel.SetAbilitiesButtons(abilitiesButtons);
        }

        public void Activate()
        {
            var abilityList = abilitiesButtons.GetCopy();
            if (abilityList.Count >= 4)
            {
                abilityRemovePanel.ViewActualAbilities();
                _rechangePanel.SetNewPanel(abilityRemovePanel.CanvasGroup);
            }
            else
            {
                abilityVariantsPanel.SetPanelsInfo();
                _rechangePanel.SetNewPanel(abilityVariantsPanel.CanvasGroup);
            }
        }

        public void Subscribe(GamePanel subscriber)
        {
            _initGamePanel = abilityRemovePanel;
            _initGamePanel.Subscribe(subscriber);
            _initGamePanel = abilityVariantsPanel;
            _initGamePanel.Subscribe(subscriber);
        }

        public void Unsubscribe(GamePanel unsubscriber)
        {
            _initGamePanel = abilityRemovePanel;
            _initGamePanel.Unsubscribe(unsubscriber);
            _initGamePanel = abilityVariantsPanel;
            _initGamePanel.Unsubscribe(unsubscriber);
        }
    }
}