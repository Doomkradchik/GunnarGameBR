using System;
using Characters;
using Cinemachine;
using MapSystem;
using MapSystem.Placeholders.Step;
using UI.EnemyesCanvas;
using UnityEngine;
using UnityEngine.Serialization;
using LocationInfo = Scriptable_objects.LocationInfo;

namespace Location
{
    public class Step : MonoBehaviour
    {
        [SerializeField] private Triggers triggers;
        [SerializeField] private MapSystem.Placeholders.Step.Spawners spawner;

        public void BindPanelsCreator(PanelsCreator panelsCreator) => spawner.BindPanelsCreator(panelsCreator);

        public void BindCameraController(CameraController cameraController) =>
            triggers.BindCameraController(cameraController);



        public void BindPlayerTransform(Transform transform)
        {
            triggers.BindTransfromPlayer(transform);
            spawner.BindPlayerTransform(transform);
            if (transform.gameObject.TryGetComponent(out ITriggerable triggerable))
            {
                triggers.BindTriggerable(triggerable);
                return;
            }

            Debug.LogException(new Exception("Step: cannot get interface this object"), this);
        }

        public void BindLocation(LocationInfo locationInfo) => triggers.BindLocation(locationInfo);

        public void Initialize()
        {
            triggers.Initialize();
            spawner.Initialize();
        }

        private void FixedUpdate()
        {
            triggers.Update();
            spawner.Update();
        }

        private void OnDrawGizmos()
        {
            spawner.Draw();
            triggers.Draw();
        }
    }
}