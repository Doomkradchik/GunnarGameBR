using System.Threading.Tasks;
using Characters.Animations;
using Characters.EffectSystem;
using MapSystem;
using MapSystem.Structs;
using Object = UnityEngine.Object;
using UnityEngine;
using Characters.Sounds;

namespace Characters.Player.States
{
    public class Attack : BaseState
    {
        private IInteractable _interactable;
        private TransitionAndStatesData _transitionAndStatesData;
        private EffectData _effectData;
        private float _animationSpeed = 1;
        private bool _setDamage;
        private bool _isPlayer;
        private bool _canSkip = true;
        private int _costAttack;


        private int _startDelayTime;
        protected Transform _vfxTransform;
        protected int _animationTimeInMileseconds;

        // private VFXSpawnType _currentVFXSpawnType = VFXSpawnType.Default;
        public bool CanSkip => _canSkip;

        public Attack()
        {
        }

        public Attack(IAnimationCommand animation, IAudioCommand audio, View statesInfo, bool isPlayer,
            TransitionAndStatesData data, VFXTransforms vfxTransforms, EffectData effectData) : base(
            animation, audio, statesInfo, vfxTransforms)
        {
            _effectData = effectData;
            _isPlayer = isPlayer;
            _parameterName = "attack";
            _transitionAndStatesData = data;
            _vfxTransform = _vfxTransforms.Center;
        }

        public override void SetStateInfo(Item item)
        {
            base.SetStateInfo(item);
            _effectData = item.Ability.EffectData;
            _costAttack = item.Ability.Cost;
        }

        public void SetAnimationSpeed(float value)
        {
            _animationSpeed = value;
        }

        public void SetThisCharacter(IInteractable point)
        {
            _interactable = point;
        }

        public override void Enter()
        {
            base.Enter();
            _animation.SetAnimation(_parameterName);
            _animation.SetSpeedAnimation(_animationSpeed);
            _animationTimeInMileseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName));
            _startDelayTime = _useAnimationLine ? (int)(_animationLine * _animationTimeInMileseconds) : (_animationTimeInMileseconds / 2);
            _setDamage = true;
            if (_isPlayer)
                InvokeDamage();
            else
                SetDamage();
        }

        private async void SetDamage()
        {
            do
            {
                _canSkip = false;
                await Task.Delay(_startDelayTime);
                var mileseconds = Mathf.Clamp(_animationTimeInMileseconds - _startDelayTime, 0, _animationTimeInMileseconds);
                _interactable.TryAttackByWeapon(_effectData, _costAttack, mileseconds / 2);
                _audio.Play(_audioParameters);
                VFXEffect vfx;
                if (_vfxEffect != null)
                {
                    vfx = Object.Instantiate(_vfxEffect);
                    vfx.transform.position = _vfxTransforms.Center.position;
                    vfx.SetLifeTime(1.5f);
                }
                await Task.Delay(mileseconds);
                _canSkip = true;
            } while (_setDamage);
        }

        private async void InvokeDamage()
        {
            _canSkip = false;
            VFXEffect vfx;
            if (_vfxEffect != null)
            {
                vfx = Object.Instantiate(_vfxEffect);
                vfx.transform.position = _vfxTransforms.Center.position;
                vfx.transform.rotation = Quaternion.Euler(0, _vfxTransforms.transform.eulerAngles.y, 0);
                vfx.SetLifeTime(1.5f);
            }
            _interactable?.TryAttackByWeapon(_effectData, _costAttack, _startDelayTime*2);
            await Task.Delay(_startDelayTime);
            _animation.SetAnimation(_parameterName);
            _audio.Play(_audioParameters);  
            _setDamage = false;
            _canSkip = true;
        }

        public bool CanUseEnergy() => _transitionAndStatesData.CharacterData.HasEnoughEnergy(_costAttack);


        public override void Tick(float tickTime)
        {
            _animation.SetSpeedAnimation(_animationSpeed);
        }

        protected override void OnExit()
        {
            _animationSpeed = 1;
            _setDamage = false;
            _canSkip = true;
        }
    }
}