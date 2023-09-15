using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scriptable_objects;

namespace UI.Scrolling
{
    public class SnapScrolling : MonoBehaviour
    {
        [SerializeField] private ScrollingPanelFactory factory;
        private int _panelsCount;
        [Range(0, 500)] [SerializeField] private int panelOffset;


        [SerializeField] private float y_offset;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private float snapSpeed;
        [SerializeField] private float scaleOffset;
        [SerializeField] private float scaleSpeed;
        [SerializeField] private float offsetDistance;

        private Transform[] _panels;
        private Vector2Ref[] _panelsPosition;
        private RectTransform _contentRect;
        private int _selectedPanelID;
        private bool _isScrolling;
        private Vector2 _contentVector;
        private Vector2Ref[] _panelsScale;
        private Vector2 _scale;

        private class Vector2Ref
        {
            public Vector2 Value { get; set; }
        }

        private void Start()
        {
            _panelsCount = factory.Count;
            _panels = new Transform[_panelsCount];
            _panelsPosition = new Vector2Ref[_panelsCount];
            _contentRect = GetComponent<RectTransform>();
            _panelsScale = new Vector2Ref[_panelsCount];

            for (var i = 0; i < _panelsCount; i++)
            {
                var currentPanel = factory.CreatePanel(i, transform);
                if (i == 0)
                {
                    _panels[i] = currentPanel;
                    _panelsPosition[i] = new Vector2Ref();
                    _panelsPosition[i].Value = -currentPanel.transform.localPosition + Vector3.up * y_offset;
                    _panelsScale[i] = new Vector2Ref();
                    _scale = currentPanel.transform.localScale;
                    continue;
                }

                var previousPanel = _panels[i - 1].transform != null
                    ? _panels[i - 1].transform.localPosition.x
                    : 0;
                currentPanel.transform.localPosition =
                    new Vector2(
                        previousPanel +
                        factory.XScale * offsetDistance + // .GetComponent<RectTransform>().sizeDelta.x
                        panelOffset, currentPanel.transform.localPosition.y);
              
                _panelsPosition[i] = new Vector2Ref();
                _panelsPosition[i].Value = -currentPanel.transform.localPosition;
                _panels[i] = currentPanel;
                _panelsScale[i] = new Vector2Ref();
            }

        }

        private void FixedUpdate()
        {
            if (_contentRect.anchoredPosition.x >= _panelsPosition[0].Value.x && !_isScrolling ||
                _contentRect.anchoredPosition.x <= _panelsPosition[^1].Value.x && !_isScrolling)
            {
                scrollRect.inertia = false;
            }

            var nearestPosition = float.MaxValue;
            for (var i = 0; i < _panelsCount; i++)
            {
                var distance = Math.Abs(_contentRect.anchoredPosition.x - _panelsPosition[i].Value.x);
                if (distance < nearestPosition)
                {
                    nearestPosition = distance;
                    _selectedPanelID = i;
                }

                var scale = Mathf.Clamp(1 / (distance / panelOffset) * scaleOffset, 0.5f, 1f);
                var currentPanel = _panels[i];
                var currentPanelScale = _panelsScale[i];
                var xScale =
                    Mathf.SmoothStep(currentPanel.transform.localScale.x, scale * _scale.x, scaleSpeed * Time.fixedDeltaTime);
                var yScale =
                    Mathf.SmoothStep(currentPanel.transform.localScale.y, scale * _scale.y, scaleSpeed * Time.fixedDeltaTime);
                currentPanelScale.Value = new Vector2(xScale, yScale);
                currentPanel.transform.localScale = currentPanelScale.Value;
            }

            var scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
            if (scrollVelocity < 400 && !_isScrolling)
            {
                scrollRect.inertia = false;
            }

            if (_isScrolling || scrollVelocity > 400) return;
            _contentVector.x =
                Mathf.SmoothStep(_contentRect.anchoredPosition.x, _panelsPosition[_selectedPanelID].Value.x,
                    snapSpeed * Time.fixedDeltaTime);
            _contentRect.anchoredPosition = _contentVector;
        }

        public void Scroll(bool scroll)
        {
            _isScrolling = scroll;
            if (scroll) scrollRect.inertia = true;
        }
    }
}