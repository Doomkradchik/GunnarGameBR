using UnityEngine.UI;
using UnityEngine;
using UI;
using UnityEngine.EventSystems;

public class UndoButton : Button
{
    [SerializeField] private CanvasGroup[] panels;

    [SerializeField] private CanvasGroup scrollPanel;

    private RechangePanel _changePanel;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if(interactable)
            _changePanel.SetNewPanel(scrollPanel);
    }

    private void OnScrollPanel()
    {
        _changePanel.SetNewPanel(scrollPanel);
    }
}
