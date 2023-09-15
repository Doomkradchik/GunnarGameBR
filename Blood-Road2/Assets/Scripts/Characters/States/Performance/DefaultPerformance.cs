using Characters.Animations;
using Characters.Sounds;
using MapSystem.Structs;
using System.Threading.Tasks;
using UnityEngine;

namespace Characters.Player.States
{
    public class DefaultPerformance : BaseState
    {
        protected int _duration;
        protected readonly Enable _onEnable;
        protected readonly GameObject _model;
        protected readonly VFXEffect _effect;
        protected PerformanceSubstate _substate;
        protected string _appearingParameterName;


        protected virtual double RatioEnter => 1.0;
        protected virtual double RatioExit => 1.0;

        public enum PerformanceSubstate : short
        {
            None,
            Disappeared,
            Appeared
        }

        public DefaultPerformance()
        {
        }

        public DefaultPerformance(Enable onEnableCharacter, GameObject model, IAnimationCommand animation, IAudioCommand audio, View view, VFXTransforms vfxTransforms) : base(animation, audio, view, vfxTransforms)
        {
            _parameterName = "performance";
             _onEnable = onEnableCharacter;
            _model = model;
            _effect = view.Effect;
        }

        public void SetAppearingClip(AnimationClip clip) // TEST
        {
            _appearingParameterName = clip.name;
            _animation.AddValue(_appearingParameterName, clip);
        }

        public async override void Enter()
        {
            base.Enter();
            _substate = PerformanceSubstate.None;
            _animation.SetAnimation(_parameterName);
            _duration = AbsuluteDuration;
            await Task.Delay((int)(_duration * RatioEnter));
            await Perform(false);
            _onEnable.Invoke(false);
            _substate = PerformanceSubstate.Disappeared;
        }

        protected virtual async Task Perform(bool perform)
        {
            //_model.SetActive(perform);
            await Task.Delay(0);
        }

        public override void Tick(float tickTime)
        {
        }

        protected async void EnableCharacter()
        {
            _substate = PerformanceSubstate.None;
            await Perform(true);
            _animation.SetAnimation(_appearingParameterName);
            if (_effect != null)
            {
                var effectCopy = Object.Instantiate(_effect);
                effectCopy.transform.position = _vfxTransforms.Down.position;
            }
            _onEnable.Invoke(true);
            await Task.Delay((int)(_duration * RatioExit));
            _substate = PerformanceSubstate.Appeared;
        }

        protected void SetPosition(Vector3 position) => _model.transform.parent.position = position;

        public bool Appear(System.Func<bool> condition, GetTarget positionProvider = null)
        {
            if (condition.Invoke() && _substate == PerformanceSubstate.Disappeared)
            {
                if (positionProvider != null) SetPosition(positionProvider.Invoke());
                EnableCharacter();
            }     

            return condition.Invoke() && _substate == PerformanceSubstate.Appeared;
        }
    }
}