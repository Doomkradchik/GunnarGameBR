using System.Collections.Generic;
using Cinemachine;
using MapSystem;
using UI.EnemyesCanvas;
using UnityEngine;
using LocationInfo = Scriptable_objects.LocationInfo;

namespace Location
{
    public class StepSpawner : MonoBehaviour
    {
        [SerializeField] private List<Step> steps;
        [SerializeField] private LocationInfo locationInfo;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PanelsCreator panelsCreator;
        [SerializeField] private Transform player;

        private void Awake()
        {
            var stepPrefab = locationInfo.CurrentProgress >= steps.Count
                ? steps[^1]
                : steps[locationInfo.CurrentProgress];
            var step = Instantiate(stepPrefab);
            step.BindCameraController(cameraController);
            step.BindPanelsCreator(panelsCreator);
            step.BindPlayerTransform(player);
            step.BindLocation(locationInfo);
            step.Initialize();
        }
    }
}