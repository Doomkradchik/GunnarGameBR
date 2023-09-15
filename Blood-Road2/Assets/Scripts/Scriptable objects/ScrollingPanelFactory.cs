using UnityEngine;
using UI.Scrolling;

namespace Scriptable_objects
{
    public abstract class ScrollingPanelFactory : ScriptableObject
    {
        public abstract int Count { get; }
        public abstract float XScale { get; }
        public abstract Transform CreatePanel(int offset, Transform root);
    }
}
