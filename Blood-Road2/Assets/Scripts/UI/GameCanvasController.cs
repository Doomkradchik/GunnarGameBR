using System;
using Banks;
using Characters;
using Characters.InteractableSystems;
using Characters.Player;
using TMPro;
using UI.AbilityChangedPanel;
using UI.CombatHUD;
using UnityEngine;

namespace UI
{
    public delegate void GamePanel();

    public class GameCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject postProcessingWorldMap;
        [SerializeField] private CanvasGroup levelCompleted;
        [SerializeField] private CanvasGroup combat;
        [SerializeField] private CanvasGroup death;
        [SerializeField] private CanvasGroup abilityChanged;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private ResourceMediator resourceMediator;
        private IInit<AbilityTrigger> _initAbilityTrigger;
        private IInit<GamePanel> _initGamePanel;
        private AbilityTrigger abilityTrigger;
        private RechangePanel _rechangePanel;


        private void Awake()
        {
            postProcessingWorldMap.SetActive(false);
            resourceMediator.SetCharacter(playerController);
            abilityTrigger = AbiltyChanged;
            _initAbilityTrigger = playerController;
            _initAbilityTrigger.Subscribe(abilityTrigger);
            abilityChanged.gameObject.TryGetComponent(out _initGamePanel);
            _initGamePanel.Subscribe(Game);
            _initGamePanel.Subscribe(playerController.OnAbilityTrigger);
            _rechangePanel = new RechangePanel();
            Game();
        }

        private void Start()
        {
            resourceMediator.Subscribe();
        }

        private void Game()
        {
            postProcessingWorldMap.SetActive(false);
            _rechangePanel.SetNewPanel(combat);
        }

        public void Death()
        {
            postProcessingWorldMap.SetActive(true);
            _rechangePanel.SetNewPanel(death);
            death.TryGetComponent(out StatsConclusionView completed);
            completed.Activate();
        }

        private void AbiltyChanged()
        {
            postProcessingWorldMap.SetActive(true);
            _rechangePanel.SetNewPanel(abilityChanged);
            abilityChanged.TryGetComponent(out AbilityChanged changed);
            changed.Activate();
        }

        public void OnLevelCompleted()
        {
            postProcessingWorldMap.SetActive(true);
            _rechangePanel.SetNewPanel(levelCompleted);
            levelCompleted.TryGetComponent(out StatsConclusionView completed);
            completed.Activate();
        }

        private void OnDestroy()
        {
            resourceMediator.Unsubscribe();
        }
    }

    [Serializable]
    public class ResourceMediator
    {
        [SerializeField] private ResourceSlider health;
        [SerializeField] private ResourceSlider mana;
        [SerializeField] private ResourceSlider energy;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI timeText;
        private BaseCharacter _character;
        private ICharacterDataSubscriber _characterDataSubscriber => _character.CharacterDataSubscriber;
        private IInit<GetValue> _initGetValue;

        public void SetCharacter(BaseCharacter character)
        {
            _character = character;
            var characterPlayer = (PlayerController)_character;
            _initGetValue = characterPlayer.MoneyBankDelegates.InitGetValue;
        }

        public void Subscribe()
        {
            _characterDataSubscriber.HealthEvent += health.SetValue;
            _characterDataSubscriber.ManaEvent += mana.SetValue;
            _characterDataSubscriber.EnergyEvent += energy.SetValue;
            _initGetValue.Subscribe((value)=>moneyText.text = value.ToString() );
            StatsCounter.Instance.Subscribe((value) =>
            {
                int minutes = value / 60;
                int seconds = value % 60;

                timeText.text = $"{minutes:00}:{seconds:00}";
            });
        }

        public void Unsubscribe()
        {
            _characterDataSubscriber.HealthEvent -= health.SetValue;
            _characterDataSubscriber.ManaEvent -= mana.SetValue;
            _characterDataSubscriber.EnergyEvent -= energy.SetValue;
            _initGetValue.Unsubscribe((value)=>moneyText.text = value.ToString() );
            StatsCounter.Instance.Unsubscribe((value) =>
            {
                int minutes = value / 60;
                int seconds = value % 60;

                timeText.text = $"{minutes:00}:{seconds:00}";
            });
        }
    }
}