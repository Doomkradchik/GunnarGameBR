using UnityEngine.UI;
using Banks;
using UnityEngine;
using Scriptable_objects;
using Bank;
using System;
using UnityEngine.Events;

public class MoneyHandler : MonoBehaviour, IBankInitable<Money>
{
    protected Money _money;
    public void Init(Money bank)
    {
        _money = bank;
    }
}