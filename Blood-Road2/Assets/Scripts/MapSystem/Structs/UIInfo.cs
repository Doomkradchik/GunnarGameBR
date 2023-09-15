using System;
using UnityEngine;

namespace MapSystem.Structs
{
    [Serializable]
    public struct UIInfo
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int Cooldown { get; private set; }


        public UIInfo(UIInfo uiInfo)
        {
            Name = uiInfo.Name;
            Description = uiInfo.Description;
            Sprite = uiInfo.Sprite;
            Cooldown = uiInfo.Cooldown;
        }

        public UIInfo(string name, string description, Sprite sprite, int cooldown)
        {
            Name = name;    
            Description = description;
            Sprite = sprite;
            Cooldown = cooldown;
        }
    }
}