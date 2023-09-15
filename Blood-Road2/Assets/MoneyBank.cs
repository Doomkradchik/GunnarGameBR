using UnityEngine.UI;
using Banks;
using UnityEngine;
using TMPro;
using Bank;
using System;

public class MoneyBank : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyCount;
    [SerializeField] private int toAddmoneyAmount;
    [SerializeField] private Money money;

    public static MoneyBank Instance { get; private set;}
    public Money MoneyInstance => money;
    private void Awake()
    {
        Instance = this;
        money.Initialize("money");
        money.Delegates.Add(toAddmoneyAmount);
        money.Delegates.InitGetValue.Subscribe(OnMoneyChanged);
        OnMoneyChanged(money.GetCurrentValue());
    }

    protected void OnMoneyChanged(int currentValue) => moneyCount.text = currentValue.ToString();
}
