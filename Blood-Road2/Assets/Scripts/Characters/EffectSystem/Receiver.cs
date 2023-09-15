using System;
using Characters.Facades;

namespace Characters.EffectSystem
{
    [Serializable]
    public class Receiver
    {
        private IAnimatableEffect _animatable;
        private EffectData _effectData;
        private Applyer _applyer;
        public void Initialize(IAnimatableEffect animatable, Applyer applyer)
        {
            _animatable = animatable;
            _applyer = applyer;
        }

        private void RegisterAnimator(Type type)
        {
           if(type != null) _animatable.SetCurrentEffectID(type);
            _applyer.RechangeCharacterDataValues(_effectData);
        }

        public void Receive(EffectData data)
        {
            _effectData = data;
            RegisterAnimator(_effectData.Type);
        }
    }
}