
using Characters.Animations;
using Characters.Sounds;
using MapSystem.Structs;

namespace Characters.Player.States
{
    public class Weakness : BaseState
    {
        public Weakness()
        {

        }

        public Weakness(IAnimationCommand animation, IAudioCommand audio, View view, VFXTransforms vfxTransforms) : base(animation, audio, view, vfxTransforms)
        {
            _parameterName = "weakness";
        }

        public override void Enter()
        {
            base.Enter();
            _animation.SetAnimation(_parameterName);
        }

        public override void Tick(float tickTime)
        {
          
        }
    }
}
