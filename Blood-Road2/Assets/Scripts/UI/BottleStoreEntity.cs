using Scriptable_objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class BottleStoreEntity : StoreEntity, IPerformable<BottleStoreEntityInfo>
{
    [SerializeField] private TMP_Text textButton;
    [SerializeField] private string key;
    [SerializeField] private View view;

    private Color[] startColors;
    private Image _buttonImage;

    [System.Serializable]
    public struct View
    {
        public Image icon;
        public TMP_Text title;
        public TMP_Text description;
        public TMP_Text price;
    }

    private void Awake()
    {
        _buttonImage = buyButton.GetComponent<Image>();
        startColors = new Color[2] { _buttonImage.color, textButton.color };
    }

    protected override void Start()
    {
        base.Start();
        Bought += (_) => FetchBottle();
    }

    protected override void OnUpdatedMoney(int currentMoneyAmount)
    {
        var canBuy = CanBuy(currentMoneyAmount);
        _buttonImage.color = canBuy ? startColors[0] : Color.red;
        textButton.color = canBuy ? startColors[1] : Color.red;
    }

    private void FetchBottle()
    {
        var bottle = new Bank.Bottle();
        bottle.Initialize(key);
        bottle.Delegates.Add.Invoke(1);
        Debug.Log("Bottle Added");
    }

    public void Perform(BottleStoreEntityInfo data)
    {
        view.icon.sprite = data.Icon;
        view.title.text = $"{data.NameEntity} {data.Lvl} lvl.";
        view.description.text = $"+{data.Count} {data.Type}/{data.Sec} sec.";
        view.price.text = price.ToString();
    }
}