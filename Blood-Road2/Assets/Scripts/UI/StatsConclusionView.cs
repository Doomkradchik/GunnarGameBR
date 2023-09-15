using Better.SceneManagement.Runtime;
using Characters.Player;
using TMPro;
using UI.CombatHUD;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class StatsConclusionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeTMP;
        [SerializeField] private TMP_Text enemiesTMP;
        [SerializeField] private TMP_Text moneyTMP;
        [SerializeField] private ActionButtonBase restart;
        [SerializeField] private ActionButtonBase menu;
        [SerializeField] private SceneLoaderAsset menuScene;
        [SerializeField] private SceneLoaderAsset currentScene;

        protected virtual void Awake()
        {
            if (restart != null) restart.Initialize(() => LoadScene(currentScene));
            if (menu != null) menu.Initialize(() => LoadScene(menuScene));
            if (timeTMP != null) timeTMP.text = "0";
            if (enemiesTMP != null) enemiesTMP.text = "0";
            if (moneyTMP != null) moneyTMP.text = "0";
        }

        private void LoadScene(SceneLoaderAsset scene)
        {
            SceneLoader.LoadScene(scene,
                new LoadSceneOptions() { SceneLoadMode = LoadSceneMode.Single, UseIntermediate = false });
        }

        public void Activate() => PerformValuesAnimation();

        private void PerformValuesAnimation()
        {

                int minutes = StatsCounter.Instance.TimeSpent / 60;
                int seconds = StatsCounter.Instance.TimeSpent % 60;

                if (timeTMP != null) timeTMP.text = $"{minutes:00}:{seconds:00}";
                
                if (enemiesTMP != null) enemiesTMP.text = StatsCounter.Instance.EnemiesKilled.ToString();
                
                if (moneyTMP != null) moneyTMP.text = StatsCounter.Instance.MoneyEarned.ToString();
            
        }
    }
}