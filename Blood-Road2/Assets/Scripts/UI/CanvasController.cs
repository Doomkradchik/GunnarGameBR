using System.Collections.Generic;
using UI;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private CanvasGroupPair[] pairs;
    [SerializeField] private string keyDefault;
    private Dictionary<string, CanvasGroup[]> _canvasGroups = new Dictionary<string, CanvasGroup[]> ();
    private RechangePanel _changePanel;
    

    private void Start()
    {
        foreach (var pair in pairs)
        {
            _canvasGroups.Add(pair.key, pair.values);
            foreach(var panel in pair.values)
                RechangePanel.Disable(panel);
        }

        _changePanel = new RechangePanel();
        _changePanel.SetNewPanel(_canvasGroups[keyDefault]);
    }

    public void SetPanel(string key) => _changePanel.SetNewPanel(_canvasGroups[key]);

    [System.Serializable]
    public struct CanvasGroupPair
    {
        public string key;
        public CanvasGroup[] values;
    }
}