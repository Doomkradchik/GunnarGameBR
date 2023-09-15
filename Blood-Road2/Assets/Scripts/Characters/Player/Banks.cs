using Bank;
using Banks;
using UnityEngine;

namespace Characters.Player
{
    public class Banks : MonoBehaviour
    {
        [SerializeField] private Money money;
        public BankDelegates MoneyBankDelegates => money.Delegates;

        public void Initialize()
        {
            money.Initialize("money");
        }
    }
}