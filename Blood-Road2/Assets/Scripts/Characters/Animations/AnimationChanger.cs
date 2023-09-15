using DG.Tweening;
using UnityEngine;

namespace Characters.Animations
{
    public class AnimationChanger : ITransitionTimeHandler
    {
        private AnimatorOverrideController animatorController;
        private AnimationClipOverrides _clipOverrides;
        private TransitionAndStateAnimatorChanger _transitionAndStateAnimatorChanger;
        private bool _forward = true;
        private AnimationClip _currentClip;
        private static readonly int CurrentState = Animator.StringToHash("CurrentState");

        public AnimationChanger(RuntimeAnimatorController animatorController, Animator animator)
        {
            this.animatorController = new AnimatorOverrideController(animatorController);
            _clipOverrides = new AnimationClipOverrides(this.animatorController.overridesCount);
            _transitionAndStateAnimatorChanger = new TransitionAndStateAnimatorChanger(_clipOverrides, animator);
            this.animatorController.GetOverrides(_clipOverrides);
        }



        public void SetAnimation(Animator animator, AnimationClip clip)
        {
            if (animator == null) return;
            if (!_transitionAndStateAnimatorChanger.CanSetInfo(clip)) return;
            animator.runtimeAnimatorController = animatorController;
            _transitionAndStateAnimatorChanger.SetNewInfo(clip);
            animator.Rebind();
            animatorController.ApplyOverrides(_clipOverrides);
        }

        public void SetTransitionTime(float time)
        {
            _transitionAndStateAnimatorChanger.SetTransitionTime(time);
        }

        private class TransitionAndStateAnimatorChanger : ITransitionTimeHandler
        {
            private AnimationClipOverrides _overrides;
            private Animator _animator;
            private string _currentState;
            private float _transitionTime = 0.2f;
            private const string NAME_STATE1 = "state 1";
            private const string NAME_STATE2 = "state 2";

            public TransitionAndStateAnimatorChanger(
                AnimationClipOverrides overrides, Animator animator)
            {
                _animator = animator;
                _overrides = overrides;
            }

            public bool CanSetInfo(AnimationClip clip)
            {
                var isClip = _overrides[_currentState] != clip;
                return isClip;
            }


            public void SetNewInfo(AnimationClip clip)
            {
                if (_currentState != null)
                {
                    var isState2 = _currentState == NAME_STATE2;
                    _currentState = isState2 ? NAME_STATE1 : NAME_STATE2;
                    if (DOTween.instance == null) return;
                    var startValue = isState2 ? 1 : 0;
                    var endValue = startValue == 0 ? 1 : 0;
                    DOTween.To(value => _animator.SetFloat(CurrentState, value), startValue, endValue, _transitionTime);
                }
                else
                {
                    _currentState = NAME_STATE1;
                }

                _overrides[_currentState] = clip;
            }

            public void SetTransitionTime(float time)
            {
                _transitionTime = time;
            }
        }
    }
}