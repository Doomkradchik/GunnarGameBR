using Scriptable_objects;
using UnityEngine;
using Spawners;

[CreateAssetMenu(fileName = "EncyclopediaPanelFactory", menuName = "Factories/ EncyclopediaPanelFactory")]
public sealed class EncyclopediaPanelFactory : PanelFactory<EnemyData, EncyclopediaEntityView>
{
    [Header("Set ONLY (Enemies Data Needed Only)")]
    [SerializeField] private EnemiesData enemiesData;

    public override int Count => enemiesData.GetList.Count;

    public override Transform CreatePanel(int offset, Transform root)
    {
        if (infos.Length != 0)
            throw new System.InvalidOperationException("Enemies Data Needed Only");

        var instance = Instantiate(prefab, root, false);
        instance.Perform(enemiesData.GetList[offset]);
        return instance.transform;
    }
}
