using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class RechangePanel
    {
        private List<CanvasGroup> _currentCanvasGroups = new List<CanvasGroup>();

        public void SetNewPanel(params CanvasGroup[] newPanels)
        {
            if (_currentCanvasGroups.Count != 0)
            {
                foreach(var cg in _currentCanvasGroups)
                    Disable(cg);

                _currentCanvasGroups.Clear();
            }

            foreach(var newP in newPanels)
            {
                _currentCanvasGroups.Add(newP);
                newP.transform.DOScale(Vector3.one, 0.1f);
                newP.DOFade(1, 0.1f);
                newP.interactable = true;
                newP.blocksRaycasts = true;
            }
        }

        public static void Disable(CanvasGroup panel)
        {
            panel.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f);
            panel.DOFade(0, 0.1f);
            panel.interactable = false;
            panel.blocksRaycasts = false;
        }
    }
}