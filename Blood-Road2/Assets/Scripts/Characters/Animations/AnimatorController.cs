using MapSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.Animations
{
    public class AnimatorController : MapperBase<string, AnimationClip>, IAnimationCommand
    {
        private Animator _animator;
        private Animation _animation;
        private bool _isDeath;
        private AnimationChanger _animationChanger;


        public AnimatorController(Animator animator)
        {
            _animator = animator;
            _animation = animator.AddComponent<Animation>();
        }

        public override void AddValue(string key, AnimationClip value)
        {
            base.AddValue(key, value);
            value.events = null;
        }

        public float LengthAnimation(string nameClip)
        {
            if (_animator == null) return 0;
            var speed = _animation.IsPlaying(nameClip) ? _animator.GetFloat("AttackSpeed") : 1;
            var length = _dictionary[nameClip].length / speed;
            return length;
        }

        public void CreateAnimationChanger(AnimatorOverrideController overrideController)
        {
            _animationChanger = new AnimationChanger(overrideController, _animator);
        }

        public void SetSpeedAnimation(float value)
        {
            _animator.SetFloat("AttackSpeed", value);
        }

        public void Die(string nameClip)
        {
            SetAnimation(nameClip);
            _isDeath = true;
        }

        public void SetAnimation(string nameClip)
        {
            bool success = _dictionary.TryGetValue(nameClip, out var clip);
            if (success == false) return;
            if (_isDeath) return;
            _animation.Stop();
            _animationChanger.SetAnimation(_animator, clip);
            _animation.clip = clip;
            _animation.Play();
        }

        public void SetTransitionTime(float time)
        {
            _animationChanger.SetTransitionTime(time);
        }
    }
}