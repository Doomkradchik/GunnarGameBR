using MapSystem;
using UnityEngine;

namespace Characters.Animations
{
    public interface ITransitionTimeHandler
    {
        public void SetTransitionTime(float time);
    }
    public interface IAnimationCommand: IMapper<string, AnimationClip>, ITransitionTimeHandler
    {
        public float LengthAnimation(string nameClip);
        public void CreateAnimationChanger(AnimatorOverrideController overrideController);
        public void SetAnimation(string nameClip);
        public void Die(string nameClip);
        public void SetSpeedAnimation(float value);
    }
}