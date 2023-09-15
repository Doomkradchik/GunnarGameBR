using UnityEngine.UI;
using Banks;
using UnityEngine;
using UnityEngine.Events;
using Bank;
using System;

public interface IBankInitable<T> where T: BaseBank
{
    void Init(T bank);
}

public class StoreEntity : MoneyHandler
{
    [SerializeField] protected Button buyButton;
    [SerializeField] protected int price;

    public event Action<int> Bought;
    public event Action<int> Failed;

    protected virtual void Start()
    {
        _money.Delegates.InitGetValue.Subscribe(OnUpdatedMoney);
        buyButton.onClick.AddListener(TryBuy);
        OnUpdatedMoney(_money.GetCurrentValue());
    }

    private void OnDestroy()
    {
        _money.Delegates.InitGetValue.Unsubscribe(OnUpdatedMoney);
        buyButton.onClick.RemoveListener(TryBuy);
    }

    protected bool CanBuy(int amount) => price <= amount;

    protected virtual void OnUpdatedMoney(int currentMoneyAmount) { Debug.Log(""); }

    private void TryBuy()
    {
        var amount = _money.GetCurrentValue();
        if(CanBuy(amount))
        {
            _money.Delegates.Remove.Invoke(price);
            Bought?.Invoke(_money.GetCurrentValue());
            Debug.Log($"Bought entity: {_money.GetCurrentValue()} remains");
            return;
        }
        Debug.Log($"Failed: {_money.GetCurrentValue()} remains");
        Failed?.Invoke(amount);
    }
}
