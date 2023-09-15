using Spawners;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scriptable_objects;

public class EncyclopediaEntityView : MonoBehaviour, IPerformable<EnemyData>
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private Sprite notFound;
    [SerializeField] private Vector2 size;

    public void Perform(EnemyData data)
    {
        tmpText.text = data.EnemyClass;
        if(data.Image == null)
        {
            image.GetComponent<RectTransform>().sizeDelta = size;
            image.sprite = notFound;
        }
        else
            image.sprite = data.Image;
    }
}
