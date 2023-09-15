using System;

namespace Characters.EffectSystem
{
    [Serializable]
    public class Sender
    {
        private EffectData _effectData;
        public void RegisterData(EffectData data)
        {
            _effectData=data;
        }

        public void RegisterReceiver(Receiver receiver)
        {
            receiver.Receive(_effectData);
        }
    }
}