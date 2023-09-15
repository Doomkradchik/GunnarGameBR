using System;
using System.Collections.Generic;
using System.IO;
using Characters;
using Characters.Enemy;
using MapSystem;
using Spawners;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(DefaultEnemy))]

public class BaseCharacterEditor : UnityEditor.Editor
{
    private BaseCharacter _baseCharacter;

    private void OnEnable()
    {
        _baseCharacter = target as BaseCharacter;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Set CharacterController")) _baseCharacter.SetCharacterController();
    }

}
[CanEditMultipleObjects]
[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : UnityEditor.Editor
{
    private EnemyData _enemyData;

    private void OnEnable()
    {
        _enemyData = target as EnemyData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Set states"))FindStates();
    }

    private void FindStates()
    {
        string[] assetGUIDs = AssetDatabase.FindAssets("t:ScriptableObject", new[] {$"Assets/Resources/Enemies/{_enemyData.EnemyClass}/{_enemyData.name}" });
        foreach (string assetGUID in assetGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGUID);
            MapperItem assetObject = AssetDatabase.LoadAssetAtPath<MapperItem>(assetPath);

            if (assetObject != null)
            {
                _enemyData.AddState(assetObject);
            }
        }
    }
}
