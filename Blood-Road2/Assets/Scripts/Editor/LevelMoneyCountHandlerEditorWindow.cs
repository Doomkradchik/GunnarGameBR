using Location;
using MapSystem.Placeholders.Step;
using SpawnersR = MapSystem.Placeholders.Step.Spawners;
using System.Collections.Generic;
using Characters.Enemy;
using Spawners;
using UnityEditor;
using UnityEngine;
using LocationInfo = Scriptable_objects.LocationInfo;
using System.Reflection;
using System;

public class LevelMoneyCountHandlerEditorWindow : EditorWindow
{
    private StepSpawner _stepSpawner;
    private LocationInfo _levelTargetSO;
    [MenuItem("Window/LevelMoneyCountHandler")]
    public static void OpenWindow()
    {
        var window = EditorWindow.GetWindow(typeof(LevelMoneyCountHandlerEditorWindow));
        window.Show();
    }

    private void OnGUI()
    {
        _stepSpawner = (StepSpawner)EditorGUILayout.ObjectField(_stepSpawner, typeof(StepSpawner), true);
        _levelTargetSO = (LocationInfo)EditorGUILayout.ObjectField(_levelTargetSO, typeof(LocationInfo), true);
        if (GUILayout.Button("Set amout of money for all steps"))
        {
            var moneyCount = GetMoneyAmountForLevel();
            _levelTargetSO.GetType().GetField("moneyInLocation", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(_levelTargetSO, moneyCount);
            EditorUtility.SetDirty(_levelTargetSO);
            Debug.Log($"Set successfully. Amount: {moneyCount}");
        }
    }

    private int GetMoneyAmountForLevel()
    {
        var result = 0;
        try
        {
            var type = _stepSpawner.GetType();
            var field = type.GetField("steps", BindingFlags.Instance | BindingFlags.NonPublic);
            var steps = field.GetValue(_stepSpawner) as List<Step>;
            foreach (var step in steps)
            {
                var spawners = step.GetType().GetField("spawner", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(step) as SpawnersR;
                var spawnerConfigs = spawners.GetType().GetField("spawners", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spawners) as List<SpawnerConfig>;
                foreach (var config in spawnerConfigs)
                {
                    var enemySpawnInfos = config.GetType().GetField("handlers", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(config) as List<EnemySpawnInfo>;
                    foreach (var info in enemySpawnInfos)
                    {
                        if (info.Data.Character is not DefaultEnemy de)
                            throw new System.InvalidOperationException();
                        var moneyForOne = (int)de.GetType().GetField("moneyCountAfterDeath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(de);
                        var count = info.Positions.Count;
                        result += moneyForOne * count;
                    }
                }
            }
        }
        catch(Exception e)
        {    
            Debug.LogError(e.Message + e.InnerException);
        }
        return result;
    }
}