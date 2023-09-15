using System.Collections.Generic;
using Characters;
using Characters.Player;
using MapSystem;
using UnityEngine.UI;
using UnityEngine;

namespace Spawners
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy", fileName = "Enemy", order = 1)]
    public class EnemyData : ScriptableObject
    {
        public string EnemyClass;
        [SerializeField] private BaseCharacter character;
        [SerializeField] private CharacterData data;
        [SerializeField] private List<MapperItem> states;
        [SerializeField] private Sprite icon;
        [SerializeField] private int moneyForDeath = 5;
        public BaseCharacter Character => character;
        public List<MapperItem> States => new(states);
        public CharacterData Data => data;

        public void AddState(MapperItem state)
        {
            if (states.Contains(state))
            {
                Debug.Log($"{name}: Full states");
                return;
            }
            states.Add(state);
        }

        public Sprite Image { get { return icon; }}
        public int MoneyForDeath { get { return moneyForDeath; }}
    }
}