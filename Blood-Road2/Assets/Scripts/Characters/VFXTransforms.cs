using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters
{
    public class VFXTransforms : MonoBehaviour
    {
        [SerializeField] private Transform up;
        [SerializeField] private Transform center;
        [SerializeField] private Transform down;
        [SerializeField] [CanBeNull] private BaseCharacter character;
        public Action DieDelegate;
        public Transform Up => up;
        public Transform Center => center;
        public Transform Down => down;
        public BaseCharacter Character => character;

        private void Awake()
        {
            DieDelegate = OnEnable;
        }

        private void OnEnable()
        {
            for (int i = 0; i < up.childCount; i++)
            {
                Destroy(up.GetChild(i).gameObject);
            }

            for (int i = 0; i < center.childCount; i++)
            {
                var childTransform = center.GetChild(i);
                var child = childTransform.gameObject;
                Destroy(child);
            }

            for (int i = 0; i < down.childCount; i++)
            {
                Destroy(down.GetChild(i).gameObject);
            }
        }
    }
}