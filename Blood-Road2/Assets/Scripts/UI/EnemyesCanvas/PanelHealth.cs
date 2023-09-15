using UI.CombatHUD;
using UnityEngine;

namespace UI.EnemyesCanvas
{
    public class PanelHealth : MonoBehaviour
    {
        [SerializeField] private ResourceSlider health;
        [SerializeField] private FollowPointPanel followPointPanel;
        public ResourceSlider Health => health;
        public FollowPointPanel FollowPointPanel => followPointPanel;

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}