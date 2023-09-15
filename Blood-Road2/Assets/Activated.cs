using System.Collections.Generic;
using UnityEngine;

public class Activated : MonoBehaviour
{
    public List<GameObject> AllObjects;
    private void Awake()
    {
        foreach (var aobject in AllObjects)
        {
             aobject.SetActive(true);
        }
    }
}
