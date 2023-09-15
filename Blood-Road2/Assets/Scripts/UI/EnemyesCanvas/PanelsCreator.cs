using Characters;
using UnityEngine;

namespace UI.EnemyesCanvas
{
    public class PanelsCreator : MonoBehaviour
    {
        [SerializeField] private PanelHealth prefab;
        [SerializeField] private Canvas current;

        private void Update()
        {
            if (current.worldCamera == null)
                current.worldCamera = Camera.main;
        }

        public void AddCharacter(BaseCharacter character)
        {
            var panel = Instantiate(prefab, transform);
            character.CharacterDataSubscriber.HealthEvent += panel.Health.SetValue;
            character.CharacterDataSubscriber.DieEvent += () =>
            {
                character.CharacterDataSubscriber.HealthEvent -= panel.Health.SetValue;
                panel.Destroy();
            };
            character.CharacterDataSubscriber.EnableEvent += (active) =>
            {
               // Debug.Log("enabled: " + active);
                panel.gameObject.SetActive(active);
            };
            panel.FollowPointPanel.SetPoint(character.VFXTransforms.Up);
        }
    }
}