using Characters.EffectSystem;
using Characters.Player;
using UnityEngine;

namespace Characters.BottlesSystem
{
    public delegate void BottleUse(Receiver receiver, EffectData data);
    public class BottleListener : MonoBehaviour
    {
        [SerializeField] private PlayerController character;
        private Sender _sender;

        private void Awake()
        {
            _sender = character.Sender;
            character.BottleUseEvent += OnBottle;
        }

        private void OnBottle(Receiver receiver, EffectData data)
        {
            _sender.RegisterData(data);
            _sender.RegisterReceiver(receiver);
        }
    }
}