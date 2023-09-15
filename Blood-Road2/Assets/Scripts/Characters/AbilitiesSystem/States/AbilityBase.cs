using Characters.Animations;
using Characters.Sounds;
using MapSystem.Structs;

namespace Characters.AbilitiesSystem.States
{
    public abstract class AbilityBase : BaseState
    {
        public bool CanSkip { get; protected set; }
        public AbilityBase(){}

        protected AbilityBase(IAnimationCommand animation, IAudioCommand audio,  View view, VFXTransforms vfxTransforms) : base(animation, audio, view, vfxTransforms)
        {
        }
    }
}