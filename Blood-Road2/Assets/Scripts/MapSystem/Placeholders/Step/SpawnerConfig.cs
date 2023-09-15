using System;
using System.Collections.Generic;
using Characters;
using Characters.Enemy;
using Characters.Player;
using Spawners;
using UI.EnemyesCanvas;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MapSystem.Placeholders
{
    public interface IInitableSpawn
    {
        void Init(SpawnCharacter spawnRoutine);

    }
    public delegate BaseCharacter SpawnCharacter(EnemyData enemyData, Vector3 position, List<MapperItem> states);
}

namespace MapSystem.Placeholders.Step
{
    [Serializable]
    public class SpawnerConfig
    {
        [SerializeField] private List<EnemySpawnInfo> handlers;
        [SerializeField] private Trigger trigger;
        private PanelsCreator _panelsCreator;
        private bool _initialized;

        public SpawnerConfig BindPanelsCreator(PanelsCreator panelsCreator)
        {
            _panelsCreator = panelsCreator;
            return this;
        }

        public void Initialize()
        {
            var canInitialized = CheckBind(_panelsCreator);
            if (!canInitialized) return;
            if (_initialized) return;
            _initialized = true;
            trigger.Initialize(InvokeSpawnRoutine);
            Debug.LogWarning("SpawnerConfig: spawners initialization completed");
        }

        public void Update(Vector3 position) => trigger.Update(position);

        private bool CheckBind<T>(T obj)
        {
            if (obj != null) return true;
            Debug.LogError($"TriggerConfig: {obj} is null");
            return false;
        }

        private BaseCharacter Spawn(EnemyData enemyData, Vector3 position, List<MapperItem> states)
        {
            var data = enemyData.Data;
            var enemy = Object.Instantiate(enemyData.Character, position, Quaternion.identity);
            if (enemy is IInitableSpawn ins)
                ins.Init(Spawn);

            if (enemy is IMoneyHandler mh)
                mh.InitMoney(enemyData.MoneyForDeath);

            enemy.BindStates(states);
            data.SetInteractable(enemy);
            enemy.SetCharacterData(data);
            _panelsCreator.AddCharacter(enemy);
            enemy.OnCreated();
           
            return enemy;
        }

        private void InvokeSpawnRoutine()
        {
            foreach (var info in handlers)
            {
                foreach (var position in info.Positions)
                {
                    Spawn(info.Data, position.Value, info.Data.States);
                }
            }
        }

        public void Draw()
        {
            if (handlers.Count == 0 && handlers[0] == null) return;
            foreach (var spawnInfo in handlers)
            {
                if (spawnInfo.Positions.Count == 0 && spawnInfo.Positions[0] == null) return;
                foreach (var position in spawnInfo.Positions)
                {
                    trigger.BindColor(Color.yellow);
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(position.Value, 0.5f);
                    trigger.Draw();
                }
            }
        }
    }
}