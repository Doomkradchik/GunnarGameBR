using System.Collections.Generic;
using Better.SceneManagement.Runtime;
using Scriptable_objects;
using TMPro;
using UI.CombatHUD;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LocationInfo = Scriptable_objects.LocationInfo;

namespace UI.Scrolling
{
    public class LevelPerformer : MonoBehaviour, IPerformable<LocationInfo>
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI progress;
        [SerializeField] private TextMeshProUGUI locationName;
        [SerializeField] private TextMeshProUGUI moneyInLocation;
        [SerializeField] private List<Image> images;
        [SerializeField] private ActionButtonBase startLocation;
        [SerializeField] private TextMeshProUGUI lockedText;
        private LocationInfo _locationInfo;

        public void Perform(LocationInfo data)
        {
            this._locationInfo = data;
            SetInfo();
        }

        private void SetInfo()
        {
            var locationInfo = this._locationInfo.GetCopy();
            image.sprite = locationInfo.Sprite;
            progress.text = locationInfo.Progress;
            locationName.text = locationInfo.Name;
            moneyInLocation.text = $"+{locationInfo.MoneyInLocation}";
            for (var i = 0; i < locationInfo.Enemies.Count; i++)
            {
                images[i].sprite = locationInfo.Enemies[i];
            }

            startLocation.gameObject.SetActive(!locationInfo.Locked);
            lockedText.gameObject.SetActive(locationInfo.Locked);

            startLocation.Initialize(() =>
            {
                if (locationInfo.Scene != null || locationInfo.Locked)
                    SceneLoader.LoadScene(locationInfo.Scene, new LoadSceneOptions()
                    {
                        SceneLoadMode = LoadSceneMode.Single,
                        UseIntermediate = false
                    });
            });
        }
    }
}