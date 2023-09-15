using System.Threading.Tasks;
using Characters.Animations;
using Characters.Player;
using Characters.Sounds;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class Fury: AbilityBase
    {
        private CharacterData _characterData;
        public Fury(){}

        public Fury(IAnimationCommand animation, IAudioCommand audio, View view, VFXTransforms vfxTransforms) : base(animation, audio, view, vfxTransforms)
        {
        }

        public void SetCharacterData(CharacterData characterData)
        {
            _characterData = characterData;
        }
        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null) return;
            var effect = GameObject.Instantiate(_vfxEffect,_vfxTransforms.Center);
            effect.transform.rotation  = Quaternion.identity;
            effect.SetLifeTime(15f);
            Wait();
            CanSkip = true;
        }

        private async void Wait()
        {
       //     _characterData.SetAdditionalHealthAfterDamage(true);
            await Task.Delay(SecondToMilliseconds(15f));
         //   _characterData.SetAdditionalHealthAfterDamage(false);
        }

        public override void Tick(float tickTime)
        {
            
        }

    }
}