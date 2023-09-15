using System;
using System.Collections.Generic;
using Characters;
using Characters.InteractableSystems;
using Cinemachine;
using UnityEngine;
using LocationInfo = Scriptable_objects.LocationInfo;

namespace MapSystem.Placeholders.Step
{
    [Serializable]
    public sealed class TriggersConfig
    {
        [SerializeField] private List<Trigger> abilities;
        [SerializeField] private Trigger cameraBoss;
        [SerializeField] private Trigger finish;
        private LocationInfo _currentLocation;
        private CameraController _cameraController;
        private ITriggerable _triggerable;
        private bool _initialized;


        public void BindLocation(LocationInfo location) => _currentLocation = location;
        public void BindTriggerable(ITriggerable triggerable) => _triggerable = triggerable;
        public void BindCameraController(CameraController cameraController) => _cameraController = cameraController;

        public void Update(Vector3 position)
        {
            foreach (var ability in abilities)
            {
                ability.Update(position);
                cameraBoss.Update(position);
                finish.Update(position);
            }
        }

        public void Draw()
        {
            cameraBoss.BindColor(Color.magenta);
            cameraBoss.Draw();

            finish.BindColor(Color.green);
            finish.Draw();

            if (abilities.Count == 0 || abilities[0] == null) return;
            foreach (var ability in abilities)
            {
                ability.BindColor(Color.cyan);
                ability.Draw();
            }
        }

        private bool CheckBind<T>(T obj)
        {
            if (obj != null) return true;
            Debug.LogError($"TriggerConfig: {obj} is null");
            return false;
        }

        public void Initialize()
        {
            var canInitialized = CheckBind(_cameraController) && CheckBind(_triggerable) && CheckBind(_currentLocation);
            if (!canInitialized) return;
            if (_initialized) return;
            _initialized = true;
            cameraBoss.Initialize(OnBoss, OnExit);
            finish.Initialize(OnFinish);
            if (abilities.Count > 0)
            {
                foreach (var ability in abilities)
                {
                    ability.Initialize(OnAbilities, OnExit);
                }
            }

            Debug.LogWarning("Trigger: Initialized Complete");
        }

        private void OnAbilities()
        {
            var canInitialized = CheckBind(_cameraController) && CheckBind(_triggerable);
            if (!canInitialized) return;
            _cameraController.AbilityCamera(true);
            _triggerable.AbilityTrigger();
        }

        private void OnBoss()
        {
            if (!CheckBind(_cameraController)) return;
            _cameraController.BossCamera(true);
        }

        private void OnFinish()
        {
            var canInitialized = CheckBind(_cameraController) && CheckBind(_triggerable);
            if (!canInitialized) return;
            _cameraController.AbilityCamera(true);
            _currentLocation.AddProgressValue(1);
            _triggerable.Finish();
        }

        private void OnExit()
        {
            if (!CheckBind(_cameraController)) return;
            _cameraController.BossCamera(false);
            _cameraController.AbilityCamera(false);
        }
    }
}