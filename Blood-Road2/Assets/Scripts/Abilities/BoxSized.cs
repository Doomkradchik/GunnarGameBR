using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BoxSized : MonoBehaviour
{
    [SerializeField] private int delay;
    async void Start()
    {
        await Task.Delay(delay);
        transform.DOScale(new Vector3(6, 1, 6), 0.5f);
    }
}
