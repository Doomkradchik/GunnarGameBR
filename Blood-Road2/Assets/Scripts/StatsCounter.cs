using UnityEngine;
using Banks;
using Characters.InteractableSystems;

namespace Characters.Player
{
    public delegate void SetCurrentTimeInSeconds(int time);
    public class StatsCounter : MonoBehaviour, IInit<SetCurrentTimeInSeconds>
    {
        public static StatsCounter Instance { get; private set; }
        public int EnemiesKilled { get; private set; }
        public int MoneyEarned { get; private set; }
        public int TimeSpent { get; private set; }

        private GetValue _getValueMoney;
        private event SetCurrentTimeInSeconds _setTime;
        private void Awake()
        {
            EnemiesKilled = 0;
            MoneyEarned = 0;
            Instance = this;
            Subscribe((value) => TimeSpent = value);
        }

        public void AddMoney(int offset)
        {
            MoneyEarned += offset;
        }

        public void InitCharacterDataSub(ICharacterDataSubscriber characterDataSubscriber)
        {
            characterDataSubscriber.DieEvent += () => EnemiesKilled++;
        }

        private void Update()
        {
            _setTime.Invoke((int)Time.timeSinceLevelLoad);
        }

        public void Subscribe(SetCurrentTimeInSeconds subscriber)
        {
            _setTime += subscriber;
        }

        public void Unsubscribe(SetCurrentTimeInSeconds unsubscriber)
        {
            _setTime -= unsubscriber;
        }
    }
}

public struct Stats
{
    public int time;
    public int enemies;
    public int money;
}