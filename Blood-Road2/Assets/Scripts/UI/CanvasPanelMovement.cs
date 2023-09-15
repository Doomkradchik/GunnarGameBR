using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class CanvasPanelMovement : MonoBehaviour
{
    [SerializeField] private PanelDataPair[] pairs;
    private Dictionary<string, CanvasGroup> _panels = new Dictionary<string, CanvasGroup>();
    private RechangePanel _changePanel;
    private void Awake()
    {
        foreach (var pair in pairs)
        {
            _panels.Add(pair.key, pair.value);
            RechangePanel.Disable(pair.value);
        }

        _changePanel = new RechangePanel();
    }

    public void MoveToPanel(string panelName)
    {
        _changePanel.SetNewPanel(_panels[panelName]);
    }

}

[System.Serializable]
public struct PanelDataPair
{
    public string key;
    public CanvasGroup value;
}