using Characters.InteractableSystems;

namespace Banks
{
    public struct BankDelegates
    {
        public Add Add { get; }
        public Remove Remove { get; }
        public SetValue SetValue { get; }
        public IInit<GetValue> InitGetValue { get; }

        public BankDelegates(Add add, Remove remove, SetValue setValue, IInit<GetValue> getValue)
        {
            InitGetValue = getValue;
            Add = add;
            Remove = remove;
            SetValue = setValue;
        }
    }
}