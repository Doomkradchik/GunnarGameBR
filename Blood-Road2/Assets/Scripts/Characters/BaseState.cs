using System;
using Characters.Animations;
using Characters.Sounds;
using MapSystem;
using MapSystem.Structs;
using UnityEngine;
using baseState=Better.UnityPatterns.Runtime.StateMachine.States.BaseState;

public interface ISerializableStates{}
namespace Characters
{
    [Serializable]
    public abstract class BaseState: baseState, ISerializableStates
    {
        protected IAnimationCommand _animation;
        protected IAudioCommand _audio;
        protected AnimationClip _animationClip;
        protected AudioClip _audioClip;
        protected AudioParameters _audioParameters;
        protected VFXEffect _vfxEffect;
        protected VFXTransforms _vfxTransforms;
        protected string _parameterName;
        protected bool _useAnimationLine;
        protected float _animationLine;

        protected virtual bool PlayOnEnter => false;

        protected int AbsuluteDuration => Mathf.Abs((short)SecondToMilliseconds(_animation.LengthAnimation(_parameterName)));

        protected BaseState(){}
        protected BaseState(IAnimationCommand animation, IAudioCommand audio, View view, VFXTransforms vfxTransforms)
        {
            _animation = animation;
            _vfxTransforms = vfxTransforms;
            _animationClip = view.Animation;
            _audioParameters = view.AudioParameters;
            _vfxEffect = view.Effect;
            _audio = audio;
            _audioClip = view.AudioClip;
            _useAnimationLine = view.UseAnimationLine;
            _animationLine = view.AnimationLine;
        }

        public override void Enter()
        {
            if (_animationClip != null)
            {
                _animation.AddValue(_parameterName, _animationClip);
            }

            if (_audioClip != null)
            {
                _audio.AddValue(_parameterName, _audioClip);
                _audio.SetAudio(_parameterName);
                if (PlayOnEnter) _audio.Play(_audioParameters);
            }

        }

        public virtual void SetStateInfo(Item item)
        {
            _animationClip = item.View.Animation;
            _parameterName = _animationClip.name;
            _vfxEffect = item.View.Effect;
        }


        public sealed override void Exit()
        {
            if(_audio != null) _audio.Stop();
            OnExit();
        }

        protected virtual void OnExit() { }

        private const int SECONDS = 1000;

        protected static int SecondToMilliseconds(float second)
        {
            var result = Mathf.RoundToInt(second * SECONDS);
            return result;
        }
    }
}