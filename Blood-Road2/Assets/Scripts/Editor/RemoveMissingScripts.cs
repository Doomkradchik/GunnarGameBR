using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RemoveMissingScripts : UnityEditor.Editor
{
    [MenuItem("GameObject/Remove Missing Scripts")]
    public static void Remove()
    {
        var objs = Resources.FindObjectsOfTypeAll<GameObject>();
        int count = objs.Sum(GameObjectUtility.RemoveMonoBehavioursWithMissingScript);
        Debug.Log($"Removed {count} missing scripts");
    }
    [MenuItem("GameObject/Disabled All Colliders")]
    public static void RemoveColliders()
    {
        var objs = Resources.FindObjectsOfTypeAll<Collider>();
        var objss = Resources.FindObjectsOfTypeAll<MeshCollider>();
        for (int i = 0; i >= objs.Length; i++)
        {
            objs[i].enabled = false;
        }
        Debug.Log($"Disabled {objs.Length} colliders");
    }
    [MenuItem("GameObject/Find Monobehaviour Scripts")]

    public static void FindAssets()
    {
        Debug.Log(FindAssetsByType<ScriptableObject>().Count);
    }
    public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        for( int i = 0; i < guids.Length; i++ )
        {
            string assetPath = AssetDatabase.GUIDToAssetPath( guids[i] );
            T asset = AssetDatabase.LoadAssetAtPath<T>( assetPath );
            if( asset != null )
            {
                assets.Add(asset);
            }
        }
        return assets;
    }
}
