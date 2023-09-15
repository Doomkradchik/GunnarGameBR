using UnityEngine;

namespace Scriptable_objects
{
    [CreateAssetMenu(fileName = "NewStoreEntity")]
    public class BottleStoreEntityInfo : ScriptableObject
    {
        [Header("VIEW")]
        [SerializeField] private Sprite icon;
        [SerializeField] private string nameEntity;
        [SerializeField] private string type;
        [Header("DETAILs")]
        [SerializeField] private int lvl;
        [SerializeField] private int count;
        [SerializeField] private int sec;

        public Sprite Icon { get { return icon; } }
        public string Type { get { return type; } }
        public string NameEntity { get { return nameEntity; } }
        public int Lvl { get { return lvl; } }
        public int Count { get { return count; } }
        public int Sec { get { return sec; } }
    }
}

