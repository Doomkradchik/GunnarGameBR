using System.Threading.Tasks;
using Characters.Animations;
using Characters.Player;
using MapSystem.Structs;
using Characters.Sounds;

namespace Characters.AbilitiesSystem.States
{
    public class UniversalBlow : AbilityBase
    {
        private CharacterData _characterData;

        public UniversalBlow()
        {
        }


        private OverrideAttack _overrideAttack;
        protected Attack _attack;
        protected int _mileseconds;
        protected SetAttackSpeed _setAttackSpeed;
      //  protected Transform VfxTransform => _characterData.weaponTransforms.Center;

        public UniversalBlow(IAnimationCommand animation, IAudioCommand audio,  View view, VFXTransforms vfxTransforms) : base(animation, audio, view, vfxTransforms)
        {
            _parameterName = "UniversalBlow";
        }

        public override void Enter()
        {
            CanSkip = false;
            base.Enter();
            _animation.SetAnimation(_parameterName);
            if (_vfxEffect == null) return;
           // var effect = GameObject.Instantiate(_vfxEffect, _characterData.weaponTransforms.Center);
           // effect.SetLifeTime(SecondToMilliseconds(10f));
            WaitAnimation();
            Wait();
        }

        private async void WaitAnimation()
        {
            _mileseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName));
            await Task.Delay(_mileseconds);
            CanSkip = true;
        }

        private async void Wait()
        {
            float speed = 0.4f;
        /*    _overrideAttack?.Invoke(new View(_vfxEffect, _clip, (int)(_mileseconds * 0.4),
                VfxTransform, VFXSpawnType.UniversalBlow, speed), true);*/
            _setAttackSpeed?.Invoke(speed);
           // _characterData.IncreaseDamageIn(2);

            await Task.Delay(SecondToMilliseconds(10f));
          //  _overrideAttack?.Invoke(View.empty, false);
            _setAttackSpeed?.Invoke(1);
          ///  _characterData.IncreaseDamageIn(1);
        }

        public override void Tick(float tickTime)
        {
        }
    }
}