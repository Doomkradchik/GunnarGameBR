using Characters.Facades;
using Characters.Player;
using UnityEngine;

namespace Characters.EffectSystem
{
    public class Linker : MonoBehaviour
    {
        [SerializeField] private Receiver _receiverEffect;
        [SerializeField] private Applyer _applyerEffect;
        public Receiver Receiver => _receiverEffect;

        public void Initialize(IAnimatableEffect animatableEffect, CharacterData characterData)
        {
            _receiverEffect.Initialize(animatableEffect, _applyerEffect);
            _applyerEffect.Initialize(characterData);
        }
    }
}