using System.Collections.Generic;
using Characters.InteractableSystems;
using Characters.Player;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters.Facades
{
    public class InteractionSystem
    {
        private IInit<SetCurrentPoint> _instCurrentPointCameraRay;
        private Eyes _instEyes;
        private List<IInteractable> _points;
        private SetPoint _setPoint;
        private SetCurrentPoint _setCurrentPoint;
        private event SetCurrentPoint _setCurrentPointInPlayerEvent;
        private event StartRechangeCurrentPoint _rechangeCurrentPointInPlayerEvent;

        public void Initialize([CanBeNull] CameraRay cameraRay, Eyes eyesCharacters, Transform transform,
            SetCurrentPoint setCurrentPointDelegate, StartRechangeCurrentPoint rechangeCurrentPointDelegate)
        {
            _setCurrentPointInPlayerEvent = setCurrentPointDelegate;
            setCurrentPointDelegate += _setCurrentPointInPlayerEvent;

            _rechangeCurrentPointInPlayerEvent = rechangeCurrentPointDelegate;
            rechangeCurrentPointDelegate += _rechangeCurrentPointInPlayerEvent;

            _points = new List<IInteractable>();

            _setPoint = SetPoint;
            _setCurrentPoint = SetCurrentPoint;
            if (cameraRay != null)
            {
                var camera = Object.Instantiate(cameraRay);
                _instCurrentPointCameraRay = camera;
                _instCurrentPointCameraRay.Subscribe(_setCurrentPoint);
            }

            _instEyes = Object.Instantiate(eyesCharacters, transform);
            _instEyes.Subscribe(_setPoint);
        }

        private void SetPoint(List<IInteractable> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (!_points.Contains(points[i]))
                {
                    _points.Add(points[i]);
                }
            }

            if (_points.Count != 0) _rechangeCurrentPointInPlayerEvent?.Invoke(_points);
        }

        private void SetCurrentPoint(IInteractable point)
        {
            if (point.Enabled == false) { return; }
            if (!_points.Contains(point))
            {
                _points.Add(point);
            }

            _setCurrentPointInPlayerEvent?.Invoke(point);
        }
    }
}