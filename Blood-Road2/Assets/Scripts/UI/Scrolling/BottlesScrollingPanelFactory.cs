using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scriptable_objects
{

    public interface IPerformable<T> where T: ScriptableObject
    {
        void Perform(T data);
    }

    public class PanelFactory<TInfo, TPerformer> : ScrollingPanelFactory where
        TInfo : ScriptableObject where TPerformer : MonoBehaviour, IPerformable<TInfo>
    {
        public TInfo[] infos;
        public TPerformer prefab;
        public override Transform CreatePanel(int offset, Transform root)
        {
            var instance = Instantiate(prefab, root, false);
            instance.Perform(infos[offset]);
            return instance.transform;
        }

        public override int Count => infos.Length;
        public override float XScale => prefab.GetComponent<RectTransform>().sizeDelta.x;
    }

    [CreateAssetMenu(fileName = "BottlesScrollingPanelFactory", menuName = "Factories/ BottlesScrollingPanelFactory")]
    public sealed class BottlesScrollingPanelFactory : PanelFactory<BottleStoreEntityInfo, BottleStoreEntity>
    {
        public override Transform CreatePanel(int offset, Transform root)
        {
            var panel = base.CreatePanel(offset, root);
            panel.GetComponent<BottleStoreEntity>().Init(MoneyBank.Instance.MoneyInstance);
            return panel;
        }
    }
}
