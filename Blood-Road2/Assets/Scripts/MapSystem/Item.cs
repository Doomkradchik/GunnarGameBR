using System;
using MapSystem.Structs;

namespace MapSystem
{
    [Serializable]
    public struct Item
    {
        public Ability Ability;
        public UIInfo UIInfo;
        public View View;

        public Item(Ability ability, UIInfo uiInfo, View view)
        {
            Ability = ability;
            UIInfo = uiInfo;
            View = view;
        }
    }
}