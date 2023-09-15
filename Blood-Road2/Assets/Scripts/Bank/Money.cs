using System;
using Banks;

namespace Bank
{
    [Serializable]
    public sealed class Money : BaseBank
    {
        public override void Initialize(string name)
        {
            NameBank = name;
            base.Initialize(name);
        }

        public int GetCurrentValue() => _value;
    }
}