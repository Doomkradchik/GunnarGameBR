using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EventsDeleteInFBX : AssetPostprocessor
    {
        [SerializeField] private GameObject[] fbx;

        private void OnPostprocessAnimation(GameObject root, AnimationClip clip)
        {
            for (int i = 0; i < clip.events.Length; i++)
            {
                clip.events[i] = null;
            }
        }
    }
}